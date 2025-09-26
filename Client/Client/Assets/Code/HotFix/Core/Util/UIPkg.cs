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

    public static UnityEngine.U2D.SpriteAtlas uui_items { get; private set; }
    public static UnityEngine.U2D.SpriteAtlas uui_res { get; private set; }

    [Event(-1)]
    static async STask Init(EC_GameStart e)
    {
        FUIBinder.Binding();
        UIPkg.ComPkg = UIPackage.AddPackage(Pkg.LoadRaw("raw_ComPkg_fui"), "ComPkg", fguiLoader);
        UIPkg.ResPkg = UIPackage.AddPackage((await SAsset.LoadAsync<TextAsset>("UI_ResPkg_fui")).bytes, "ResPkg", fguiLoader);
        UIPkg.Items = UIPackage.AddPackage((await SAsset.LoadAsync<TextAsset>("UI_Items_fui")).bytes, "Items", fguiLoader);
        UIGlobalConfig.isTouchUI += UIHelper.IsOnTouchFUI;

        if (Pkg.res.CheckLocationValid("UI_Items"))
            uui_items = SAsset.Load<UnityEngine.U2D.SpriteAtlas>("UI_Items");
        if (Pkg.res.CheckLocationValid("UI_UIAtlas"))
            uui_res = SAsset.Load<UnityEngine.U2D.SpriteAtlas>("UI_UIAtlas");
    }
    static async void fguiLoader(string name, string extension, System.Type type, PackageItem item)
    {
        switch (item.type)
        {
            case PackageItemType.Sound:
            case PackageItemType.Atlas:
                item.owner.SetItemAsset(item, await SAsset.LoadAsync<UnityEngine.Object>($"UI_{name}"), DestroyMethod.Custom);
                break;
            default:
                Loger.Error("未定义加载->" + item.type);
                break;
        }
    }
}
