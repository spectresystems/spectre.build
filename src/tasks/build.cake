#load "mod.cake"

Task(SpectreTasks.Build)
    .IsDependentOn(SpectreTasks.Clean)
    .IsDependentOn(SpectreTasks.Restore);
    
Task(SpectreTasks.BuildSolution)
    .PartOf(SpectreTasks.Build)
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