using FairyGUI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assemblies;
using UnityEngine.Networking;
using YooAsset;

public class Loading
{
    UIPackage package;
    GComponent ui;
    public GProgressBar bar;
    public GTextField text;
    public async void Enter()
    {
        if (!Application.isEditor && GameStart.Inst.Runtime == CodeRuntime.Assembly)
        {
#if HybridCLR
            foreach (var item in Resources.LoadAll<TextAsset>("AOT"))
                HybridCLR.RuntimeApi.LoadMetadataForAOTAssembly(item.bytes, HybridCLR.HomologousImageMode.SuperSet);
#else

#endif
        }
        package = UIPackage.AddPackage("Loader/Loader");
        ui = package.CreateObject("Loading").asCom;
        GRoot.inst.AddChild(ui);
        ui.MakeFullScreen();
        ui.AddRelation(GRoot.inst, RelationType.Size);

        bar = ui.GetChild("_loadingBar").asProgress;
        bar.value = 0;
        text = ui.GetChild("_Text").asTextField;
        this.ui.GetChild("_yes").onClick.Add(Application.Quit);

        if (!Application.isEditor & (GameStart.Inst.playMode == EPlayMode.HostPlayMode || GameStart.Inst.playMode == EPlayMode.WebPlayMode))
        {
            using UnityWebRequest www = UnityWebRequest.Get($"{GameStart.Inst.resUrl.TrimEnd('/')}/main.json");
            await www.SendWebRequest().AsTask();
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.LogError(www.error);
                this.ShowError(www.error);
                return;
            }
            MainJson main = null;
            try { main = JsonConvert.DeserializeObject<MainJson>(www.downloadHandler.text); }
            catch (Exception) { }
            if (main == null)
            {
                this.ShowError($"download this [{www.downloadHandler.text}]");
                return;
            }
            if (!main.open)
            {
                this.ShowError(main.text);
                return;
            }
            if (new Version(Application.version) < new Version(main.app))
            {
                this.ShowError($"app {Application.version} < {main.app}");
                return;
            }
        }

        Pkg.Load(GameStart.Inst.playMode, this);
    }
    public void Dispose()
    {
        ui.Dispose();
        UIPackage.RemovePackage("Loader/Loader");
    }
    public void ShowError(string error)
    {
        this.ui.GetController("c1").selectedIndex = 1;
        this.ui.GetChild("_error").text = error;
    }
    class MainJson
    {
        public bool open;
        public string text;
        public string app;
    }
}
