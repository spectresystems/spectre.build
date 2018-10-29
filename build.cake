#load "./src/mod.cake"

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////

Task("Pack-Scripts")
    .PartOf(SpectreTasks.Pack)
    .Does<SpectreData>((context, data) => 
{
    NuGetPack(new NuGetPackSettings {
        Id = "Spectre.Build",
        Title = "Spectre.Build",
        IconUrl = new Uri("https://raw.githubusercontent.com/spectresystems/graphics/master/png/logo-medium.png"),
        Authors = new List<string>() { "Patrik Svensson" },
        Owners = new List<string>() { "spectresystems" },
        Description = "A set of opinionated, reusable, and convention based build scripts for Cake.",
        Copyright = "Spectre Systems AB",
        NoPackageAnalysis = true,
        BasePath = "./src",
        OutputDirectory = data.Paths.NuGetPackages,
        Files = new List<NuSpecContent>() {
            new NuSpecContent {
                Source = "**/*.cake",
                Target = "Content"
            }
        },
        Version = data.Version.SemanticVersion,
    });
});

///////////////////////////////////////////////////////////////////////////////
// EXECUTION
///////////////////////////////////////////////////////////////////////////////

Spectre.Build();