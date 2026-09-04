using UnityEditor;
using UnityEngine;
using TMPro;

public class TMPFontAssetCleaner : AssetModificationProcessor
{
    // 当Unity准备保存资产时，此方法会被调用
    public static string[] OnWillSaveAssets(string[] paths)
    {
        foreach (string path in paths)
        {
            // 只处理 .asset 文件
            if (!path.EndsWith(".asset")) continue;

            // 尝试加载资产并检查是否为 TMP_FontAsset
            var font = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(path);
            if (font == null) continue;

            // 如果是动态字体，则清除其动态生成的纹理数据
            if (font.atlasPopulationMode == AtlasPopulationMode.Dynamic)
            {
                // 参数 'true' 表示强制清除所有动态添加的数据
                font.ClearFontAssetData(true);
                Debug.Log($"Cleared dynamic data for font asset: {path}");
            }
        }
        return paths; // 返回修改后的路径列表
    }
}