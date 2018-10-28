public sealed class SpectreFiles
{
    public FilePathCollection TestProjects { get; }
    public FilePathCollection Solutions { get; }

    public FilePathCollection NuGetPackages { get; }

    public SpectreFiles(ICakeContext context)
    {
        TestProjects = context.GetFiles("./src/**/*.Tests.csproj");
        Solutions = context.GetFiles("./src/**/*.sln");
        NuGetPackages = new FilePathCollection(PathComparer.Default);
    }
}