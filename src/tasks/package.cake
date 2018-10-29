#load "mod.cake"

Spectre.Tasks.Pack = Task("Pack")
    .Does<SpectreData>((context, data) => 
{
    context.Verbose("Collecting packages...");
    foreach(var package in GetFiles($"{data.Paths.NuGetPackages}/*.nupkg"))
    {
        data.Files.NuGetPackages.Add(package);
    }
});