using System;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using System.Reflection;
using Event;

public enum UIModel
{
    UGUI,
    FGUI,
}
public enum UIStates
{
    Loading,
    OnTask,
    Success,
}
namespace Game
{
    class UI : Core.STree<UIBase>
    {
        public static UI Inst { get; } = new();

        UI() : base()
        {
            GameObject root = new("UIRoot", typeof(RectTransform));
            root.layer = Layers.UI;
            GameObject.DontDestroyOnLoad(root);
            UGUIRoot = root.transform as RectTransform;
            UGUIRoot.sizeDelta = new Vector2(Screen.width, Screen.height);
            UGUIRoot.pivot = Vector2.zero;
            UGUIRoot.position = Vector3.zero;
            UGUICamera = GameObject.Find("UGUICamera").GetComponent<Camera>();

            NTexture.CustomDestroyMethod += t => SAsset.Release(t);
            NAudioClip.CustomDestroyMethod += t => SAsset.Release(t);

            Client.World.Root.AddChild(this);
        }

        /// <summary>
        /// UGUI模式有效
        /// </summary>
        public RectTransform UGUIRoot { get; }
        public Camera UGUICamera { get; }

        internal async STask Load()
        {
            //ugui init
            {
                UGUIRoot.gameObject.SetActive(SettingL.UIModel == UIModel.UGUI);
                UGUICamera.gameObject.SetActive(SettingL.UIModel == UIModel.UGUI);
            }

            //fgui init
            {
                FairyGUI.UIConfig.defaultFont = "Impact";
                GRoot.inst.visible = SettingL.UIModel == UIModel.FGUI;
                StageCamera.main.gameObject.SetActive(SettingL.UIModel == UIModel.FGUI);
                UIPkg.ComPkg = UIPackage.AddPackage((await SAsset.LoadAsync<TextAsset>("UI/FUI/ComPkg/ComPkg_fui.bytes")).bytes, "ComPkg", fguiLoader);
                UIPkg.ResPkg = UIPackage.AddPackage((await SAsset.LoadAsync<TextAsset>("UI/FUI/ResPkg/ResPkg_fui.bytes")).bytes, "ResPkg", fguiLoader);
                UIPkg.Items = UIPackage.AddPackage((await SAsset.LoadAsync<TextAsset>("UI/FUI/Items/Items_fui.bytes")).bytes, "Items", fguiLoader);
            }
        }
        static async void fguiLoader(string name, string extension, System.Type type, PackageItem item)
        {
            switch (item.type)
            {
                case PackageItemType.Sound:
                case PackageItemType.Atlas:
                    item.owner.SetItemAsset(item, await SAsset.LoadAsync<UnityEngine.Object>($"UI/FUI/{item.owner.name}/{name}{extension}"), DestroyMethod.Custom);
                    break;
                default:
                    Loger.Error("未定义加载->" + item.type);
                    break;
            }
        }
        public T Open<T>(params object[] data) where T : UIBase, new()
        {
            T ui = GetChild<T>();
            if (ui != null)
                return ui;

            UIConfig cfg = typeof(T).GetCustomAttribute<UIConfig>() ?? UIConfig.Default;

            ui = new();
            this.AddChild(ui);
            ui.LoadConfig(cfg, new STask<T>(), data);
            this.GetChildren().Sort((x, y) => x.uiConfig.SortOrder - y.uiConfig.SortOrder);

            for (int i = this.GetChildren().Count - 1; i >= 0; i--)
            {
                UIBase tmp = this.GetChildren()[i];
                if (tmp == ui)
                    continue;
                if (tmp.uiConfig.HideIfOpenOtherUI && tmp.isShow)
                {
                    tmp.Hide();
                    break;
                }
            }
            ui.Show();
            ui.onCompleted.TrySetResult(ui);

            return ui;
        }
        public async STask<T> OpenAsync<T>(params object[] data) where T : UIBase, new()
        {
            T ui = GetChild<T>();
            if (ui != null)
            {
                await ui.onCompleted;
                return ui;
            }
            using (await STaskLocker.Lock(this))
            {
                UIConfig cfg = typeof(T).GetCustomAttribute<UIConfig>() ?? UIConfig.Default;

                UIHelper.EnableUIInput(false);
                ui = new();
                this.AddChild(ui);
                //在执行异步的过程中有可能会关闭这个UI
                ui.onDispose.Add(() =>
                {
                    if (ui.uiStates < UIStates.Success)
                        UIHelper.EnableUIInput(true);
                });
                STask loadTask = ui.LoadConfigAsync(cfg, new STask<T>(), data);
                this.GetChildren().Sort((x, y) => x.uiConfig.SortOrder - y.uiConfig.SortOrder);
                await loadTask;
                await ui.onTask;
                if (ui.Disposed)
                    return ui;

                for (int i = this.GetChildren().Count - 1; i >= 0; i--)
                {
                    UIBase tmp = this.GetChildren()[i];
                    if (tmp == ui)
                        continue;
                    if (tmp.uiConfig.HideIfOpenOtherUI && tmp.isShow)
                    {
                        tmp.Hide();
                        break;
                    }
                }
                ui.EventEnable = true;
                ui.TimerEnable = true;
                ui.Show();
                UIHelper.EnableUIInput(true);

                ui.onCompleted.TrySetResult(ui);
                return ui;
            }
        }

        public T OpenSubUI<T>(UIBase parent, params object[] data) where T : UIBase, new()
        {
            T ui = parent.GetSubUI<T>();
            if (ui != null)
                return ui;

            UIConfig cfg = typeof(T).GetCustomAttribute<UIConfig>() ?? UIConfig.Default;

            ui = new();
            parent.AddChild(ui);
            ui.LoadConfig(cfg, new STask<T>(), data);
            ui.Show();
            ui.onCompleted.TrySetResult(ui);

            return ui;
        }
        public async STask<T> OpenSubUIAsync<T>(UIBase parent, params object[] data) where T : UIBase, new()
        {
            T ui = parent.GetSubUI<T>();
            if (ui != null)
            {
                await ui.onCompleted;
                return ui;
            }
            using (await STaskLocker.Lock(this))
            {
                UIConfig cfg = typeof(T).GetCustomAttribute<UIConfig>() ?? UIConfig.Default;

                UIHelper.EnableUIInput(false);
                ui = new();
                //在执行异步的过程中有可能会关闭这个UI
                ui.onDispose.Add(() =>
                {
                    if (ui.uiStates < UIStates.Success)
                        UIHelper.EnableUIInput(true);
                });
                parent.AddChild(ui);
                await ui.LoadConfigAsync(cfg, new STask<T>(), data);
                await ui.onTask;
                if (ui.Disposed)
                    return ui;

                ui.EventEnable = true;
                ui.TimerEnable = true;
                ui.Show();
                UIHelper.EnableUIInput(true);

                ui.onCompleted.TrySetResult(ui);
                return ui;
            }
        }

        public T Open3D<T>(params object[] data) where T : UIBase, new()
        {
            UIConfig cfg = typeof(T).GetCustomAttribute<UIConfig>() ?? UIConfig.Default;

            T ui = new();
            this.AddChild(ui);
            ui.LoadConfig(cfg, new STask<T>(), data);
            ui.onCompleted.TrySetResult(ui);

            return ui;
        }
        public async STask<T> Open3DAsync<T>(params object[] data) where T : UIBase, new()
        {
            using (await STaskLocker.Lock(this))
            {
                UIConfig cfg = typeof(T).GetCustomAttribute<UIConfig>() ?? UIConfig.Default;

                T ui = new();
                this.AddChild(ui);
                await ui.LoadConfigAsync(cfg, new STask<T>(), data);
                await ui.onTask;
                if (ui.Disposed)
                    return ui;

                ui.EventEnable = true;
                ui.TimerEnable = true;

                ui.onCompleted.TrySetResult(ui);
                return ui;
            }
        }

        public void CloseAll(Func<UIBase, bool> test)
        {
            int len = this.GetChildren().Count;
            for (; len > 0; len--)
            {
                UIBase ui = this.GetChildren()[len - 1];
                if (!test(ui)) continue;
                ui.Dispose();
            }
        }

        [Event]
        void outScene(EC_OutScene e) => CloseAll(ui => ui.uiConfig.CloseIfChangeScene);
    }
}
