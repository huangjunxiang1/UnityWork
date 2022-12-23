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

            NTexture.CustomDestroyMethod += textureUnLoad;
            NAudioClip.CustomDestroyMethod += audioUnLoad;
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
            }
        }
        async void fguiLoader(string name, string extension, System.Type type, PackageItem item)
        {
            switch (item.type)
            {
                case PackageItemType.Sound:
                    {
                        var task = AssetLoad.LoadAsync<AudioClip>($"UI/FUI/{item.owner.name}/{name}{extension}");
                        await task;
                        item.owner.SetItemAsset(item, task.GetResult(), DestroyMethod.Custom);
                    }
                    break;
                case PackageItemType.Atlas:
                    {
                        var task = AssetLoad.LoadAsync<Texture>($"UI/FUI/{item.owner.name}/{name}{extension}");
                        await task;
                        item.owner.SetItemAsset(item, task.GetResult(), DestroyMethod.Custom);
                    }
                    break;
                default:
                    Loger.Error("未定义加载->" + item.type);
                    break;
            }
        }
        void textureUnLoad(Texture texture)
        {
            AssetLoad.Release(texture);
        }
        void audioUnLoad(AudioClip audio)
        {
            AssetLoad.Release(audio);
        }

        public T Open<T>(params object[] data) where T : UIBase, new()
        {
            T ui = Get<T>();
            if (ui != null)
                return ui;

            var os = typeof(T).GetCustomAttributes(typeof(Main.UIConfig), true);
            Main.UIConfig cfg;
            if (os.Length == 0)
                cfg = Main.UIConfig.Default;
            else
                cfg = (Main.UIConfig)os[0];

            ui = new();
            _uiLst.Add(ui);
            ui.LoadConfig(cfg, data);

            return ui;
        }
        public async TaskAwaiter<T> OpenAsync<T>(params object[] data) where T : UIBase, new()
        {
            T ui = Get<T>();
            if (ui != null)
                return ui;

            var os = typeof(T).GetCustomAttributes(typeof(Main.UIConfig), true);
            Main.UIConfig cfg;
            if (os.Length == 0)
                cfg = Main.UIConfig.Default;
            else
                cfg = (Main.UIConfig)os[0];

            ui = new();
            _uiLst.Add(ui);
            ui.LoadConfigAsync(cfg, data);
            await ui.LoadWaiter;

            return ui;
        }

        public T Open3D<T>(params object[] data) where T : UIBase, new()
        {
            var os = typeof(T).GetCustomAttributes(typeof(Main.UIConfig), true);
            Main.UIConfig cfg;
            if (os.Length == 0)
                cfg = Main.UIConfig.Default;
            else
                cfg = (Main.UIConfig)os[0];

            T ui = new();
            _3duiLst.Add(ui);
            ui.LoadConfig(cfg, data);

            return ui;
        }
        public async TaskAwaiter<T> Open3DAsync<T>(params object[] data) where T : UIBase, new()
        {
            var os = typeof(T).GetCustomAttributes(typeof(Main.UIConfig), true);
            Main.UIConfig cfg;
            if (os.Length == 0)
                cfg = Main.UIConfig.Default;
            else
                cfg = (Main.UIConfig)os[0];

            T ui = new();
            _3duiLst.Add(ui);
            ui.LoadConfigAsync(cfg, data);
            await ui.LoadWaiter;

            return ui;
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
            if (ui != null)
                ui.Dispose();
        }

        /// <summary>
        /// Dispose的时候会回调到Remove
        /// </summary>
        public void CloseAll()
        {
            int len = _uiLst.Count;
            for (; len > 0; len--)
            {
                var ui = _uiLst[len - 1];
                if (!ui.uiConfig.CloseOnChangeScene)
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
                if (!ui.uiConfig.CloseOnChangeScene)
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
    }
}
