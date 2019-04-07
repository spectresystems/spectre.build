#load "mod.cake"

public static CakeTaskBuilder OnlyRunIfThereAreUnitTests(this CakeTaskBuilder builder)
{
    return builder.WithCriteria<SpectreData>(
        (ctx, data) => data.Files.TestProjects.Count > 0,
        "No unit tests found"
    );
}

public static CakeTaskBuilder OnlyRunIfThereAreSolutions(this CakeTaskBuilder builder)
{
    return builder.WithCriteria<SpectreData>(
        (ctx, data) => data.Files.Solutions.Count > 0,
        "No solutions found"
    );
}

public static CakeTaskBuilder IfThereAreNuGetPackages(this CakeTaskBuilder builder)
{
    return builder.WithCriteria<SpectreData>(
        (context, data) => data.Files.NuGetPackages.Count > 0,
        "No NuGet packages found");
}

public static CakeTaskBuilder OnlyOnBuildServer(this CakeTaskBuilder builder)
{
    return builder.WithCriteria<SpectreData>(
        (ctx, data) => data.CI.IsRunningOnCI,
        "Not running on build server"
    );
}

public static CakeTaskBuilder OnlyOnAppVeyor(this CakeTaskBuilder builder)
{
    return builder.WithCriteria<SpectreData>(
        (ctx, data) => ctx.BuildSystem().AppVeyor.IsRunningOnAppVeyor,
        "Not running on AppVeyor"
    );
}

public static CakeTaskBuilder OnlyOnMasterBranch(this CakeTaskBuilder builder)
{
    return builder.WithCriteria<SpectreData>(
        (ctx, data) => data.CI.IsMasterBranch,
        "Not on master branch"
    );
}

public static CakeTaskBuilder OnlyForIncrementalBuild(this CakeTaskBuilder builder)
{
    return builder.WithCriteria<SpectreData>(
        (ctx, data) => !data.Rebuild,
        "Not an incremental build"
    );
}

public static CakeTaskBuilder OnlyForRebuild(this CakeTaskBuilder builder)
{
    return builder.WithCriteria<SpectreData>(
        (ctx, data) => data.Rebuild,
        "Not a rebuild"
    );
}

public static CakeTaskBuilder RequiresTaggedBuild(this CakeTaskBuilder builder)
{
    return builder.WithCriteria<SpectreData>(
        (ctx, data) => data.CI.IsTaggedBuild,
        "Not a tagged build"
    );
}

public static CakeTaskBuilder Disable(this CakeTaskBuilder builder)
{
    return builder.WithCriteria<SpectreData>(
        (ctx, data) => false,
        "Task have been disabled"
    );
}