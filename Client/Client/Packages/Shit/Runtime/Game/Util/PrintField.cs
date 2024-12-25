using Newtonsoft.Json;
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
}
