using YooAsset;

static class SLoader
{
    static ResourcePackage _Res;
    static ResourcePackage _Raw;

    static SLoader()
    {
        YooAssets.TryGetPackage("Res", out _Res);
        YooAssets.TryGetPackage("Raw", out _Raw);
    }

    public static class Res
    {
        public static class Group_UI
        {
            public static SAssetWarper Item_UI = new(_Res, "UI_{0}");
        }
        public static class Group_Texture
        {
            public static SAssetWarper Item_Texture = new(_Res, "Texture_{0}");
        }
        public static class Group_model
        {
            public static SAssetWarper Item_3D = new(_Res, "model_{0}");
        }
        public static class Group_config
        {
            public static SAssetWarper Item_res = new(_Res, "config_{0}");
        }
        public static class Group_scene
        {
            public static SAssetWarper Item_Scene = new(_Res, "scene_{0}");
        }
        public static class Group_other
        {
            public static SAssetWarper Item_Other = new(_Res, "other_{0}");
        }
        public static class Group_shader
        {
            public static SAssetWarper Item_Shader = new(_Res, "shader_{0}");
        }
    }
    public static class Raw
    {
        public static class Group_raw
        {
            public static SAssetWarper Item_raw = new(_Raw, "raw_{0}");
        }
    }
}
