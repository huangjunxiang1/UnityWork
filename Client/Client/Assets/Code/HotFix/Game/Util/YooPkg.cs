using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YooAsset;
using Game;

static class YooPkg
{
    public static ResourcePackage res;
    public static ResourcePackage raw;

    public static async STask LoadAsync(EPlayMode mode)
    {
        List<ResourcePackage> pkgs = new() { res, raw };
        for (int i = 0; i < pkgs.Count; i++)
        {
            var pkg = pkgs[i];

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
                    url = APPConfig.Inst.resUrl,
                    fallBackUrl = APPConfig.Inst.resUrl
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
            await initializationOperation.AsTask();

            var version = pkg.RequestPackageVersionAsync();
            await version.AsTask();
            await pkg.UpdatePackageManifestAsync(version.PackageVersion).AsTask();

            if (mode == EPlayMode.HostPlayMode)
            {
                var downloader = pkg.CreateResourceDownloader(10, 3);
                downloader.BeginDownload();
                await downloader.AsTask();
            }
        }
    }

    public static byte[] LoadRaw(string location)
    {
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
