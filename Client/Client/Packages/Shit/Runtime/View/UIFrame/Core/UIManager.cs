using System;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using Event;

public enum UIModel
{
    UGUI,
    FGUI,
}
public enum UIStatus
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

#if FairyGUI
            FairyGUI.NTexture.CustomDestroyMethod += t => SAsset.Release(t);
            FairyGUI.NAudioClip.CustomDestroyMethod += t => SAsset.Release(t);
#endif

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

            ui = new() { isCrucialRoot = true };
            this.AddChild(ui);
            ui.LoadConfig(cfg, new STask<T>(), data).AddEvent(() =>
            {
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
            });

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

                UIGlobalConfig.EnableUIInput(false);
                ui = new() { isCrucialRoot = true };
                this.AddChild(ui);
                //在执行异步的过程中有可能会关闭这个UI
                ui.onDispose.Add(() =>
                {
                    if (ui.uiStates < UIStatus.Success)
                        UIGlobalConfig.EnableUIInput(true);
                });
                await ui.LoadConfigAsync(cfg, new STask<T>(), data);
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
                ui.Show();
                UIGlobalConfig.EnableUIInput(true);

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
                if (ui.Disposed)
                    return ui;

                ui.onCompleted.TrySetResult(ui);
                return ui;
            }
        }

        public void CloseUI(Func<UIBase, bool> test = null)
        {
            foreach (var ui in this.ToChildren().FindAll(t => t is UIBase u && (test == null || test(u))))
            {
                if (ui.Disposed) continue;
                ui.Dispose();
            }
        }
    }
}
