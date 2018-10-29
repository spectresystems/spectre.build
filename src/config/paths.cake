public sealed class SpectrePaths
{
    public DirectoryPath Artifacts { get; }
    public DirectoryPath Packages { get; }
    public DirectoryPath NuGetPackages { get; }

    public SpectrePaths(ICakeContext context)
    {
        Artifacts = context.MakeAbsolute(new DirectoryPath("./.artifacts"));
        Packages = Artifacts.Combine("packages");
        NuGetPackages = Packages.Combine("nuget");
    }

    public void Dump(ICakeContext context)
    {
        context.Verbose("Artifacts: {0}", Artifacts.FullPath);
        context.Verbose("Packages: {0}", Packages.FullPath);
        context.Verbose("NuGet packages: {0}", NuGetPackages.FullPath);
    }
}