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
            uiSprite();
            return;
        }
    }

    void uiSprite()
    {
        TextureImporter ti = this.assetImporter as TextureImporter;
        ti.textureType = TextureImporterType.Sprite;
    }
}