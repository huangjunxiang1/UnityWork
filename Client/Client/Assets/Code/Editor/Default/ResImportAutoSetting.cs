using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.Bindings;

public class ResImportAutoSetting : AssetPostprocessor
{
    void OnPreprocessAsset()
    {
        if (this.assetPath.StartsWith("Assets/Art/UI/uui") || this.assetPath.StartsWith("Assets/Art/UI/items"))
        {
            if (this.assetImporter is TextureImporter ti)
                uiSprite(ti);
            return;
        }
        if (this.assetPath.StartsWith("Assets/Res/UI"))
        {
            if (this.assetImporter is TextureImporter ti)
            {
                texture(ti);
                if (this.assetPath.StartsWith("Assets/Res/UI/FUI/ComPkg"))
                {
                    EditorUtility.DisplayDialog("错误", "FUI的组件包不应该导出texture 请检查导出设置", "确定");
                }
            }
            return;
        }
    }

    void uiSprite(TextureImporter ti)
    {
        ti.textureType = TextureImporterType.Sprite;
    }

    void texture(TextureImporter ti)
    {
        ti.mipmapEnabled = false;
    }
}