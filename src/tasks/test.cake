#load "mod.cake"

Spectre.Tasks.Test = Task("Test");

// Run unit tests
Spectre.Tasks.RunUnitTests = Task("Unit-Tests")
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