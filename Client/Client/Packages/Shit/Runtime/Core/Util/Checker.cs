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
                    var ps = method.GetParameters();
                    var atts = method.GetCustomAttributes<SAttribute>().ToArray();

                    if (atts.FirstOrDefault(t => t is Timer) != null)
                    {
                        if (method.IsGenericMethod)
                            Loger.Error($"{method.ReflectedType.FullName}  {method.Name}  timer函数不能是泛型函数");
                        if (method.ReturnType != typeof(void))
                            Loger.Error($"{method.ReflectedType.FullName}  {method.Name}  timer函数必须没有返回值");
                        if (ps.Length != 0)
                            Loger.Error($"Timer事件函数必须是无参数 class:{method.ReflectedType.FullName} method:{method.Name}");
                    }
                    //Event
                    if (atts.FirstOrDefault(t => t is EventAttribute) != null)
                        Check(method);
                    //System
                    if (atts.FirstOrDefault(t => t is EventWatcherSystem) != null)
                    {
                        if (!method.IsStatic)
                            Loger.Error($"{method.ReflectedType.FullName}  {method.Name}  system函数必须是静态函数");
                        if (method.IsGenericMethod)
                            Loger.Error($"{method.ReflectedType.FullName}  {method.Name}  system函数不能是泛型函数");
                        if (method.ReturnType != typeof(void))
                            Loger.Error($"{method.ReflectedType.FullName}  {method.Name}  system函数必须没有返回值");
                        if (ps.Length < 2 || ps.Length > 11)
                            Loger.Error($"{method.ReflectedType.FullName}  {method.Name}  {nameof(EventWatcherSystem)}函数参数个数不正确");
                        for (int k = 1; k < ps.Length; k++)
                        {
                            if (!typeof(SComponent).IsAssignableFrom(ps[k].ParameterType))
                                Loger.Error($"{method.ReflectedType.FullName}  {method.Name}  system函数必须只有组件参数");
                        }
                    }
                    if (atts.FirstOrDefault(t => t is SystemAttribute && t is not EventWatcherSystem) != null)
                    {
                        if (!method.IsStatic)
                            Loger.Error($"{method.ReflectedType.FullName}  {method.Name}  system函数必须是静态函数");
                        if (method.IsGenericMethod)
                            Loger.Error($"{method.ReflectedType.FullName}  {method.Name}  system函数不能是泛型函数");
                        if (method.ReturnType != typeof(void))
                            Loger.Error($"{method.ReflectedType.FullName}  {method.Name}  system函数必须没有返回值");
                        if (ps.Length < 1 || ps.Length > 10)
                            Loger.Error($"{method.ReflectedType.FullName}  {method.Name}  system函数参数个数不正确");
                        for (int k = 0; k < ps.Length; k++)
                        {
                            if (!typeof(SComponent).IsAssignableFrom(ps[k].ParameterType))
                                Loger.Error($"{method.ReflectedType.FullName}  {method.Name}  system函数必须只有组件参数");
                        }
                    }
                }
            }
        }
        [Conditional(ConstDefCore.DebugEnableString)]
        public static void Check(MethodInfo method)
        {
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
            }
        }
    }
}
