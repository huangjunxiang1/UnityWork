using Newtonsoft.Json;
using System;
using System.Collections;
using System.Diagnostics;
using System.Text;

public static class PrintField
{
    static JsonSerializerSettings settings = new JsonSerializerSettings
    {
#if !Server
        Formatting = Formatting.Indented,
#else
        Formatting = Formatting.None,
#endif
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore, // 处理循环引用
        Converters = Game.UnityMathematicsJsonConverter.Converters,
    };
    [Conditional(SSetting.CoreSetting.DebugEnableString)]
    public static void Print(string format, object o)
    {
        if (o == null)
        {
            Loger.Log(string.Format(format, "null"));
            return;
        }
        var str = string.Empty;
        try { str = JsonConvert.SerializeObject(o, settings); }
        catch (Exception ex) { Loger.Error(ex); }
        Loger.Log(string.Format(format, str.ToString()));
    }
    [Conditional(SSetting.CoreSetting.DebugEnableString)]
    public static void Print(object o)
    {
        if (o == null)
        {
            Loger.Log("null");
            return;
        }
        var str = string.Empty;
        try { str = JsonConvert.SerializeObject(o, settings); }
        catch (Exception ex) { Loger.Error(ex); }
        Loger.Log(str.ToString());
    }
}
