public sealed class SpectreNuGet
{
    public string ApiKey { get; }
    public string Feed { get; }

    public SpectreNuGet(ICakeContext context)
    {
        ApiKey = GetSetting(context, "nuget-api-key");
        Feed = GetSetting(context, "nuget-feed", "https://nuget.org/api/v2/package");
    }

    private string GetSetting(ICakeContext context, string name, string defaultValue = null)
    {
        return context.Argument(name, 
            context.EnvironmentVariable(
                "SPECTRE_" + name.Replace("-", "_").ToUpperInvariant()
            ) ?? defaultValue);
    }
}