﻿using Newtonsoft.Json;
using System;
using System.Collections;
using System.Diagnostics;
using System.Text;

public static class PrintField
{
    static Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings
    {
        Formatting = Newtonsoft.Json.Formatting.Indented,
        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore, // 处理循环引用
        Converters = Game.UnityMathematicsJsonConverter.Converters,
    };
    [Conditional(ConstDefCore.DebugEnableString)]
    public static void Print(string format, object o)
    {
        if (o == null)
        {
            Loger.Log(string.Format(format, "null"));
            return;
        }
#if !Server
        var str = string.Empty;
        try
        {
            settings.Formatting = Formatting.Indented;
            str = Newtonsoft.Json.JsonConvert.SerializeObject(o, settings);
        }
        catch (Exception ex)
        {
            Loger.Error(ex);
        }
#else
        var str = string.Empty;
        try
        {
            settings.Formatting = Formatting.None;
            str = Newtonsoft.Json.JsonConvert.SerializeObject(o, settings);
        }
        catch (Exception ex)
        {
            Loger.Error(ex);
        }
#endif
        Loger.Log(string.Format(format, str.ToString()));
    }
    [Conditional(ConstDefCore.DebugEnableString)]
    public static void Print(object o)
    {
        if (o == null)
        {
            Loger.Log("null");
            return;
        }
#if !Server
        var str = string.Empty;
        try
        {
            settings.Formatting = Formatting.Indented;
            str = Newtonsoft.Json.JsonConvert.SerializeObject(o, settings);
        }
        catch (Exception ex)
        {
            Loger.Error(ex);
        }
#else
        var str = string.Empty;
        try
        {
            settings.Formatting = Formatting.None;
            str = Newtonsoft.Json.JsonConvert.SerializeObject(o, settings);
        }
        catch (Exception ex)
        {
            Loger.Error(ex);
        }
#endif
        Loger.Log(str.ToString());
    }

    static void WriteField(StringBuilder str, object o, int layer)
    {
        if (o == null)
        {
            str.AppendLine("Null");
            return;
        }
        var type = o.GetType();
        if (type.IsPrimitive || type == typeof(string) || type.IsEnum || !type.IsClass)
        {
            str.AppendLine(o.ToString());
            return;
        }
        if (layer >= 10)
        {
            Loger.Error("层级过大");
            str.AppendLine(o.ToString());
            return;
        }

        if (o is IList lst)
        {
            str.AppendLine();
            for (int j = 0; j < lst.Count; j++)
            {
                str.Append(getSpaceInLayer(layer) + $" [{j}]=");
                WriteField(str, lst[j], layer + 1);
            }
            str.AppendLine();
            return;
        }
        else if (o is IDictionary map)
        {
            str.AppendLine();
            foreach (var item in map.Keys)
            {
                str.Append(getSpaceInLayer(layer) + "key:");
                WriteField(str, item, layer + 1);
                str.Append(getSpaceInLayer(layer) + "value:");
                WriteField(str, map[item], layer + 1);
                str.AppendLine();
            }
            return;
        }

        str.AppendLine(getSpaceInLayer(layer));
        var fields = type.GetFields();
        for (int i = 0; i < fields.Length; i++)
        {
            var f = fields[i];
            if (f.IsStatic) continue;
            str.Append(getSpaceInLayer(layer) + f.Name + ":");
            WriteField(str, f.GetValue(o), layer + 1);
        }
        var propers = type.GetProperties();
        for (int i = 0; i < propers.Length; i++)
        {
            var f = propers[i];
            str.Append(getSpaceInLayer(layer) + f.Name + ":");
            WriteField(str, f.GetValue(o), layer + 1);
        }
        str.AppendLine();
    }
    static string[] spaceStrs = new string[]
    {
        " ",
        "  ",
        "   ",
        "    ",
        "     ",
        "      ",
        "       ",
        "        ",
        "         ",
        "          ",
    };
    static string getSpaceInLayer(int layer)
    {
        return spaceStrs[layer];
    }
}
