using Core;
using Event;
using Game;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;
using YooAsset;

static class Handler
{
    [Event(-100, Queue = true)]
    static async STask Init(EC_GameStart e)
    {
        YooAssets.Initialize();
        var loader = (YooassetLoader)SAsset.Loader;
        loader.LoadPackage("Res");

        var playMode = EPlayMode.EditorSimulateMode;
        // 编辑器下的模拟模式
        InitializationOperation initializationOperation = null;
        if (playMode == EPlayMode.EditorSimulateMode)
        {
            var simulateBuildResult = EditorSimulateModeHelper.SimulateBuild(EDefaultBuildPipeline.ScriptableBuildPipeline, "Res");
            var createParameters = new EditorSimulateModeParameters();
            createParameters.EditorFileSystemParameters = FileSystemParameters.CreateDefaultEditorFileSystemParameters(simulateBuildResult);
            initializationOperation = loader.Package.InitializeAsync(createParameters);
        }

        // 单机运行模式
        if (playMode == EPlayMode.OfflinePlayMode)
        {
            /*var createParameters = new OfflinePlayModeParameters();
            createParameters.BuildinFileSystemParameters = FileSystemParameters.CreateDefaultBuildinFileSystemParameters();
            initializationOperation = package.InitializeAsync(createParameters);*/
        }

        // 联机运行模式
        if (playMode == EPlayMode.HostPlayMode)
        {
            /*string defaultHostServer = GetHostServerURL();
            string fallbackHostServer = GetHostServerURL();
            IRemoteServices remoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
            var createParameters = new HostPlayModeParameters();
            createParameters.BuildinFileSystemParameters = FileSystemParameters.CreateDefaultBuildinFileSystemParameters();
            createParameters.CacheFileSystemParameters = FileSystemParameters.CreateDefaultCacheFileSystemParameters(remoteServices);
            initializationOperation = package.InitializeAsync(createParameters);*/
        }

        // WebGL运行模式
        if (playMode == EPlayMode.WebPlayMode)
        {
            /*var createParameters = new WebPlayModeParameters();
            createParameters.WebFileSystemParameters = FileSystemParameters.CreateDefaultWebFileSystemParameters();
            initializationOperation = package.InitializeAsync(createParameters);*/
        }

        await initializationOperation.AsTask();
        var version = loader.Package.RequestPackageVersionAsync();
        await version.AsTask();
        await loader.Package.UpdatePackageManifestAsync(version.PackageVersion).AsTask();
        var downloader = loader.Package.CreateResourceDownloader(10, 3);
        YooAssets.SetDefaultPackage(loader.Package);

        DG.Tweening.DOTween.Init();
        SettingL.Languege = SystemLanguage.Chinese;
        Application.targetFrameRate = -1;

        DBuffer buffM_ST = new(new MemoryStream((SAsset.Load<TextAsset>($"config_{nameof(TabM_ST)}")).bytes));
        if (buffM_ST.ReadHeaderInfo())
            TabM_ST.Init(buffM_ST);

        DBuffer buffM = new(new MemoryStream((await SAsset.LoadAsync<TextAsset>($"config_{nameof(TabM)}")).bytes));
        if (buffM.ReadHeaderInfo())
            TabM.Init(buffM, ConstDefCore.Debug);

        DBuffer buffL = new(new MemoryStream((await SAsset.LoadAsync<TextAsset>($"config_{nameof(TabL)}")).bytes));
        if (buffL.ReadHeaderInfo())
            TabL.Init(buffL, ConstDefCore.Debug);
    }

    [Event]
    static void EC_GameStart(EC_GameStart e)
    {
        {
            bool showExit = false;
            var input = new ESCInput();
            input.esconEsc.started += e =>
            {
                if (showExit)
                    return;
                if (e.ReadValueAsButton())
                {
                    showExit = true;
                    Box.Op_YesOrNo("退出游戏", "是否退出游戏?", "确定", "取消", () =>
                    {
                        UnityEngine.Application.Quit();
                        showExit = false;
                    },
                    () => showExit = false);
                }
            };
        }
    }
    [Event]
    static void EC_OutScene(EC_OutScene e)
    {
        BaseCamera.Current?.Dispose();
    }
    [Event]
    static void EC_InScene(EC_InScene e)
    {
        if (e.sceneId == 1)
            Client.Data.Clear();
        if (e.sceneId > 10000)
        {
            BaseCamera.SetCamera(new FreedomCamera(Camera.main.GetComponent<CinemachineBrain>()));
            GameObject cm = new("CMTarget");
            cm.transform.position = new Vector3(0, 0, 0);
            BaseCamera.Current.Init(cm);
            BaseCamera.Current.EnableCamera();
        }
    }
    [Event]
    static void EC_QuitGame(EC_QuitGame e)
    {
        try { Server.Close(); } catch (System.Exception ex) { throw ex; }
        try { Client.Close(); } catch (System.Exception ex) { throw ex; }
        TabM_ST.Tab.Data.Dispose();
        Game.ShareData.Dispose();
    }
    [Event]
    static void EC_AcceptedMessage(EC_AcceptedMessage e)
    {
        if (!string.IsNullOrEmpty(e.message.error))
            Box.Tips(e.message.error);
    }
}
