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
    public class UIManager : Core.STree
    {
        public UIManager() : base()
        {
            GameObject root = new("UIRoot", typeof(RectTransform));
            root.layer = Layers.UI;
            GameObject.DontDestroyOnLoad(root);
            UGUIRoot = root.transform as RectTransform;
            UGUIRoot.sizeDelta = new Vector2(Screen.width, Screen.height);
            UGUIRoot.pivot = Vector2.zero;
            UGUIRoot.position = Vector3.zero;

            NTexture.CustomDestroyMethod += t => SAsset.Release(t);
            NAudioClip.CustomDestroyMethod += t => SAsset.Release(t);

            Client.World.Root.AddChild(this);
        }

        /// <summary>
        /// UGUI模式有效
        /// </summary>
        public RectTransform UGUIRoot { get; }

        public T Open<T>(params object[] data) where T : UIBase, new()
        {
            T ui = GetChild<T>();
            if (ui != null)
                return ui;

            UIConfig cfg = typeof(T).GetCustomAttribute<UIConfig>() ?? UIConfig.Default;

            ui = new();
            this.AddChild(ui);
            ui.LoadConfig(cfg, new STask<T>(), data);
            this.GetChildren().Sort((x, y) => ((UIBase)x).uiConfig.SortOrder - ((UIBase)y).uiConfig.SortOrder);

            for (int i = this.GetChildren().Count - 1; i >= 0; i--)
            {
                UIBase tmp = (UIBase)this.GetChildren()[i];
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

                InputHelper.EnableUIInput(false);
                ui = new();
                this.AddChild(ui);
                //在执行异步的过程中有可能会关闭这个UI
                ui.onDispose.Add(() =>
                {
                    if (ui.uiStates < UIStates.Success)
                        InputHelper.EnableUIInput(true);
                });
                STask loadTask = ui.LoadConfigAsync(cfg, new STask<T>(), data);
                this.GetChildren().Sort((x, y) => ((UIBase)x).uiConfig.SortOrder - ((UIBase)y).uiConfig.SortOrder);
                await loadTask;
                await ui.onTask;
                if (ui.Disposed)
                    return ui;

                for (int i = this.GetChildren().Count - 1; i >= 0; i--)
                {
                    UIBase tmp = (UIBase)this.GetChildren()[i];
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
                InputHelper.EnableUIInput(true);

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
            parent.GetChildren().Sort((x, y) => ((UIBase)x).uiConfig.SortOrder - ((UIBase)y).uiConfig.SortOrder);
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

                InputHelper.EnableUIInput(false);
                ui = new();
                //在执行异步的过程中有可能会关闭这个UI
                ui.onDispose.Add(() =>
                {
                    if (ui.uiStates < UIStates.Success)
                        InputHelper.EnableUIInput(true);
                });
                parent.AddChild(ui);
                await ui.LoadConfigAsync(cfg, new STask<T>(), data);
                parent.GetChildren().Sort((x, y) => ((UIBase)x).uiConfig.SortOrder - ((UIBase)y).uiConfig.SortOrder);
                await ui.onTask;
                if (ui.Disposed)
                    return ui;

                ui.EventEnable = true;
                ui.TimerEnable = true;
                ui.Show();
                InputHelper.EnableUIInput(true);

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
            foreach (var ui in this.GetChildren().FindAll(t => test((UIBase)t)))
            {
                if (ui.Disposed) continue;
                ui.Dispose();
            }
        }
        public void CloseAll()
        {
            foreach (var ui in this.ToChildren())
            {
                if (ui.Disposed) continue;
                ui.Dispose();
            }
        }

        [Event(-101)]
        void outScene(EC_OutScene e) => CloseAll(ui => true);
    }
}
