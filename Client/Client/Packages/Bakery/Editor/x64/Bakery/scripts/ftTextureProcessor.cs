using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
#if UNITY_2021_2_OR_NEWER
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.Serialization.Formatters.Binary;
#endif

public class ftTextureProcessor : AssetPostprocessor
{
    public static Dictionary<string, ftRenderLightmap.Int2> texSettings = new Dictionary<string, ftRenderLightmap.Int2>();
    static BakeryProjectSettings pstorage;
    //static ftGlobalStorage gstorage;

    public const int TEX_LM = 0;
    public const int TEX_LMDEFAULT = 1;
    public const int TEX_MASK = 2;
    public const int TEX_DIR = 3;
    public const int TEX_MASK_NO_ALPHA = 4;
    public const int TEX_DIR_NO_ALPHA = 5;

#if (!UNITY_2020_2_OR_NEWER)
    // Pre-broken Unity
    void OnPreprocessTexture()
#else
    // Do not use OnPreprocessTexture on >= 2020.2.2 due to it forcing full project reimport!
    // https://forum.unity.com/threads/unity-made-full-project-reimport-on-every-small-change.1066844/
    #if (!BAKERY_INCLUDED || BAKERY_NOREIMPORT)
        // First package import OR the new define is set
        void OnPreprocessAsset()
    #else
        // Existing project - don't change the method, or the preprocessor hash (?) will change and trigger the full reimport again!
        void OnPreprocessTexture()
    #endif
#endif
    {
        TextureImporter importer = assetImporter as TextureImporter;
        if (importer == null) return;

        var settings = new ftRenderLightmap.Int2();

        bool loadFromAsset = false;
#if UNITY_2021_2_OR_NEWER
        // For parallel import
        if (UnityEditor.EditorSettings.refreshImportMode == AssetDatabase.RefreshImportMode.OutOfProcessPerQueue)
        {
            loadFromAsset = true;

            try
            {
                using (var mmf = MemoryMappedFile.OpenExisting("FTEXPROC"))
                {
                    using (var stream = mmf.CreateViewStream(0, ftRenderLightmap.maxTexSettingsSize))
                    {
                        var formatter = new BinaryFormatter();
                        var buffer = new byte[ftRenderLightmap.maxTexSettingsSize];
                        if (!stream.CanRead)
                        {
                            Debug.LogError("Can't read texture settings from memory mapped file");
                            return;
                        }
                        stream.Read(buffer, 0, ftRenderLightmap.maxTexSettingsSize);
                        var texS = formatter.Deserialize(new MemoryStream(buffer)) as Dictionary<string, ftRenderLightmap.Int2>;

                        if (!texS.TryGetValue(importer.assetPath, out settings)) return;
                    }
                }
            }
            catch(FileNotFoundException)
            {
                return;
            }
            catch(System.Exception)
            {
                throw;
            }
        }
#endif

        if (!loadFromAsset)
        {
            if (!texSettings.TryGetValue(importer.assetPath, out settings)) return;
        }

        if (pstorage == null) pstorage = ftLightmaps.GetProjectSettings();

        importer.maxTextureSize = (int)settings.x;
        importer.mipmapEnabled = pstorage.mipmapLightmaps;
        importer.wrapMode = TextureWrapMode.Clamp;

        int texType = (int)settings.y;
        switch(texType)
        {
            case TEX_LM:
            {
                importer.textureType = TextureImporterType.Lightmap;
                if (pstorage.lightmapCompression != BakeryProjectSettings.Compression.CompressButAllowOverridingAsset)
                {
                    importer.textureCompression = pstorage.lightmapCompression == BakeryProjectSettings.Compression.ForceCompress ?
                        TextureImporterCompression.Compressed : TextureImporterCompression.Uncompressed;
                }
                break;
            }
            case TEX_LMDEFAULT:
            {
                importer.textureType = TextureImporterType.Default;
                if (pstorage.lightmapCompression != BakeryProjectSettings.Compression.CompressButAllowOverridingAsset)
                {
                    importer.textureCompression = pstorage.lightmapCompression == BakeryProjectSettings.Compression.ForceCompress ?
                        TextureImporterCompression.Compressed : TextureImporterCompression.Uncompressed;
                }
                break;
            }
            case TEX_MASK:
            {
                importer.textureType = TextureImporterType.Default;
                importer.textureCompression = pstorage.lightmapCompression != BakeryProjectSettings.Compression.ForceNoCompress ? TextureImporterCompression.CompressedHQ : TextureImporterCompression.Uncompressed;
                importer.alphaSource = TextureImporterAlphaSource.FromInput;
                break;
            }
            case TEX_MASK_NO_ALPHA:
            {
                importer.textureType = TextureImporterType.Default;
                importer.textureCompression = pstorage.lightmapCompression != BakeryProjectSettings.Compression.ForceNoCompress ? TextureImporterCompression.Compressed : TextureImporterCompression.Uncompressed;
                importer.alphaSource = TextureImporterAlphaSource.None;
                break;
            }
            case TEX_DIR:
            {
                importer.textureType = TextureImporterType.Default;
                importer.textureCompression =  pstorage.lightmapCompression != BakeryProjectSettings.Compression.ForceNoCompress ? (pstorage.dirHighQuality ? TextureImporterCompression.CompressedHQ : TextureImporterCompression.Compressed) : TextureImporterCompression.Uncompressed;
                importer.sRGBTexture = (pstorage.format8bit == BakeryProjectSettings.FileFormat.PNG);
                break;
            }
            case TEX_DIR_NO_ALPHA:
            {
                importer.textureType = TextureImporterType.Default;
                importer.textureCompression = pstorage.lightmapCompression != BakeryProjectSettings.Compression.ForceNoCompress ? TextureImporterCompression.Compressed : TextureImporterCompression.Uncompressed;
                importer.alphaSource = TextureImporterAlphaSource.None;
                importer.sRGBTexture = false;//(pstorage.format8bit == BakeryProjectSettings.FileFormat.PNG);
                break;
            }
        }
    }
}

