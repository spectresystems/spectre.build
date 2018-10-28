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
}