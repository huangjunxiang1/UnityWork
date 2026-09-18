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
        [Conditional(SSetting.CoreSetting.DebugEnableString)]
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

                // ===== 检查字段（新增） =====
                var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                foreach (var field in fields)
                {
                    if (field.GetCustomAttributes<EventAttribute>(false).Any())
                        CheckFieldOrProperty(field);
                }

                // ===== 检查属性（新增） =====
                var props = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                foreach (var prop in props)
                {
                    if (prop.GetCustomAttributes<EventAttribute>(false).Any())
                        CheckFieldOrProperty(prop);
                }
            }
        }
        [Conditional(SSetting.CoreSetting.DebugEnableString)]
        public static void Check(MethodInfo method)
        {
            var origin = method.GetParameters();
            var ps = origin;
            if (origin.Length > 1 && origin.LastOrDefault()?.ParameterType == typeof(EventHandler))
                ps = origin[..1];

            if (method.IsGenericMethod)
                Loger.Error($"事件函数不能是泛型函数  class:{method.ReflectedType.FullName} method:{method.Name}");
            if (method.ReturnType != typeof(void) && method.ReturnType != typeof(STask))
                Loger.Error($"事件函数的返回类型只能是void或者{nameof(STask)} class:{method.ReflectedType.FullName} method:{method.Name}");

            var ea = method.GetCustomAttributes<EventAttribute>(false).ToArray();
            for (int i = 0; i < ea.Length; i++)
            {
                if (ea[i].EventType != null && ea[i].EventType.IsPrimitive)
                    Loger.Error($"不要使用系统值类型作为事件参数类型  class:{method.ReflectedType.FullName} method:{method.Name}");
            }
            if (ps.Length > 0 && ps[0].ParameterType.IsPrimitive)
                Loger.Error($"不要使用系统值类型作为事件参数类型  class:{method.ReflectedType.FullName} method:{method.Name}");
            if (ea.FirstOrDefault(t => t.Parallel) != null)
            {
                if (origin.Length != 1 || origin[0].ParameterType == typeof(EventHandler))
                    Loger.Error($"多线程事件只能一个参数  class:{method.ReflectedType.FullName} method:{method.Name}");
            }

            //动态注册的函数
            if (ea.Length == 0)
            {
                if (ps.Length == 1 && ps[0].ParameterType != typeof(EventHandler))
                    return;
            }
            else
            {
                if (ps.Length == 0 && ea.FirstOrDefault(t => t.EventType == null) == null)
                    return;
                if (ps.Length == 1 && ps[0].ParameterType != typeof(EventHandler) && ea.Length == 1 && ea[0].EventType == null)
                    return;
            }

            Loger.Error($"事件定义错误 class:{method.ReflectedType.FullName} method:{method.Name}");
        }
        // ===== 新增：字段/属性检查 =====
        [Conditional(SSetting.CoreSetting.DebugEnableString)]
        private static void CheckFieldOrProperty(MemberInfo member)
        {
            Type memberType = null;
            bool isProperty = member is PropertyInfo;

            if (isProperty)
            {
                var prop = (PropertyInfo)member;
                if (!prop.CanWrite)
                    Loger.Error($"事件属性必须可写 class:{member.ReflectedType.FullName} member:{member.Name}");
                memberType = prop.PropertyType;
            }
            else
            {
                var field = (FieldInfo)member;
                memberType = field.FieldType;
            }

            // 类型必须为 class（非值类型）
            if (memberType.IsValueType)
                Loger.Error($"事件字段/属性不能是值类型 class:{member.ReflectedType.FullName} member:{member.Name}");

            // 不能是开放泛型
            if (memberType.ContainsGenericParameters)
                Loger.Error($"事件字段/属性不能是开放泛型类型 class:{member.ReflectedType.FullName} member:{member.Name}");

            // 检查特性配置
            var attrs = member.GetCustomAttributes<EventAttribute>(false).ToArray();
            foreach (var attr in attrs)
            {
                // 字段/属性不允许指定 EventType（类型由成员自身推断）
                if (attr.EventType != null)
                    Loger.Error($"字段/属性不需要指定 EventType class:{member.ReflectedType.FullName} member:{member.Name}");

                // 字段/属性不支持并行
                if (attr.Parallel)
                    Loger.Error($"字段/属性不支持 Parallel class:{member.ReflectedType.FullName} member:{member.Name}");

                // Queue 和 Type 可以存在（但不强制检查），但若用户误用可酌情警告
                // 这里只针对明确冲突的项报错
            }
        }
    }
}
