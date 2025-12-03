using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static partial class SSetting
{
    public static partial class ViewSetting
    {
        public static int OcTree_MaxTreesPerNode = 32;
        public static int OcTree_MaxOcTreeDepth = 16;
        public static SystemLanguage LanguageType { get; set; } = SystemLanguage.Unknown;

    }
}
