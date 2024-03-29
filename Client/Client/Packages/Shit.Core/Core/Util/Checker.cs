using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    internal static class Checker
    {
        [Conditional(ConstDefCore.DebugEnableString)]
        public static void Check(List<Type> types)
        {
            for (int i = 0; i < types.Count; i++)
            {
                var type = types[i];
                var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

                for (int j = 0; j < methods.Length; j++)
                {
                    var method = methods[j];
                    if (method.GetCustomAttributes<SAttribute>().Count() > 1)
                    {
                        Loger.Error($"{method.ReflectedType.FullName}  {method.Name}  属性多于1个");
                        continue;
                    }
                    var ps = method.GetParameters();
                    var att = method.GetCustomAttribute<SAttribute>();

                    //timer
                    {
                        if (att is TimerAttribute)
                        {
                            if (method.IsGenericMethod)
                                Loger.Error($"{method.ReflectedType.FullName}  {method.Name}  timer函数不能是泛型函数");
                            if (method.ReturnType != typeof(void))
                                Loger.Error($"{method.ReflectedType.FullName}  {method.Name}  timer函数必须没有返回值");

                            if (ps.Length > 1)
                                Loger.Error($"Timer事件函数参数个数最多为1 class:{method.ReflectedType.FullName} method:{method.Name}");
                            if (ps.Length == 1)
                            {
                                if (typeof(__Timer).IsAssignableFrom(ps[0].ParameterType) && typeof(__Timer) != ps[0].ParameterType)
                                {
                                    if (!method.IsStatic)
                                        Loger.Error($"组合Timer事件函数必须是静态 class:{method.ReflectedType.FullName} method:{method.Name}");
                                }
                                else
                                    Loger.Error($"Timer事件函数参数类型错误 class:{method.ReflectedType.FullName} method:{method.Name}");
                            }
                        }
                        if (ps.Length > 0 && typeof(__Timer).IsAssignableFrom(ps[0].ParameterType) && typeof(__Timer) != ps[0].ParameterType)
                        {
                            if (att is not TimerAttribute)
                                Loger.Error($"Timer事件函数参数标记类型必须是{nameof(TimerAttribute)} class:{method.ReflectedType.FullName} method:{method.Name}");
                        }
                    }

                    //Event
                    if(att is EventAttribute)
                    {
                        Check(method);
                    }
                }
            }
        }
        [Conditional(ConstDefCore.DebugEnableString)]
        public static void Check(MethodInfo method)
        {
            var att = method.GetCustomAttribute<EventAttribute>();
            var ps = method.GetParameters();

            if (method.IsGenericMethod)
                Loger.Error($"事件函数不能是泛型函数  class:{method.ReflectedType.FullName} method:{method.Name}");
            if (ps.Length > 2)
                Loger.Error($"参数类型大于2 class:{method.ReflectedType.FullName} method:{method.Name}");
            if (method.ReturnType != typeof(void) && method.ReturnType != typeof(STask))
                Loger.Error($"事件函数的返回类型只能是void或者{nameof(STask)} class:{method.ReflectedType.FullName} method:{method.Name}");
            if (ps.Length == 2)
            {
                if (ps[1].ParameterType != typeof(EventHandler))
                    Loger.Error($"无法解析的参数类型 class:{method.ReflectedType.FullName} method:{method.Name}");
            }
            if (ps.Length == 1)
            {
                if (ps[0].ParameterType.IsPrimitive)
                    Loger.Error($"不要使用系统值类型作为事件参数类型  class:{method.ReflectedType.FullName} method:{method.Name}");

                if (typeof(__ChangeHandle).IsAssignableFrom(ps[0].ParameterType))
                {
                    if (method != null && method.ReturnType == typeof(STask))
                        Loger.Error($"change处理事件不可使用task class:{method.ReflectedType.FullName} method:{method.Name}");
                }
                if (typeof(__EventWatcher).IsAssignableFrom(ps[0].ParameterType))
                {
                    if (att.RPC)
                        Loger.Error($"EventWatcher处理事件 本身就是rpc事件 不可标记为rpc class:{method.ReflectedType.FullName}  method: {method.Name}");
                }
                if (typeof(__KVWatcher).IsAssignableFrom(ps[0].ParameterType))
                {
                    if (att.Type == 0)
                        Loger.Error($"KVWatcher处理事件 需要标记Type class:{method.ReflectedType.FullName}   method:  {method.Name}");
                    if (att.RPC)
                        Loger.Error($"KVWatcher处理事件 本身就是rpc事件 不可标记为rpc class:{method.ReflectedType.FullName}   method:  {method.Name}");
                }
                if (typeof(__UpdateHandle).IsAssignableFrom(ps[0].ParameterType))
                {
                    if (method != null && method.ReturnType == typeof(STask))
                        Loger.Error($"update处理事件不可使用task class:{method.ReflectedType.FullName}    method:   {method.Name}");
                }
            }
        }
    }
}
