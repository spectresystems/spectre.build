#load "mod.cake"

Spectre.Tasks.Build = Task("Build");

// Build solution
Spectre.Tasks.BuildSolution = Task("Build-Solution")
    .OnlyRunIfThereAreSolutions()
    .Does<SpectreData>((context, data) =>
{
    foreach(var solution in data.Files.Solutions)
    {
        DotNetCoreBuild(solution.FullPath, new DotNetCoreBuildSettings {
            Configuration = data.Configuration,
            Verbosity = DotNetCoreVerbosity.Minimal,
            NoRestore = true,
            MSBuildSettings = new DotNetCoreMSBuildSettings()
                .TreatAllWarningsAs(MSBuildTreatAllWarningsAs.Error)
                .WithProperty("Version", data.Version.SemanticVersion)
                .WithProperty("AssemblyVersion", data.Version.MajorMinorPatchRevision)
                .WithProperty("FileVersion", data.Version.MajorMinorPatchRevision)
                .WithProperty("PackageVersion", data.Version.SemanticVersion)
        });
    }
});