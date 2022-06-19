using System.Collections.Generic;
using UnityEngine;
using System;

public static class LanguageS
{
    class Language
    {
        public Dictionary<int, Mapping> kvs;
        public DBuffer buff;
    }
    struct Mapping
    {
        public int key;
        public bool isReaded;
        public int index;
        public string value;
    }

    static readonly Language[] languageArray = new Language[(int)SystemLanguage.Unknown];

    public static SystemLanguage LanguageType { get; set; } = SystemLanguage.Chinese;

    public static string ToLan(this int key)
    {
        Language lan = languageArray[(int)LanguageType];

        if (lan == null)
        {
            Loger.Error("没有加载语言包 " + LanguageType);
            return string.Empty;
        }

        if (!lan.kvs.TryGetValue(key, out Mapping kv))
        {
            Loger.Error("Language没有key:" + key);
            return string.Empty;
        }

        if (!kv.isReaded)
        {
            lan.buff.Seek(kv.index);
            kv.value = lan.buff.ReadString();
            kv.isReaded = true;
            lan.kvs[kv.key] = kv;
        }

        return kv.value;
    }

    public static void Load(int languageType, DBuffer buff)
    {
        int len = buff.ReadInt();
        Language lan = languageArray[languageType] = new Language();
        lan.buff = buff;
        lan.kvs = new Dictionary<int, Mapping>(len);
        for (int i = 0; i < len; i++)
        {
            Mapping map = new();
            map.key = buff.ReadInt();
            map.index = buff.Position;
            buff.Seek(buff.Position + buff.ReadInt());
            lan.kvs.Add(map.key, map);
        }
    }
    public static void Clear()
    {
        Array.Clear(languageArray, 0, languageArray.Length);
    }
}