public static MSBuildSettings UseVSWhere(this MSBuildSettings settings, ICakeContext context, string subPath = null)
{
    subPath = subPath ?? "./MSBuild/Current/Bin/MSBuild.exe";

    var root = context.VSWhereLatest();
    context.Information(root.FullPath);
    settings.ToolPath = root.CombineWithFilePath(subPath);
    return settings;
}

public static MSBuildSettings AddUwpSettings(this MSBuildSettings settings)
{
    return settings
        .SetVerbosity(Verbosity.Minimal)
        .UseToolVersion(MSBuildToolVersion.VS2019)
        .SetMSBuildPlatform(MSBuildPlatform.x64)
        .WithProperty("RestoreAdditionalProjectFallbackFolders", "")
        .WithProperty("AppxBundle","Always")
        .WithProperty("UapAppxPackageBuildMode", "StoreUpload")
        .WithProperty("AppxBundlePlatforms","x86|x64");
}

public static MSBuildSettings SetUwpOutput(this MSBuildSettings settings, DirectoryPath path)
{
    return settings.WithProperty("AppxPackageDir", path.FullPath);
}

public static MSBuildSettings UseNativeToolchain(this MSBuildSettings settings, bool useNativeToolchain = true)
{
    return settings.WithProperty("UseDotNetNativeToolchain", useNativeToolchain ? "true" : "false");
}

public static MSBuildSettings BuildAppXBundle(this MSBuildSettings settings)
{
    return settings.WithProperty("BuildAppxUploadPackageForUap", "true");
}