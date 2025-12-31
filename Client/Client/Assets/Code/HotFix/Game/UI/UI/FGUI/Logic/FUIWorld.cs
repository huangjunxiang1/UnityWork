using Game;
using System;
using System.Collections.Generic;
using System.Text;
using Core;
using Unity.Mathematics;

partial class FUIWorld
{
    WorldScene world = Client.Scene.Current.As<WorldScene>();
    SGameObject player;
    protected override void OnEnter()
    {
        player = this.GetParam<SGameObject>(0);
        player.AddComponent<ViewHexComponent>().ui = this;
        this._logging.onClick.Add(on_logging);
    }

    void on_logging()
    {
        int2 xy = Hex.GetGridxy(player.Transform.position);
        world.Logging(xy);
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
        int2 xy = Hex.GetGridxy(t.position);
        int2 center = Hex.GetCenterGrid(xy);
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
