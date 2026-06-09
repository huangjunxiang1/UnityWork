using System;
using System.Collections.Generic;
using System.Text;
using YooAsset;

static class YooPkg
{
    public static ResourcePackage _Res { get; private set; }
    public static ResourcePackage _Raw { get; private set; }

    public static void Init(ResourcePackage res, ResourcePackage raw)
    {
        _Res = res;
        _Raw = raw;
    }

    public static class Res
    {
        public static YooPackageLoader UI = new(_Res, "UI");
        public static YooPackageLoader _3D = new(_Res, "3D");
    }
}
