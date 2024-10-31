using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using FairyGUI;
using Event;
using UnityEngine;

public class UIPkg
{
    public static UIPackage ComPkg { get; private set; }
    public static UIPackage ResPkg { get; private set; }
    public static UIPackage Items { get; private set; }

    [Event(-1)]
    static async STask Init(EC_GameStart e)
    {
        FUIBinder.Binding();
        FairyGUI.UIConfig.defaultFont = "Impact";
        UIPkg.ComPkg = UIPackage.AddPackage((await SAsset.LoadAsync<TextAsset>("UI_ComPkg_fui")).bytes, "ComPkg", fguiLoader);
        UIPkg.ResPkg = UIPackage.AddPackage((await SAsset.LoadAsync<TextAsset>("UI_ResPkg_fui")).bytes, "ResPkg", fguiLoader);
        UIPkg.Items = UIPackage.AddPackage((await SAsset.LoadAsync<TextAsset>("UI_Items_fui")).bytes, "Items", fguiLoader);
        FUIBase.CreateUI += (t, s) => UIPkg.ComPkg.CreateObject(s).asCom;
        FUIBase.CreateUIAsync += (t, s) =>
        {
            STask<GComponent> task = new();
            UIPkg.ComPkg.CreateObjectAsync(s, obj => task.TrySetResult(obj));
            return task;
        };
        TouchHelper.isTouchUI += UIHelper.IsOnTouchFUI;
    }
    static async void fguiLoader(string name, string extension, System.Type type, PackageItem item)
    {
        switch (item.type)
        {
            case PackageItemType.Sound:
            case PackageItemType.Atlas:
                item.owner.SetItemAsset(item, await SAsset.LoadAsync<UnityEngine.Object>($"UI_{item.owner.name}/{name}"), DestroyMethod.Custom);
                break;
            default:
                Loger.Error("未定义加载->" + item.type);
                break;
        }
    }
}
