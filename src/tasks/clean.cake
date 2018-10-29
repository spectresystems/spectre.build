#load "mod.cake"

Task(SpectreTasks.Clean)
    .IsDependentOn(SpectreTasks.CleanArtifacts)
    .IsDependentOn(SpectreTasks.CleanBinaries);

// Clean artifacts
Task(SpectreTasks.CleanArtifacts)
    .Does<SpectreData>(data => 
{
    CleanDirectory(data.Paths.Artifacts);
    CleanDirectory(data.Paths.Packages);
    CleanDirectory(data.Paths.NuGetPackages);
});

// Clean binaries
Task(SpectreTasks.CleanBinaries)
    .WithCriteria<SpectreData>((ctx, data) => data.Rebuild, "Incremental build")
    .Does<SpectreData>(data => 
{
    CleanDirectories($"./src/**/bin/{data.Configuration}");
    CleanDirectories($"./src/**/obj/{data.Configuration}");
});