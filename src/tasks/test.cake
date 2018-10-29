#load "mod.cake"

Task(SpectreTasks.Test)
    .IsDependentOn(SpectreTasks.Build);

// Run unit tests
Task(SpectreTasks.RunUnitTests)
    .PartOf(SpectreTasks.Test)
    .OnlyRunIfThereAreUnitTests()
    .Does<SpectreData>((context, data) =>
{
    foreach(var project in data.Files.TestProjects) 
    {
        context.DotNetCoreTest(project.FullPath, new DotNetCoreTestSettings {
            NoBuild = true,
            NoRestore = true,
            Configuration = data.Configuration
        });
    }
});