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
    public string BranchName { get; set; }

    public SpectreCI(ICakeContext context)
    {
        var buildSystem = context.BuildSystem();

        BranchName = buildSystem.AppVeyor.Environment.Repository.Branch;
        if (string.IsNullOrWhiteSpace(BranchName)) 
        {
            // Try get the branch name from Git.
            var info = context.GitVersion();
            if (info != null) {
                BranchName = info.BranchName;
            }
        }

        var commitMessage = buildSystem.AppVeyor.Environment.Repository.Commit.Message?.Trim();
        var isMaintenanceBuild = (commitMessage?.StartsWith("(build)", StringComparison.OrdinalIgnoreCase) ?? false) ||
                                 (commitMessage?.StartsWith("(docs)", StringComparison.OrdinalIgnoreCase) ?? false);

        IsLocal = buildSystem.IsLocalBuild;
        IsRunningOnCI = buildSystem.AppVeyor.IsRunningOnAppVeyor;
        IsPullRequest = buildSystem.AppVeyor.Environment.PullRequest.IsPullRequest;
        IsDevelopBranch = "develop".Equals(BranchName, StringComparison.OrdinalIgnoreCase);
        IsMasterBranch = "master".Equals(BranchName, StringComparison.OrdinalIgnoreCase);
        IsReleaseBranch = BranchName.StartsWith("release", StringComparison.OrdinalIgnoreCase);
        IsTaggedBuild = IsBuildTagged(buildSystem);
        IsMaintenanceBuild = isMaintenanceBuild;
    }

    public void Dump(ICakeContext context)
    {
        if (string.IsNullOrWhiteSpace(BranchName))
        {
            context.Warning("Branch: {0}", "N/A");
        }
        else
        {
            context.Verbose("Branch: {0}", BranchName);
        }

        context.Verbose("Local build? {0}", IsLocal ? "Yes" : "No");
        context.Verbose("Pull request? {0}", IsPullRequest ? "Yes" : "No");
        context.Verbose("Master branch? {0}", IsMasterBranch ? "Yes" : "No");
        context.Verbose("Tagged build? {0}", IsTaggedBuild ? "Yes" : "No");
    }

    private static bool IsBuildTagged(BuildSystem buildSystem)
    {
        return buildSystem.AppVeyor.Environment.Repository.Tag.IsTag
            && !string.IsNullOrWhiteSpace(buildSystem.AppVeyor.Environment.Repository.Tag.Name);
    }
}