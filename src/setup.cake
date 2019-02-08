#load "mod.cake"

Setup<SpectreData>(context => 
{
    var data = new SpectreData(context, Spectre.Parameters);
    data.Dump(context, Spectre.Parameters);

    if(context.HasArgument("dryrun"))
    {
        // Include an extra line to make it prettier.
        context.Information("");
    }

    return data;
});