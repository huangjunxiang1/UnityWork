using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Main;
using FairyGUI;

static class UIHelper
{
    static UnityEngine.EventSystems.EventSystem eventSysCurrent;
    static int EnableCounter = 0;
    readonly static Dictionary<string, FuiTextureLoad> fuiTexLoad = new();
    readonly static Dictionary<GLoader, string> loaderUrl = new();
    readonly static Dictionary<RawImage, TaskAwaiter> uuiTexLoadTask = new();

    public static void EnableUIInput(bool enable)
    {
        if (!enable) EnableCounter++;
        else EnableCounter--;

        if (GameL.Setting.UIModel == UIModel.UGUI)
        {
            if (!eventSysCurrent)
                eventSysCurrent = UnityEngine.EventSystems.EventSystem.current;
            if (eventSysCurrent)
                eventSysCurrent.enabled = EnableCounter <= 0;
        }
        else
            GRoot.inst.touchable = EnableCounter <= 0;
    }

    public static bool IsOnTouchFUI()
    {
        GObject g = GRoot.inst.touchTarget;
        while (g != null)
        {
            if (g == GRoot.inst) return true;
            g = g.parent;
        }
        return false;
    }
    public static bool IsOnTouchFUI(Vector2 position)
    {
        position.y = Screen.height - position.y;
        DisplayObject o = Stage.inst.HitTest(position, true);
        GObject g = GRoot.inst.DisplayObjectToGObject(o);
        while (g != null)
        {
            if (g == GRoot.inst) return true;
            g = g.parent;
        }
        return false;
    }

    public static Vector2 WorldToFUI(Vector3 world)
    {
        Vector3 sp = Camera.main.WorldToScreenPoint(world);
        //原点位置转换
        sp.y = Screen.height - sp.y;
        return sp;
    }
    public static Vector2 WorldToGObject(Vector3 world, GObject g)
    {
        Vector2 p = WorldToFUI(world);
        p.x -= g.width / 2;
        p.y -= g.height / 2;
        return p;
    }

    public static string GetFGUIItemUrl(string name)
    {
        PackageItem pi = UIPkg.ResPkg.GetItem(name);
        if (pi == null)
            return null;
        return $"{UIPackage.URL_PREFIX}{UIPkg.ResPkg.id}{pi.id}";
    }

    public static TaskAwaiter SetTexture(this GLoader loader, string url)
    {
        if (loaderUrl.TryGetValue(loader, out string last))
            fuiTexLoad[last].loaders.Remove(loader);
        loaderUrl[loader] = url;

        if (!fuiTexLoad.TryGetValue(url, out FuiTextureLoad load))
        {
            fuiTexLoad[url] = load = new FuiTextureLoad();
            load.loaders = new List<GLoader>();
            load.loaders.Add(loader);
            load.task = new TaskAwaiter();
            fuiLoadTex(url);
        }
        else
        {
            if (load.nt != null)
            {
                loader.texture = load.nt;
                return TaskAwaiter.Completed;
            }
            else
                load.loaders.Add(loader);
        }
        return load.task;
    }
    public static TaskAwaiter SetTexture(this RawImage ri, string url)
    {
        if (uuiTexLoadTask.TryGetValue(ri, out TaskAwaiter task))
            task.TryCancel();

        uuiTexLoadTask[ri] = task = new TaskAwaiter(url);
        uuiLoadTex(ri, url, task);
        return task;
    }

    async static void fuiLoadTex(string url)
    {
        Texture tex = await AssetLoad.LoadAsync<Texture>(url);
        FuiTextureLoad load = fuiTexLoad[url];

        bool has = false;
        int len = load.loaders.Count;
        for (int i = 0; i < len; i++)
        {
            GLoader loader = load.loaders[i];
            if (!loader.isDisposed)
            {
                has = true;
                break;
            }
        }

        if (!has)
        {
            AssetLoad.Release(tex);
            for (int i = 0; i < len; i++)
            {
                GLoader loader = load.loaders[i];
                loaderUrl.Remove(loader);
            }
            fuiTexLoad.Remove(url);
        }
        else
        {
            NTexture nt = new NTexture(tex);
            nt.destroyMethod = DestroyMethod.Custom;
            nt.onRelease += v =>
            {
                fuiTexLoad.Remove(url);
                v.Dispose();
            };
            load.nt = nt;

            for (int i = 0; i < len; i++)
            {
                GLoader loader = load.loaders[i];
                loaderUrl.Remove(loader);
                if (!loader.isDisposed)
                    loader.texture = nt;
            }
            load.loaders.Clear();


            load.task.TrySetResult();
        }
    }
    async static void uuiLoadTex(RawImage ri, string url, TaskAwaiter task)
    {
        Texture tex = await AssetLoad.LoadAsync<Texture>(url);
        //ui已关闭并销毁
        if (!ri)
        {
            AssetLoad.Release(tex);
            return;
        }
        //被新的url替代了
        if (!task.IsCompleted && task.IsDisposed)
        {
            AssetLoad.Release(tex);
            return;
        }
      
        uuiTexLoadTask.Remove(ri);
        ri.texture = tex;
        AssetLoad.AddTextureRef(ri.gameObject, tex);
        task.TrySetResult();
    }

    class FuiTextureLoad
    {
        public TaskAwaiter task;
        public List<GLoader> loaders;
        public NTexture nt;
    }
}
