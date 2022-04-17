using System.Collections.Generic;
using UnityEngine;

public static class LanguageS
{
    public static SystemLanguage LanguageType { get; set; } = SystemLanguage.Chinese;

    static Dictionary<int, Language> map;
    public static string ToLan(this int key)
    {
        if (!map.TryGetValue(key, out var ret))
        {
            Loger.Error("Language没有key:" + key);
            return string.Empty;
        }

        if (LanguageType == SystemLanguage.Chinese)
            return ret.cn;

        return ret.en;
    }

    public static void Init(DBuffer buff)
    {
        int len = buff.ReadInt();
        map = new Dictionary<int, Language>(len);
        for (int i = 0; i < len; i++)
        {
            Language t = new Language(buff);
            map.Add(t.id, t);
        }
    }
}