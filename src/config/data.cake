#load "mod.cake"
#load "../parameters.cake"

public sealed class SpectreData
{
    public string Configuration { get; }
    public bool Rebuild { get; }
    public bool PublishRequiresTaggedBuild { get; }

    public SpectrePaths Paths { get; }
    public SpectreFiles Files { get; }
    public SpectreVersion Version { get; }
    public SpectreCI CI { get; }
    public SpectreNuGet NuGet{ get; }

    public SpectreData(ICakeContext context, BuildParameters parameters)
    {
        Configuration = context.Argument("configuration", "Release");
        Rebuild = context.HasArgument("rebuild");
        PublishRequiresTaggedBuild = parameters.RequireTaggedBuildForPublish;

        Paths = new SpectrePaths(context);
        Files = new SpectreFiles(context);
        Version = new SpectreVersion(context);
        CI = new SpectreCI(context);
        NuGet = new SpectreNuGet(context);
    }
}