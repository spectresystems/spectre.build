public sealed class BuildParameters
{
    public bool RequireTaggedBuildForPublish { get; set; }

    public BuildParameters()
    {
        RequireTaggedBuildForPublish = true;
    }
}