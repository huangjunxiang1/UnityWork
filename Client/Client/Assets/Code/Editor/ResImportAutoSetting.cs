using UnityEditor;
using UnityEngine;
using Main;
using UnityEngine.U2D;
using UnityEngine.Bindings;

public class ResImportAutoSetting : AssetPostprocessor
{
    void OnPreprocessAsset()
    {
        if (this.assetPath.StartsWith("Assets/Art/UI/uui"))
        {
            if (this.assetImporter is TextureImporter ti)
                uiSprite(ti);
            return;
        }
        if (this.assetPath.StartsWith("Assets/Res/UI/FUI"))
        {
            if (this.assetImporter is TextureImporter ti)
                fguiTex(ti);
            return;
        }
    }

    void uiSprite(TextureImporter ti)
    {
        ti.textureType = TextureImporterType.Sprite;
    }

    void fguiTex(TextureImporter ti)
    {
        ti.mipmapEnabled = false;
    }
}