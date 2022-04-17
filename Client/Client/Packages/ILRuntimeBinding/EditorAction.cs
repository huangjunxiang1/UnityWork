using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using ILRuntime.Runtime.Stack;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.CLR.Utils;
using ILRuntime.CLR.Method;

unsafe static class EditorAction
{
    public static void Binding(ILRuntime.Runtime.Enviorment.AppDomain appdomain)
    {
#if UNITY_EDITOR
        Application.logMessageReceived += log;
#endif

#if DEBUG && (UNITY_EDITOR || UNITY_ANDROID || UNITY_IPHONE)
        //由于Unity的Profiler接口只允许在主线程使用，为了避免出异常，需要告诉ILRuntime主线程的线程ID才能正确将函数运行耗时报告给Profiler
        appdomain.UnityMainThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
#endif

#if UNITY_EDITOR || DebugEnable
        appdomain.DebugService.StartDebugService(56000);

        //这里做一些ILRuntime的注册
        var mi = typeof(Loger).GetMethod("Log", new System.Type[] { typeof(object) });
        appdomain.RegisterCLRMethodRedirection(mi, LogerLog);
        mi = typeof(Loger).GetMethod("Error", new System.Type[] { typeof(object) });
        appdomain.RegisterCLRMethodRedirection(mi, LogerError);
        mi = typeof(Loger).GetMethod("Warning", new System.Type[] { typeof(object) });
        appdomain.RegisterCLRMethodRedirection(mi, LogerWarning);
#endif
    }


#if UNITY_EDITOR || DebugEnable
    //编写重定向方法对于刚接触ILRuntime的朋友可能比较困难，比较简单的方式是通过CLR绑定生成绑定代码，然后在这个基础上改，比如下面这个代码是从UnityEngine_Debug_Binding里面复制来改的
    //如何使用CLR绑定请看相关教程和文档
    unsafe static StackObject* LogerLog(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
    {
        //ILRuntime的调用约定为被调用者清理堆栈，因此执行这个函数后需要将参数从堆栈清理干净，并把返回值放在栈顶，具体请看ILRuntime实现原理文档
        ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
        StackObject* ptr_of_this_method;
        //这个是最后方法返回后esp栈指针的值，应该返回清理完参数并指向返回值，这里是只需要返回清理完参数的值即可
        StackObject* __ret = ILIntepreter.Minus(__esp, 1);
        //取Log方法的参数，如果有两个参数的话，第一个参数是esp - 2,第二个参数是esp -1, 因为Mono的bug，直接-2值会错误，所以要调用ILIntepreter.Minus
        ptr_of_this_method = ILIntepreter.Minus(__esp, 1);

        //这里是将栈指针上的值转换成object，如果是基础类型可直接通过ptr->Value和ptr->ValueLow访问到值，具体请看ILRuntime实现原理文档
        object message = typeof(object).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
        //所有非基础类型都得调用Free来释放托管堆栈
        __intp.Free(ptr_of_this_method);

        //在真实调用Debug.Log前，我们先获取DLL内的堆栈
        var stacktrace = __domain.DebugService.GetStackTrace(__intp);

        //我们在输出信息后面加上DLL堆栈
        Loger.Log(message + "\n" + stacktrace);

        return __ret;
    }
    unsafe static StackObject* LogerError(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
    {
        //ILRuntime的调用约定为被调用者清理堆栈，因此执行这个函数后需要将参数从堆栈清理干净，并把返回值放在栈顶，具体请看ILRuntime实现原理文档
        ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
        StackObject* ptr_of_this_method;
        //这个是最后方法返回后esp栈指针的值，应该返回清理完参数并指向返回值，这里是只需要返回清理完参数的值即可
        StackObject* __ret = ILIntepreter.Minus(__esp, 1);
        //取Log方法的参数，如果有两个参数的话，第一个参数是esp - 2,第二个参数是esp -1, 因为Mono的bug，直接-2值会错误，所以要调用ILIntepreter.Minus
        ptr_of_this_method = ILIntepreter.Minus(__esp, 1);

        //这里是将栈指针上的值转换成object，如果是基础类型可直接通过ptr->Value和ptr->ValueLow访问到值，具体请看ILRuntime实现原理文档
        object message = typeof(object).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
        //所有非基础类型都得调用Free来释放托管堆栈
        __intp.Free(ptr_of_this_method);

        //在真实调用Debug.Log前，我们先获取DLL内的堆栈
        var stacktrace = __domain.DebugService.GetStackTrace(__intp);

        //我们在输出信息后面加上DLL堆栈
        Loger.Error(message + "\n" + stacktrace);

        return __ret;
    }
    unsafe static StackObject* LogerWarning(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
    {
        //ILRuntime的调用约定为被调用者清理堆栈，因此执行这个函数后需要将参数从堆栈清理干净，并把返回值放在栈顶，具体请看ILRuntime实现原理文档
        ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
        StackObject* ptr_of_this_method;
        //这个是最后方法返回后esp栈指针的值，应该返回清理完参数并指向返回值，这里是只需要返回清理完参数的值即可
        StackObject* __ret = ILIntepreter.Minus(__esp, 1);
        //取Log方法的参数，如果有两个参数的话，第一个参数是esp - 2,第二个参数是esp -1, 因为Mono的bug，直接-2值会错误，所以要调用ILIntepreter.Minus
        ptr_of_this_method = ILIntepreter.Minus(__esp, 1);

        //这里是将栈指针上的值转换成object，如果是基础类型可直接通过ptr->Value和ptr->ValueLow访问到值，具体请看ILRuntime实现原理文档
        object message = typeof(object).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
        //所有非基础类型都得调用Free来释放托管堆栈
        __intp.Free(ptr_of_this_method);

        //在真实调用Debug.Log前，我们先获取DLL内的堆栈
        var stacktrace = __domain.DebugService.GetStackTrace(__intp);

        //我们在输出信息后面加上DLL堆栈
        Loger.Warning(message + "\n" + stacktrace);

        return __ret;
    }
#endif


#if UNITY_EDITOR
    static void log(string condition, string stackTrace, LogType type)
    {
        if (condition.Contains("Please add following code:"))
        {
            string[] s = condition.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i].Contains("Please add following code:"))
                {
                    string path = Application.dataPath + @$"\..\Packages\ILRuntimeBinding\DelegateBinding\GenerateDelegateBinding_{SystemInfo.deviceUniqueIdentifier}.cs";
                    if (!File.Exists(path))
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("using System;");
                        sb.AppendLine("");
                        sb.AppendLine($"unsafe static class GenerateDelegateBinding_{SystemInfo.deviceUniqueIdentifier}");
                        sb.AppendLine("{");
                        sb.AppendLine("    [ILRuntimeDelegateBinding]");
                        sb.AppendLine("    static void Binding(ILRuntime.Runtime.Enviorment.AppDomain appdomain)");
                        sb.AppendLine("    {");
                        sb.AppendLine("        //xx1Start");
                        sb.AppendLine("        //xx1End");
                        sb.AppendLine("");
                        sb.AppendLine("        //xx2Start");
                        sb.AppendLine("        //xx2End");
                        sb.AppendLine("    }");
                        sb.AppendLine("}");
                        File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
                    }

                    if (s[i].Contains("Cannot find Delegate Adapter"))
                    {
                        string ctx = s[i + 1];

                        AppendBinding(path, "xx1Start", "        " + ctx);
                    }
                    else if (i > 0 && s[i - 1].Contains("Cannot find convertor"))
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("        " + s[i + 1]);
                        sb.AppendLine("        " + s[i + 2]);
                        sb.AppendLine("        " + s[i + 3]);
                        sb.AppendLine("        " + s[i + 4]);

                        string ss = s[i + 5];
                        if (ss.IndexOf("<") + 1 == ss.IndexOf(">"))
                            ss = ss.Replace("<>", null);
                        sb.AppendLine("        " + ss);

                        sb.AppendLine("        " + s[i + 6]);
                        sb.Append("        " + s[i + 7]);
                        AppendBinding(path, "xx2Start", sb.ToString());
                    }
                    else
                    {
                        Loger.Error("分析不出来的注册类型");
                    }
                }
            }

        }
    }
    static void AppendBinding(string path, string mark, string content)
    {
        if (File.ReadAllText(path).Contains(content))
        {
            return;
        }
        List<string> cs = File.ReadAllLines(path, Encoding.UTF8).ToList();
        int idx = cs.FindIndex(t => t.Contains(mark));
        if (idx == -1)
        {
            Loger.Error("autoGenerateDelegateAdapter Cant find markLine");
            return;
        }
        cs.Insert(idx + 1, content);
        File.WriteAllLines(path, cs, Encoding.UTF8);
    }
#endif
}
