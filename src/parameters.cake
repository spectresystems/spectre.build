#load "mod.cake"

public sealed class SpectreFeatures
{
    public bool CleanBinaries { get; set; } = true;
    public bool BuildSolution { get; set; } = true;
    public bool RestoreNuGetPackages { get; set; } = true;
    public bool RunUnitTests { get; set; } = true;
    public bool PublishAppVeyorArtifacts { get; set; } = true;
    public bool PublishNuGetPackages { get; set; } = true;
}

public sealed class BuildParameters
{
    public SpectreFeatures Features { get; }

    public BuildParameters()
    {
        Features = new SpectreFeatures();
    }
}