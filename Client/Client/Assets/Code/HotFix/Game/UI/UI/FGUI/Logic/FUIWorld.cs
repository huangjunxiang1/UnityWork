using Game;
using System;
using System.Collections.Generic;
using System.Text;
using Core;
using Unity.Mathematics;

partial class FUIWorld
{
    protected override void OnEnter()
    {
        this.GetParam<SObject>(0).AddComponent<ViewHexComponent>().ui = this;
    }

    [InSystem]
    static void In(PlayerComponent p)
    {
        var ui= Client.UI.GetChild<FUIWorld>();
        if (ui != null)
            p.Entity.AddComponent<ViewHexComponent>().ui = ui;
    }
    [ChangeSystem]
    static void Change(TransformComponent t, ViewHexComponent view)
    {
        int2 xy = Hex.PositionToHex(t.position);
        int2 center = PlayerComponent.getCenterGrid(xy);
        view.ui._hex.text = $"Hex({xy.x},{xy.y}) CenterGrid({center.x},{center.y})";
    }
    class ViewHexComponent : SComponent
    {
        public FUIWorld ui;
    }

    protected override void OnView()
    {
        
    }
}
