#load "mod.cake"
#load "../config/mod.cake"
#load "../extensions/mod.cake"

Task(SpectreTasks.Restore);

// Restore NuGet Packages
Task(SpectreTasks.RestoreNuGetPackages)
    .PartOf(SpectreTasks.Restore)
    .OnlyRunIfThereAreSolutions()
    .Does<SpectreData>((context, data) =>
{
    foreach(var solution in data.Files.Solutions)
    {
        context.DotNetCoreRestore(solution.FullPath);
    }
});