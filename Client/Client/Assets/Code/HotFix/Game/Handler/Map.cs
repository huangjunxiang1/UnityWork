using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using Main;
using UnityEngine;

static class Map
{
    [Msg(OuterOpcode.G2C_EnterMap)]
    static async void EnterMap(IMessage message)
    {
        G2C_EnterMap rep = message as G2C_EnterMap;
        await GameL.Scene.InScene(10001);
        await GameL.UI.OpenAsync<FUIFighting>();
    }
}
