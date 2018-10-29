#load "config/mod.cake"
#load "tasks/mod.cake"
#load "data.cake"
#load "parameters.cake"
#load "setup.cake"

private SpectreBuild _engine;
public SpectreBuild Spectre
{
    get
    {
        if (_engine == null)
        {
            _engine = new SpectreBuild(Context, Tasks, target => RunTarget(target));
        }
        return _engine;
    }
}

public class SpectreBuild
{
    private readonly ICakeContext _context;
    private readonly Action<string> _action;
    
    public BuildParameters Parameters { get; }
    public SpectreTasks Tasks { get; }

    public SpectreBuild(ICakeContext context, IReadOnlyList<ICakeTaskInfo> engineTasks, Action<string> action)
    {
        _context = context;
        _action = action;

        Parameters = new BuildParameters();
        Tasks = new SpectreTasks(_context, engineTasks);
    }

    public void Build()
    {
        // Wire up tasks.
        Tasks.WireUp(_context, Parameters);

        // Execute the build.
        _action(_context.Argument("target", "Default"));
    }
}
