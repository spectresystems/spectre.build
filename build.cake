#load "./src/mod.cake"

///////////////////////////////////////////////////////////////////////////////
// SETUP
///////////////////////////////////////////////////////////////////////////////

Spectre.Parameters.Features.CleanBinaries = false;
Spectre.Parameters.Features.RestoreNuGetPackages = false;
Spectre.Parameters.Features.BuildSolution = false;
Spectre.Parameters.Features.RunUnitTests = false;

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////

Task("Pack-Scripts")
    .PartOf(Spectre.Tasks.Pack)
    .Does<SpectreData>((context, data) => 
{
    NuGetPack(new NuGetPackSettings {
        Id = "Spectre.Build",
        Title = "Spectre.Build",
        Version = data.Version.SemanticVersion,
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
    });
});

///////////////////////////////////////////////////////////////////////////////
// EXECUTION
///////////////////////////////////////////////////////////////////////////////

Spectre.Build();