#load "mod.cake"

Task(SpectreTasks.Publish)
    .IsDependentOn(SpectreTasks.Test)
    .IsDependentOn(SpectreTasks.Pack)
    .IsDependentOn(SpectreTasks.PublishAppVeyorArtifacts)
    .IsDependentOn(SpectreTasks.PublishNuGetPackages);

// Upload AppVeyor artifacts
Task(SpectreTasks.PublishAppVeyorArtifacts)
    .IsDependentOn(SpectreTasks.Test)
    .IsDependentOn(SpectreTasks.Pack)
    .OnlyOnAppVeyor()
    .IfThereAreNuGetPackages()
    .Does<SpectreData>((context, data) => 
{
    foreach(var file in data.Files.NuGetPackages)
    {
        context.BuildSystem().AppVeyor.UploadArtifact(file);
    }
});

// Publish packages to NuGet
Task(SpectreTasks.PublishNuGetPackages)
    .IsDependentOn(SpectreTasks.Test)
    .IsDependentOn(SpectreTasks.Pack)
    .OnlyOnBuildServer()
    .OnlyOnMasterBranch()
    .RequiresTaggedBuild()
    .IfThereAreNuGetPackages()
    .Does<SpectreData>((context, data) => 
{
    // Make sure that there is an API key.
    if(string.IsNullOrWhiteSpace(data.NuGet.ApiKey)) {
        throw new CakeException("No NuGet API key specified.");
    }
    // Make sure that there is a feed URL.
    if(string.IsNullOrWhiteSpace(data.NuGet.Feed)) {
        throw new CakeException("No NuGet feed specified.");
    }

    foreach(var file in data.Files.NuGetPackages)
    {
        context.NuGetPush(file, new NuGetPushSettings {
            ApiKey = data.NuGet.ApiKey,
            Source = data.NuGet.Feed
        });
    }    
});