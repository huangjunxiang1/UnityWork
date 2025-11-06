using System.Collections.Generic;
using UnityEngine;
using System;

public static class LanguageUtil
{
    class Language
    {
        public Dictionary<int, Mapping> kvs_int;
        public Dictionary<string, Mapping> kvs_str;
        public Dictionary<string, Mapping2> kvs_str2;
        public DBuffer buff;
    }
    struct Mapping
    {
        public int index;
        public string value;
    }
    struct Mapping2
    {
        public int index;
        public string[] value;
    }

    static readonly Language[] languageArray = new Language[(int)SystemLanguage.Unknown];

    public static SystemLanguage LanguageType { get; set; } = SystemLanguage.Unknown;

    public static string ToLan(this int key)
    {
        Language lan = languageArray[(int)LanguageType];

        if (lan == null)
        {
            Loger.Error($"没有加载语言包 SystemLanguage={LanguageType}");
            return string.Empty;
        }

        if (!lan.kvs_int.TryGetValue(key, out Mapping kv))
        {
            Loger.Error($"Language没有key:{key} SystemLanguage={LanguageType}");
            return string.Empty;
        }

        if (kv.value == null)
        {
            lan.buff.Seek(kv.index);
            kv.value = lan.buff.Readstring();
            lan.kvs_int[key] = kv;
        }

        return kv.value;
    }
    public static string ToLan(this int key, params object[] args)
    {
        return string.Format(key.ToLan(), args);
    }
    public static string ToLan(this string key)
    {
        Language lan = languageArray[(int)LanguageType];

        if (lan == null)
        {
            Loger.Error($"没有加载语言包 SystemLanguage={LanguageType}");
            return string.Empty;
        }

        if (!lan.kvs_str.TryGetValue(key, out Mapping kv))
        {
            Loger.Error($"Language没有key:{key} SystemLanguage={LanguageType}");
            return string.Empty;
        }

        if (kv.value == null)
        {
            lan.buff.Seek(kv.index);
            kv.value = lan.buff.Readstring();
            lan.kvs_str[key] = kv;
        }

        return kv.value;
    }
    public static string ToLan(this string key, params object[] args)
    {
        return string.Format(key.ToLan(), args);
    }
    public static string ToAuto(this string key)
    {
        if (LanguageType == SystemLanguage.Chinese || LanguageType == SystemLanguage.ChineseSimplified)
            return key;
        return key.ToLan();
    }
    public static string ToAuto(this string key, params object[] args)
    {
        return string.Format(key.ToAuto(), args);
    }
    public static string[] ToLans(this string key)
    {
        Language lan = languageArray[(int)LanguageType];

        if (lan == null)
        {
            Loger.Error($"没有加载语言包 SystemLanguage={LanguageType}");
            return Array.Empty<string>();
        }

        if (!lan.kvs_str2.TryGetValue(key, out Mapping2 kv))
        {
            Loger.Error($"Language没有key:{key} SystemLanguage={LanguageType}");
            return Array.Empty<string>();
        }

        if (kv.value == null)
        {
            lan.buff.Seek(kv.index);
            kv.value = lan.buff.Readstrings();
            lan.kvs_str2[key] = kv;
        }

        return kv.value;
    }
    public static void Load(int languageType, DBuffer buff, bool isDebug)
    {
        Language lan = languageArray[languageType] = new Language();
        lan.buff = buff;

        int len = buff.Readint();
        lan.kvs_int = new Dictionary<int, Mapping>(len);
        for (int i = 0; i < len; i++)
        {
            Mapping map = new();
            int key = buff.Readint();
            map.index = buff.Position;
            if (isDebug) map.value = buff.Readstring();
            else buff.Seek(buff.Readint() + buff.Position);
            lan.kvs_int.Add(key, map);
        }

        len = buff.Readint();
        lan.kvs_str = new Dictionary<string, Mapping>(len);
        for (int i = 0; i < len; i++)
        {
            Mapping map = new();
            string key = buff.Readstring();
            map.index = buff.Position;
            if (isDebug) map.value = buff.Readstring();
            else buff.Seek(buff.Readint() + buff.Position);
            lan.kvs_str.Add(key, map);
        }

        len = buff.Readint();
        lan.kvs_str2 = new Dictionary<string, Mapping2>(len);
        for (int i = 0; i < len; i++)
        {
            Mapping2 map = new();
            string key = buff.Readstring();
            map.index = buff.Position;
            if (isDebug) map.value = buff.Readstrings();
            else buff.Seek(buff.Readint() + buff.Position);
            lan.kvs_str2.Add(key, map);
        }
    }
    public static void Clear()
    {
        Array.Clear(languageArray, 0, languageArray.Length);
    }
    public static void Clear(int languageType)
    {
        languageArray[languageType] = null;
    }
}