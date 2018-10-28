public sealed class SpectreCI
{
    public bool IsLocal { get; set; }
    public bool IsRunningOnCI { get; set; }
    public bool IsPullRequest { get; set; }
    public bool IsDevelopBranch { get; set; }
    public bool IsMasterBranch { get; set; }
    public bool IsReleaseBranch { get; set; }
    public bool IsTaggedBuild { get; set; }
    public bool IsMaintenanceBuild { get; set; }

    public SpectreCI(ICakeContext context)
    {
        var buildSystem = context.BuildSystem();
        var branchName = buildSystem.AppVeyor.Environment.Repository.Branch;

        var commitMessage = buildSystem.AppVeyor.Environment.Repository.Commit.Message?.Trim();
        var isMaintenanceBuild = (commitMessage?.StartsWith("(build)", StringComparison.OrdinalIgnoreCase) ?? false) ||
                                 (commitMessage?.StartsWith("(docs)", StringComparison.OrdinalIgnoreCase) ?? false);

        IsLocal = buildSystem.IsLocalBuild;
        IsRunningOnCI = buildSystem.AppVeyor.IsRunningOnAppVeyor;
        IsPullRequest = buildSystem.AppVeyor.Environment.PullRequest.IsPullRequest;
        IsDevelopBranch = "develop".Equals(branchName, StringComparison.OrdinalIgnoreCase);
        IsMasterBranch = "master".Equals(branchName, StringComparison.OrdinalIgnoreCase);
        IsReleaseBranch = branchName.StartsWith("release", StringComparison.OrdinalIgnoreCase);
        IsTaggedBuild = IsBuildTagged(buildSystem);
        IsMaintenanceBuild = isMaintenanceBuild;
    }

    private static bool IsBuildTagged(BuildSystem buildSystem)
    {
        return buildSystem.AppVeyor.Environment.Repository.Tag.IsTag
            && !string.IsNullOrWhiteSpace(buildSystem.AppVeyor.Environment.Repository.Tag.Name);
    }
}