﻿using System.Collections.Generic;
using UnityEngine;
using System;

public static class LanguageS
{
    class Language
    {
        public Dictionary<int, Mapping> kvs_int;
        public Dictionary<string, Mapping> kvs_str;
        public DBuffer buff;
    }
    struct Mapping
    {
        public int index;
        public string value;
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