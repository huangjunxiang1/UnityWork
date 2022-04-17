using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Main;

static class UIHelper
{
    public static async void SetTexture(this RawImage ri, string texPath)
    {
        if (!ri) return;
        Texture tex = await AssetLoad.LoadAsync<Texture>(texPath);
        AssetLoad.AddTextureRef(ri.gameObject, tex);
    }
}
