#load "mod.cake"

Spectre.Tasks.Clean = Task("Clean");

// Clean artifacts
Spectre.Tasks.CleanArtifacts = Task("Clean-Artifacts")
    .Does<SpectreData>(data => 
{
    CleanDirectory(data.Paths.Artifacts);
    CleanDirectory(data.Paths.Packages);
    CleanDirectory(data.Paths.NuGetPackages);
});

// Clean binaries
Spectre.Tasks.CleanBinaries = Task("Clean-Binaries")
    .WithCriteria<SpectreData>((ctx, data) => data.Rebuild, "Incremental build")
    .Does<SpectreData>(data => 
{
    CleanDirectories($"./src/**/bin/{data.Configuration}");
    CleanDirectories($"./src/**/obj/{data.Configuration}");
});