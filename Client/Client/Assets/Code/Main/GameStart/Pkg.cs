using Game;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using YooAsset;

public static class Pkg
{
    public static ResourcePackage res { get; private set; }
    public static ResourcePackage raw { get; private set; }

    public static async void Load(EPlayMode mode, Loading loading)
    {
        YooAssets.Initialize();
        var loader = (YooassetLoader)SAsset.Loader;
        res = YooAssets.CreatePackage("Res");
        raw = YooAssets.CreatePackage("Raw");
        loader.SetDefaultPackage(Pkg.res);

        await initPackage(mode, loading, raw);
        await initPackage(mode, loading, res);
        loading.text.text = "success";
        loading.Dispose();
    }
    async static STask initPackage(EPlayMode mode, Loading loading, ResourcePackage package)
    {
        InitializePackageOperation initializationOperation = null;
        // 编辑器下的模拟模式
        if (mode == EPlayMode.EditorSimulateMode)
        {
            var buildResult = EditorSimulateBuildInvoker.Build(package.PackageName, (int)EBundleType.VirtualAssetBundle);
            var packageRoot = buildResult.PackageRootDirectory;
            var createParameters = new EditorSimulateModeOptions();
            createParameters.EditorFileSystemParameters = FileSystemParameters.CreateDefaultEditorFileSystemParameters(packageRoot);
            createParameters.EditorFileSystemParameters.AddParameter(EFileSystemParameter.VirtualWebglMode, true);
            createParameters.EditorFileSystemParameters.AddParameter(EFileSystemParameter.VirtualDownloadMode, true);
            createParameters.EditorFileSystemParameters.AddParameter(EFileSystemParameter.VirtualDownloadSpeed, 1024 * 1000);
            createParameters.EditorFileSystemParameters.AddParameter(EFileSystemParameter.AsyncSimulateMinFrame, 5);
            createParameters.EditorFileSystemParameters.AddParameter(EFileSystemParameter.AsyncSimulateMaxFrame, 10);
            initializationOperation = package.InitializePackageAsync(createParameters);
        }

        // 单机运行模式
        if (mode == EPlayMode.OfflinePlayMode)
        {
            var createParameters = new OfflinePlayModeOptions();
            createParameters.BuiltinFileSystemParameters = FileSystemParameters.CreateDefaultBuiltinFileSystemParameters();
            initializationOperation = package.InitializePackageAsync(createParameters);
        }

        // 联机运行模式
        if (mode == EPlayMode.HostPlayMode)
        {
            IRemoteService remoteService = new RemoteServices()
            {
                url = getUrl(package),
                fallBackUrl = getUrl(package),
            };
            var createParameters = new HostPlayModeOptions();
            createParameters.BuiltinFileSystemParameters = FileSystemParameters.CreateDefaultBuiltinFileSystemParameters();
            createParameters.BuiltinFileSystemParameters.AddParameter(EFileSystemParameter.CopyBuiltinPackageManifest, true);
            createParameters.CacheFileSystemParameters = FileSystemParameters.CreateDefaultSandboxFileSystemParameters(remoteService);
            createParameters.CacheFileSystemParameters.AddParameter(EFileSystemParameter.DownloadMaxConcurrency, 5);
            createParameters.CacheFileSystemParameters.AddParameter(EFileSystemParameter.DownloadMaxRequestPerFrame, 1);
            createParameters.CacheFileSystemParameters.AddParameter(EFileSystemParameter.DownloadWatchdogTimeout, 10);
            initializationOperation = package.InitializePackageAsync(createParameters);
        }

        // WebGL运行模式
        if (mode == EPlayMode.WebPlayMode)
        {
#if UNITY_WEBGL && WEIXINMINIGAME && !UNITY_EDITOR
            var createParameters = new WebPlayModeOptions();
            string defaultHostServer = GetHostServerURL();
            string fallbackHostServer = GetHostServerURL();
            string packageRoot = $"{WeChatWASM.WX.env.USER_DATA_PATH}/__GAME_FILE_CACHE"; // Change this path if subdirectories are required.
            IRemoteService remoteService = new RemoteService(defaultHostServer, fallbackHostServer);
            createParameters.WebServerFileSystemParameters = WechatFileSystemCreater.CreateFileSystemParameters(packageRoot, remoteService);
            initializationOperation = package.InitializePackageAsync(createParameters);
#else
            var createParameters = new WebPlayModeOptions();
            createParameters.WebServerFileSystemParameters = FileSystemParameters.CreateDefaultWebServerFileSystemParameters();
            initializationOperation = package.InitializePackageAsync(createParameters);
#endif
        }

        loading.text.text = $"init {package.PackageName}";
        await initializationOperation;

        var version = package.RequestPackageVersionAsync();
        loading.text.text = $"request {package.PackageName} version";
        await version;
        if (version.Status != EOperationStatus.Succeeded)
        {
            loading.ShowError(version.Error);
            return;
        }

        {
            loading.text.text = $"update {package.PackageName} manifest vs={version.PackageVersion}";
            var options = new LoadPackageManifestOptions(version.PackageVersion, 60);
            var req_manifest = package.LoadPackageManifestAsync(options);
            await req_manifest;
            if (req_manifest.Status != EOperationStatus.Succeeded)
            {
                loading.ShowError(req_manifest.Error);
                return;
            }
        }

        if (mode == EPlayMode.HostPlayMode)
        {
            var options = new ResourceDownloaderOptions(10, 3);
            var downloader = package.CreateResourceDownloader(options);
            downloader.StartDownload();
            loading.text.text = $"download {package.PackageName} 0/{downloader.TotalDownloadCount} 0/{getSizeStr(downloader.TotalDownloadBytes)} vs={version.PackageVersion}";
            downloader.DownloadProgressChanged += t =>
            {
                loading.bar.max = 10000;
                loading.bar.value = t.Progress * 10000;
                loading.text.text = $"download {package.PackageName} {t.CurrentDownloadCount}/{t.TotalDownloadCount} {getSizeStr(t.CurrentDownloadBytes)}/{getSizeStr(t.TotalDownloadBytes)} vs={version.PackageVersion}";
            };
            await downloader;
            if (downloader.Status != EOperationStatus.Succeeded)
            {
                loading.ShowError(downloader.Error);
                return;
            }
        }

        await package.ClearCacheAsync(new ClearCacheOptions(ClearCacheMethods.ClearUnusedBundleFiles));
        await package.ClearCacheAsync(new ClearCacheOptions(ClearCacheMethods.ClearUnusedManifestFiles));
    }
    static string getSizeStr(long bytes)
    {
        return $"{bytes / (float)(1024 * 1024):0.00#}MB";
    }
    static string getUrl(ResourcePackage pkg)
    {
        return $"{GameStart.Inst.resUrl.TrimEnd('/')}/{pkg.PackageName}/{(pkg == Pkg.raw ? "" : $"{GetPlatformName(Application.platform)}/")}";
    }
    static string GetPlatformName(RuntimePlatform platform)
    {
        if (platform == RuntimePlatform.WindowsEditor
            || platform == RuntimePlatform.WindowsPlayer)
            return "StandaloneWindows64";
        if (platform == RuntimePlatform.OSXEditor
            || platform == RuntimePlatform.OSXPlayer
            || platform == RuntimePlatform.IPhonePlayer)
            return "iOS";
        if (platform == RuntimePlatform.Android)
            return "Android";
        if (platform == RuntimePlatform.WebGLPlayer)
            return "WebGL";
        Loger.Error($"unknown platform={platform}");
        return "unknown";
    }

    public static byte[] LoadRaw(string location)
    {
        if (!Pkg.raw.IsLocationValid(location))
            return null;
        var handler = Pkg.raw.LoadAssetSync<RawFileObject>(location);
        var bs = handler.GetAssetObject<RawFileObject>().GetBytes();
        handler.Dispose();
        return bs;
    }
    public static string LoadRawText(string location)
    {
        if (!Pkg.raw.IsLocationValid(location))
            return null;
        var handler = Pkg.raw.LoadAssetSync<RawFileObject>(location);
        var bs = handler.GetAssetObject<RawFileObject>().GetText();
        handler.Dispose();
        return bs;
    }
    public static async STask<byte[]> LoadRawAsync(string location)
    {
        if (!Pkg.raw.IsLocationValid(location))
            return null;
        var handler = Pkg.raw.LoadAssetAsync<RawFileObject>(location);
        await handler;
        var bs = handler.GetAssetObject<RawFileObject>().GetBytes();
        handler.Dispose();
        return bs;
    }
    public static async STask<string> LoadRawTextAsync(string location)
    {
        if (!Pkg.raw.IsLocationValid(location))
            return null;
        var handler = Pkg.raw.LoadAssetAsync<RawFileObject>(location);
        await handler;
        var bs = handler.GetAssetObject<RawFileObject>().GetText();
        handler.Dispose();
        return bs;
    }

    class RemoteServices : IRemoteService
    {
        public string url;
        public string fallBackUrl;

        public IReadOnlyList<string> GetRemoteUrls(string fileName)
        {
            return new List<string> { url + fileName, fallBackUrl + fileName };
        }
    }
}
