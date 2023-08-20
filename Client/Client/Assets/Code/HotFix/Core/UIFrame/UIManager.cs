using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using Game;
using FairyGUI;
using Main;

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
    class UIManager
    {
        public UIManager()
        {
            GameObject root = new("UIRoot", typeof(RectTransform));
            root.layer = Layers.UI;
            GameObject.DontDestroyOnLoad(root);
            UGUIRoot = root.transform as RectTransform;
            UGUIRoot.sizeDelta = new Vector2(Screen.width, Screen.height);
            UGUIRoot.pivot = Vector2.zero;
            UGUIRoot.position = Vector3.zero;
            UGUICamera = GameObject.Find("UGUICamera").GetComponent<Camera>();

            NTexture.CustomDestroyMethod += t => AssetLoad.Release(t);
            NAudioClip.CustomDestroyMethod += t => AssetLoad.Release(t);
        }

        readonly List<UIBase> _uiLst = new();
        readonly List<UIBase> _3duiLst = new();

        /// <summary>
        /// UGUI模式有效
        /// </summary>
        public RectTransform UGUIRoot { get; }
        public Camera UGUICamera { get; }

        public async TaskAwaiter Init()
        {
            //ugui init
            {
                UGUIRoot.gameObject.SetActive(GameL.Setting.UIModel == UIModel.UGUI);
                UGUICamera.gameObject.SetActive(GameL.Setting.UIModel == UIModel.UGUI);
            }

            //fgui init
            {
                GRoot.inst.visible = GameL.Setting.UIModel == UIModel.FGUI;
                StageCamera.main.gameObject.SetActive(GameL.Setting.UIModel == UIModel.FGUI);
                UIPkg.ComPkg = UIPackage.AddPackage((await AssetLoad.LoadAsync<TextAsset>("UI/FUI/ComPkg/ComPkg_fui.bytes")).bytes, "ComPkg", fguiLoader);
                UIPkg.ResPkg = UIPackage.AddPackage((await AssetLoad.LoadAsync<TextAsset>("UI/FUI/ResPkg/ResPkg_fui.bytes")).bytes, "ResPkg", fguiLoader);
                UIPkg.Items = UIPackage.AddPackage((await AssetLoad.LoadAsync<TextAsset>("UI/FUI/Items/Items_fui.bytes")).bytes, "Items", fguiLoader);
            }
        }
        async void fguiLoader(string name, string extension, System.Type type, PackageItem item)
        {
            switch (item.type)
            {
                case PackageItemType.Sound:
                case PackageItemType.Atlas:
                    item.owner.SetItemAsset(item, await AssetLoad.LoadAsync<UnityEngine.Object>($"UI/FUI/{item.owner.name}/{name}{extension}"), DestroyMethod.Custom);
                    break;
                default:
                    Loger.Error("未定义加载->" + item.type);
                    break;
            }
        }
        public T Open<T>(params object[] data) where T : UIBase, new()
        {
            T ui = Get<T>();
            if (ui != null)
                return ui;

            if (Types.GetAttribute(typeof(T), typeof(Main.UIConfig)) is not Main.UIConfig cfg)
                cfg = Main.UIConfig.Default;

            ui = new();
            _uiLst.Add(ui);
            ui.LoadConfig(cfg, new TaskAwaiter<T>(), data);
            _uiLst.Sort((x, y) => x.uiConfig.SortOrder - y.uiConfig.SortOrder);

            for (int i = _uiLst.Count - 1; i >= 0; i--)
            {
                UIBase tmp = _uiLst[i];
                if (tmp == ui)
                    continue;
                if (tmp.uiConfig.HideOnOpenOtherUI && tmp.isShow && tmp.uiConfig.UIType < UIType.GlobalUI)
                {
                    tmp.Hide();
                    break;
                }
            }
            ui.Show();
            ((TaskAwaiter<T>)ui.onCompleted).TrySetResult(ui);

            return ui;
        }
        public TaskAwaiter<T> OpenAsync<T>(params object[] data) where T : UIBase, new()
        {
            T ui = Get<T>();
            if (ui == null)
            {
                open();
                async void open()
                {
                    if (Types.GetAttribute(typeof(T), typeof(Main.UIConfig)) is not Main.UIConfig cfg)
                        cfg = Main.UIConfig.Default;

                    UIHelper.EnableUIInput(false);
                    ui = new();
                    //在执行异步的过程中有可能会关闭这个UI
                    ui.onDispose.Add(() =>
                    {
                        if (ui.uiStates < UIStates.Success)
                            UIHelper.EnableUIInput(true);
                    });
                    _uiLst.Add(ui);
                    TaskAwaiter loadTask = ui.LoadConfigAsync(cfg, new TaskAwaiter<T>(), data);
                    _uiLst.Sort((x, y) => x.uiConfig.SortOrder - y.uiConfig.SortOrder);
                    await loadTask;
                    await ui.onTask;
                    if (ui.Disposed)
                        return;

                    for (int i = _uiLst.Count - 1; i >= 0; i--)
                    {
                        UIBase tmp = _uiLst[i];
                        if (tmp == ui)
                            continue;
                        if (tmp.uiConfig.HideOnOpenOtherUI && tmp.isShow && tmp.uiConfig.UIType < UIType.GlobalUI)
                        {
                            tmp.Hide();
                            break;
                        }
                    }
                    ui.ListenerEnable = true;
                    Timer.AutoRigisterTimer(ui);
                    ui.Show();
                    UIHelper.EnableUIInput(true);

                    ((TaskAwaiter<T>)ui.onCompleted).TrySetResult(ui);
                }
            }
          
            return (TaskAwaiter<T>)ui.onCompleted;
        }

        public T OpenSubUI<T>(UIBase parent, params object[] data) where T : UIBase, new()
        {
            T ui = parent.GetSubUI<T>();
            if (ui != null)
                return ui;

            if (Types.GetAttribute(typeof(T), typeof(Main.UIConfig)) is not Main.UIConfig cfg)
                cfg = Main.UIConfig.Default;

            ui = new();
            parent.AddChild(ui);
            ui.LoadConfig(cfg, new TaskAwaiter<T>(), data);
            ui.Show();
            ((TaskAwaiter<T>)ui.onCompleted).TrySetResult(ui);

            return ui;
        }
        public TaskAwaiter<T> OpenSubUIAsync<T>(UIBase parent, params object[] data) where T : UIBase, new()
        {
            T ui = parent.GetSubUI<T>();
            if (ui == null)
            {
                open();
                async void open()
                {
                    if (Types.GetAttribute(typeof(T), typeof(Main.UIConfig)) is not Main.UIConfig cfg)
                        cfg = Main.UIConfig.Default;

                    UIHelper.EnableUIInput(false);
                    ui = new();
                    //在执行异步的过程中有可能会关闭这个UI
                    ui.onDispose.Add(() =>
                    {
                        if (ui.uiStates < UIStates.Success)
                            UIHelper.EnableUIInput(true);
                    });
                    parent.AddChild(ui);
                    await ui.LoadConfigAsync(cfg, new TaskAwaiter<T>(), data);
                    await ui.onTask;
                    if (ui.Disposed)
                        return;

                    ui.ListenerEnable = true;
                    Timer.AutoRigisterTimer(ui);
                    ui.Show();
                    UIHelper.EnableUIInput(true);

                    ((TaskAwaiter<T>)ui.onCompleted).TrySetResult(ui);
                }
            }
           
            return (TaskAwaiter<T>)ui.onCompleted;
        }

        public T Open3D<T>(params object[] data) where T : UIBase, new()
        {
            if (Types.GetAttribute(typeof(T), typeof(Main.UIConfig)) is not Main.UIConfig cfg)
                cfg = Main.UIConfig.Default;

            T ui = new();
            _3duiLst.Add(ui);
            ui.LoadConfig(cfg, new TaskAwaiter<T>(), data);
            ((TaskAwaiter<T>)ui.onCompleted).TrySetResult(ui);

            return ui;
        }
        public TaskAwaiter<T> Open3DAsync<T>(params object[] data) where T : UIBase, new()
        {
            TaskAwaiter<T> task = new();
            open();
            async void open()
            {
                if (Types.GetAttribute(typeof(T), typeof(Main.UIConfig)) is not Main.UIConfig cfg)
                    cfg = Main.UIConfig.Default;

                T ui = new();
                _3duiLst.Add(ui);
                await ui.LoadConfigAsync(cfg, task, data);
                await ui.onTask;
                if (ui.Disposed)
                    return;

                ui.ListenerEnable = true;
                Timer.AutoRigisterTimer(ui);

                task.TrySetResult(ui);
            }
            return task;
        }

        public T Get<T>() where T : UIBase
        {
            return _uiLst.Find(t => t is T) as T;
        }

        /// <summary>
        /// 关闭UI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Close<T>() where T : UIBase
        {
            T ui = Get<T>();
            ui?.Dispose();
        }

        /// <summary>
        /// Dispose的时候会回调到Remove
        /// </summary>
        public void CloseAll()
        {
            int len = _uiLst.Count;
            for (; len > 0; len--)
            {
                UIBase ui = _uiLst[len - 1];
                if (ui.uiConfig.UIType >= UIType.GlobalUI)
                    continue;
                ui.Dispose();
            }
            len = _3duiLst.Count;
            for (; len > 0; len--)
                _3duiLst[len - 1].Dispose();
        }
        public void CloseOnlyUI()
        {
            int len = _uiLst.Count;
            for (; len > 0; len--)
            {
                var ui = _uiLst[len - 1];
                if (ui.uiConfig.UIType >= UIType.GlobalUI)
                    continue;
                ui.Dispose();
            }
        }
        public void CloseOnlyUI3D()
        {
            int len = _3duiLst.Count;
            for (; len > 0; len--)
                _3duiLst[len - 1].Dispose();
        }

        /// <summary>
        /// 只移除 Close的时候会回调到这里
        /// </summary>
        /// <param name="ui"></param>
        public void Remove(UIBase ui)
        {
            if (ui is FUI3D || ui is UUI3D)
                _3duiLst.Remove(ui);
            else
                _uiLst.Remove(ui);
        }

        public bool ShowLastUI()
        {
            for (int i = _uiLst.Count - 1; i >= 0; i--)
            {
                UIBase ui = _uiLst[i];
                if (!ui.isShow)
                {
                    ui.Show();
                    return true;
                }
            }
            return false;
        }
    }
}
