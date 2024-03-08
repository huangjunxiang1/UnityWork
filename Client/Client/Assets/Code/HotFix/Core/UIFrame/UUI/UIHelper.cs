using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

static partial class UIHelper
{
    static UnityEngine.U2D.SpriteAtlas Items = SAsset.Load<UnityEngine.U2D.SpriteAtlas>("UI/UUI/Atlas/Items.spriteatlasv2");
    static UnityEngine.U2D.SpriteAtlas UIAtlas = SAsset.Load<UnityEngine.U2D.SpriteAtlas>("UI/UUI/Atlas/UIAtlas.spriteatlasv2");
    public static Sprite ToUUIItemUrl(this string name) => Items.GetSprite(name);
    public static Sprite ToUUIResUrl(this string name) => UIAtlas.GetSprite(name);
}