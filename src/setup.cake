#load "mod.cake"

Setup<SpectreData>(context => 
{
    var data = new SpectreData(context, Spectre.Parameters);

    context.Information("Version: {0}", data.Version.SemanticVersion);
    context.Information("Configuration: {0}", data.Configuration);
    context.Information("Publish feed: {0}", data.NuGet.Feed);

    if(context.HasArgument("dryrun"))
    {
        // Include an extra line to make it prettier.
        context.Information("");
    }

    return data;
});