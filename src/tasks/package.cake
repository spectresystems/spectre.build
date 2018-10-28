#load "../config/mod.cake"
#load "mod.cake"

Task(SpectreTasks.Pack)
    .IsDependentOn(SpectreTasks.Build)
    .Does<SpectreData>((context, data) => 
{
   foreach(var package in GetFiles($"{data.Paths.NuGetPackages}/*.nupkg"))
   {
       data.Files.NuGetPackages.Add(package);
   }
});