#load "mod.cake"

Task(SpectreTasks.Restore)
    .IsDependentOn(SpectreTasks.RestoreNuGetPackages);

// Restore NuGet Packages
Task(SpectreTasks.RestoreNuGetPackages)
    .OnlyRunIfThereAreSolutions()
    .Does<SpectreData>((context, data) =>
{
    foreach(var solution in data.Files.Solutions)
    {
        context.DotNetCoreRestore(solution.FullPath);
    }
});