﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YooAsset;
using Game;

public static class YooPkg
{
    public static ResourcePackage res;
    public static ResourcePackage raw;

    public static async void Load(EPlayMode mode, Loading loading)
    {
        YooAssets.Initialize();
        var loader = (YooassetLoader)SAsset.Loader;
        res = YooAssets.TryGetPackage("Res") ?? YooAssets.CreatePackage("Res");
        raw = YooAssets.TryGetPackage("Raw") ?? YooAssets.CreatePackage("Raw");
        loader.SetDefaultPackage(YooPkg.res);

        await initPackage(mode, loading, raw);
        await initPackage(mode, loading, res);
        loading.text.text = "success";
        loading.Dispose();
    }
    async static STask initPackage(EPlayMode mode, Loading loading, ResourcePackage pkg)
    {
        InitializationOperation initializationOperation = null;
        // 编辑器下的模拟模式
        if (mode == EPlayMode.EditorSimulateMode)
        {
            var simulateBuildResult = EditorSimulateModeHelper.SimulateBuild(pkg.PackageName);
            var createParameters = new EditorSimulateModeParameters();
            createParameters.EditorFileSystemParameters = FileSystemParameters.CreateDefaultEditorFileSystemParameters(simulateBuildResult.PackageRootDirectory);
            initializationOperation = pkg.InitializeAsync(createParameters);
        }

        // 单机运行模式
        if (mode == EPlayMode.OfflinePlayMode)
        {
            var createParameters = new OfflinePlayModeParameters();
            createParameters.BuildinFileSystemParameters = FileSystemParameters.CreateDefaultBuildinFileSystemParameters();
            initializationOperation = pkg.InitializeAsync(createParameters);
        }

        // 联机运行模式
        if (mode == EPlayMode.HostPlayMode)
        {
            IRemoteServices remoteServices = new RemoteServices()
            {
                url = $"{GameStart.Inst.resUrl}{pkg.PackageName}/",
                fallBackUrl = $"{GameStart.Inst.fallBackResUrl}{pkg.PackageName}/"
            };
            var createParameters = new HostPlayModeParameters();
            createParameters.BuildinFileSystemParameters = FileSystemParameters.CreateDefaultBuildinFileSystemParameters();
            createParameters.CacheFileSystemParameters = FileSystemParameters.CreateDefaultCacheFileSystemParameters(remoteServices);
            initializationOperation = pkg.InitializeAsync(createParameters);
        }

        // WebGL运行模式
        if (mode == EPlayMode.WebPlayMode)
        {
            var createParameters = new WebPlayModeParameters();
#if UNITY_WEBGL && WEIXINMINIGAME && !UNITY_EDITOR
			string defaultHostServer = GetHostServerURL();
            string fallbackHostServer = GetHostServerURL();
            IRemoteServices remoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
            createParameters.WebServerFileSystemParameters = WechatFileSystemCreater.CreateWechatFileSystemParameters(remoteServices);
#else
            createParameters.WebServerFileSystemParameters = FileSystemParameters.CreateDefaultWebServerFileSystemParameters();
#endif
            initializationOperation = pkg.InitializeAsync(createParameters);
        }

        loading.text.text = $"init {pkg.PackageName}";
        await initializationOperation.AsTask();

        var version = pkg.RequestPackageVersionAsync();
        loading.text.text = $"request {pkg.PackageName} version";
        await version.AsTask();
        if (version.Status != EOperationStatus.Succeed)
        {
            loading.ShowError(version.Error);
            return;
        }

        loading.text.text = $"update {pkg.PackageName} manifest vs={version.PackageVersion}";
        var req_manifest = pkg.UpdatePackageManifestAsync(version.PackageVersion);
        await req_manifest.AsTask();
        if (req_manifest.Status != EOperationStatus.Succeed)
        {
            loading.ShowError(req_manifest.Error);
            return;
        }

        if (mode == EPlayMode.HostPlayMode)
        {
            var downloader = pkg.CreateResourceDownloader(10, 3);
            downloader.BeginDownload();
            loading.text.text = $"download {pkg.PackageName} 0/{downloader.TotalDownloadCount} 0/{getSizeStr(downloader.TotalDownloadBytes)} vs={version.PackageVersion}";
            downloader.DownloadUpdateCallback += t =>
            {
                loading.bar.max = 10000;
                loading.bar.value = t.Progress * 10000;
                loading.text.text = $"download {pkg.PackageName} {t.CurrentDownloadCount}/{t.TotalDownloadCount} {getSizeStr(t.CurrentDownloadBytes)}/{getSizeStr(t.TotalDownloadBytes)} vs={version.PackageVersion}";
            };
            await downloader.AsTask();
            if (downloader.Status != EOperationStatus.Succeed)
            {
                loading.ShowError(downloader.Error);
                return;
            }
        }
    }
    static string getSizeStr(long bytes)
    {
        return $"{bytes / (float)(1024 * 1024):0.00#}MB";
    }

    public static byte[] LoadRaw(string location)
    {
        if (!YooPkg.raw.CheckLocationValid(location))
            return null;
        var handler = YooPkg.raw.LoadRawFileSync(location);
        var bs = handler.GetRawFileData();
        handler.Dispose();
        return bs;
    }

    class RemoteServices : IRemoteServices
    {
        public string url;
        public string fallBackUrl;
        public string GetRemoteFallbackURL(string fileName) => fallBackUrl + fileName;
        public string GetRemoteMainURL(string fileName) => url + fileName;
    }
}
