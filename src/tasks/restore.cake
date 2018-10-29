#load "mod.cake"

Spectre.Tasks.Restore = Task("Restore");

// Restore NuGet Packages
Spectre.Tasks.RestoreNuGetPackages = Task("Restore-NuGet-Packages")
    .OnlyRunIfThereAreSolutions()
    .Does<SpectreData>((context, data) =>
{
    foreach(var solution in data.Files.Solutions)
    {
        context.DotNetCoreRestore(solution.FullPath);
    }
});