#tool nuget:?package=GitVersion.CommandLine&version=4.0.0

public sealed class SpectreVersion
{
    public string MajorMinorPatch { get; }
    public string MajorMinorPatchRevision { get; }
    public string SemanticVersion { get; }
    public string MsiVersion { get; }
    
    public SpectreVersion(ICakeContext context)
    {
        string version = null;
        string fullVersion = null;
        string semVersion = null;
        string milestone = null;
        string msiVersion = null;

        if (context.IsRunningOnWindows())
        {
            if (context.BuildSystem().AppVeyor.IsRunningOnAppVeyor)
            {
                // Update AppVeyor version number.
                context.GitVersion(new GitVersionSettings{
                    UpdateAssemblyInfo = true,
                    OutputType = GitVersionOutput.BuildServer,
                    UpdateAssemblyInfoFilePath = "./src/SharedAssemblyInfo.cs",
                });
            }
            
            GitVersion assertedVersions = context.GitVersion(new GitVersionSettings
            {
                OutputType = GitVersionOutput.Json,
            });

            var major = assertedVersions.Major;
            var minor = assertedVersions.Minor;
            var patch = assertedVersions.Patch;
            var revision = assertedVersions.PreReleaseNumber ?? 999;

            version = assertedVersions.MajorMinorPatch;
            semVersion = assertedVersions.LegacySemVerPadded;
            milestone = string.Concat("v", version);

            msiVersion = $"{major}.{minor}.{10000 + patch * 1000 + revision}";
            fullVersion = $"{version}.{revision}";
        }

        if (string.IsNullOrEmpty(version) || string.IsNullOrEmpty(semVersion))
        {
            throw new CakeException("Could not parse version.");
        }

        MajorMinorPatch = version;
        MajorMinorPatchRevision = fullVersion;
        SemanticVersion = semVersion;
        MsiVersion = msiVersion;
    }
}