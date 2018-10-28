#load "mod.cake"
#load "../config/mod.cake"

Task(SpectreTasks.Clean);

// Clean artifacts
Task(SpectreTasks.CleanArtifacts)
    .PartOf(SpectreTasks.Clean)
    .Does<SpectreData>(data => 
{
    CleanDirectory(data.Paths.Artifacts);
    CleanDirectory(data.Paths.Packages);
    CleanDirectory(data.Paths.NuGetPackages);
});

// Clean binaries
Task(SpectreTasks.CleanBinaries)
    .PartOf(SpectreTasks.Clean)
    .WithCriteria<SpectreData>((ctx, data) => data.Rebuild, "Incremental build")
    .Does<SpectreData>(data => 
{
    CleanDirectories($"./src/**/bin/{data.Configuration}");
    CleanDirectories($"./src/**/obj/{data.Configuration}");
});