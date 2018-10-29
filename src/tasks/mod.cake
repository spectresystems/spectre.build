#load "../data.cake"
#load "../criterias.cake"

#load "build.cake"
#load "clean.cake"
#load "package.cake"
#load "publish.cake"
#load "restore.cake"
#load "test.cake"

Task("Default")
    .IsDependentOn(SpectreTasks.Pack);

Task("BuildServer")
    .IsDependentOn(SpectreTasks.Publish);

public class SpectreTasks
{
    public const string Clean = "Clean";
    public const string CleanArtifacts = "Clean-Artifacts";
    public const string CleanBinaries = "Clean-Binaries";
    public const string Restore = "Restore";
    public const string RestoreNuGetPackages = "Restore-NuGet-Packages";
    public const string Build = "Build";
    public const string BuildSolution = "Build-Solution";
    public const string Test = "Test";
    public const string RunUnitTests = "Run-Unit-Test";
    public const string Pack = "Pack";
    public const string Publish = "Publish";
    public const string PublishAppVeyorArtifacts = "Publish-AppVeyor-Artifacts";
    public const string PublishNuGetPackages = "Publish-NuGet-Packages";
}

public static CakeTaskBuilder PartOf(this CakeTaskBuilder builder, string task)
{
    return builder.IsDependeeOf(task);
}
