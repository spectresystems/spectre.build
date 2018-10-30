#load "../data.cake"
#load "../parameters.cake"
#load "../criterias.cake"

#load "build.cake"
#load "clean.cake"
#load "package.cake"
#load "publish.cake"
#load "restore.cake"
#load "test.cake"

// Define targets
Spectre.Tasks.Default = Task("Default");
Spectre.Tasks.BuildServer = Task("BuildServer");

public sealed class SpectreTasks
{
    private readonly ICakeContext _context;
    private readonly List<CakeTask> _engineTasks;

    public SpectreTasks(ICakeContext context, IReadOnlyList<ICakeTaskInfo> engineTasks)
    {
        _context = context;
        _engineTasks = (List<CakeTask>)engineTasks;
    }

    // Targets
    public CakeTaskBuilder Default { get; set; }
    public CakeTaskBuilder BuildServer { get; set; }

    // Tasks
    public CakeTaskBuilder Clean { get; set; }
    public CakeTaskBuilder CleanArtifacts { get; set; }
    public CakeTaskBuilder CleanBinaries { get; set; }
    public CakeTaskBuilder Restore { get; set; }
    public CakeTaskBuilder RestoreNuGetPackages { get; set; }
    public CakeTaskBuilder Build { get; set; }
    public CakeTaskBuilder BuildSolution { get; set; }
    public CakeTaskBuilder Test { get; set; }
    public CakeTaskBuilder RunUnitTests { get; set; }
    public CakeTaskBuilder Pack { get; set; }
    public CakeTaskBuilder Publish { get; set; }
    public CakeTaskBuilder PublishAppVeyorArtifacts { get; set; }
    public CakeTaskBuilder PublishNuGetPackages { get; set; }

    public void WireUp(ICakeContext context, BuildParameters parameters)
    {
        // Targets
        Default.IsDependentOn(Pack);
        BuildServer.IsDependentOn(Publish);

        // Remove unused tasks
        RemoveTask(CleanBinaries, parameters.Features.CleanBinaries);
        RemoveTask(BuildSolution, parameters.Features.BuildSolution);
        RemoveTask(PublishAppVeyorArtifacts, parameters.Features.PublishAppVeyorArtifacts);
        RemoveTask(PublishNuGetPackages, parameters.Features.PublishNuGetPackages);
        RemoveTask(RestoreNuGetPackages, parameters.Features.RestoreNuGetPackages);
        RemoveTask(RunUnitTests, parameters.Features.RunUnitTests);

        // Wire up high level flow.
        Build?.IsDependentOn(Clean);
        Build?.IsDependentOn(Restore);
        Test?.IsDependentOn(Build);
        Pack?.IsDependentOn(Test);
        Publish?.IsDependentOn(Pack);

        // Connect "batteries included" tasks with their parents.
        CleanArtifacts?.PartOf(Clean, true);
        CleanBinaries?.PartOf(Clean, parameters.Features.CleanBinaries);
        RestoreNuGetPackages?.PartOf(Restore, parameters.Features.RestoreNuGetPackages);
        BuildSolution?.PartOf(Build, parameters.Features.BuildSolution);
        RunUnitTests?.PartOf(Test, parameters.Features.RunUnitTests);
        PublishAppVeyorArtifacts?.PartOf(Publish, parameters.Features.PublishAppVeyorArtifacts);
        PublishNuGetPackages?.PartOf(Publish, parameters.Features.PublishNuGetPackages);

        // Find children and attach them to their grand parents.
        // This so we can keep the task chain if a child task is executed directly.
        foreach(var child in _engineTasks.Where(t => t.Dependees.Count > 0))
        {
            // Find the dependents parents.
            var parentNames = new HashSet<string>(child.Dependees.Select(x => x.Name), StringComparer.OrdinalIgnoreCase);
            var parentTasks = _engineTasks.Where(t => parentNames.Contains(t.Name));

            // Find the dependents grand parents.
            var grandParents = parentTasks.SelectMany(p => p.Dependencies).ToArray();
            var grandParentNames = grandParents.Select(g => g.Name);

            if (grandParents.Length > 0)
            {
                context.Debug("{0} is a child to {1}, and grandchild to {2}",
                    child.Name,  string.Join(" and ", parentNames), string.Join(" and ", grandParentNames));

                foreach(var grandParent in grandParents)
                {
                    child.Dependencies.Add(new CakeTaskDependency(grandParent.Name, grandParent.Required));
                }
            }
        }
    }

    private void RemoveTask(CakeTaskBuilder builder, bool condition)
    {
        if (!condition && builder.Task is CakeTask task)
        {
            _context.Debug("Removing {0}...", task.Name);
            _engineTasks.Remove(task);

            foreach (var engineTask in _engineTasks)
            {
                engineTask.Dependencies.RemoveAll(d => d.Name == task.Name);
                engineTask.Dependees.RemoveAll(d => d.Name == task.Name);
            }
        }
    }
}

public static CakeTaskBuilder PartOf(this CakeTaskBuilder builder, CakeTaskBuilder task)
{
    if(task != null)
    {
        builder.IsDependeeOf(task.Task.Name);
    }
    return builder;
}

public static CakeTaskBuilder PartOf(this CakeTaskBuilder builder, CakeTaskBuilder task, bool condition)
{
    if(task != null && condition)
    {
        builder.IsDependeeOf(task.Task.Name);
    }
    return builder;
}

public static CakeTaskBuilder ClearActions(this CakeTaskBuilder builder)
{
    ((CakeTask)builder.Task).Actions.Clear();
    return builder;
}

public static CakeTaskBuilder ClearCriterias(this CakeTaskBuilder builder)
{
    ((CakeTask)builder.Task).Criterias.Clear();
    return builder;
}
