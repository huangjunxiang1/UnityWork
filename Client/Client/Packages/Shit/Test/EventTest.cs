using System;
using System.Collections.Generic;
using System.Linq;
using Core;

// ========== 测试用事件类型 ==========
public class MyEvent { public int Id; }
public class EventA { }
public class EventB { }
public class EC_Event { public string Data; }

// ========== 测试类：继承 Core.SObject ==========
public class EventTestClass : Core.SObject
{
    public static List<string> Log = new List<string>();

    // ---------- 字段测试（无需 EventType） ----------
    [Event(SortOrder = 50, Type = 1)]
    public static MyEvent StaticField_Type1;

    [Event(SortOrder = 60, Type = 2)]
    public MyEvent InstanceField_Type2;

    // ---------- 属性测试（无需 EventType） ----------
    [Event(SortOrder = 70, Type = 3)]
    public static MyEvent StaticProperty_Type3 { get; set; }

    [Event(SortOrder = 80, Type = 4)]
    public MyEvent InstanceProperty_Type4 { get; set; }

    // ---------- 静态方法（未注册，仅占位） ----------
    [Event(EventType = typeof(MyEvent), SortOrder = 0)]
    public static void Static_NoParam_Default() => Log.Add("Static_NoParam_Default");
    [Event(EventType = typeof(MyEvent), SortOrder = 5)]
    public static void Static_NoParam_CustomSort() => Log.Add("Static_NoParam_CustomSort");
    [Event(SortOrder = 1)] // 有参，无 EventType
    public static void Static_Param(MyEvent e) => Log.Add($"Static_Param Id={e.Id}");
    [Event(EventType = typeof(EventA), SortOrder = 1)]
    [Event(EventType = typeof(EventB), SortOrder = 2)]
    public static void Static_MultipleEvents() => Log.Add("Static_MultipleEvents");
    [Event(Parallel = true)] // 有参，合法
    public static void Static_String(string s) => Log.Add($"Static_String '{s}' (并行)");

    // ---------- 委托注册的目标方法（不需要标签） ----------
    public static void DelegateTarget(MyEvent e) => Log.Add($"DelegateTarget Id={e.Id}");

    // ---------- 基本实例方法（无参 + EventType） ----------
    [Event(EventType = typeof(MyEvent), SortOrder = 0, Type = 10)]
    public void Instance_NoParam_Default() => Log.Add("Instance_NoParam_Default");
    [Event(EventType = typeof(MyEvent), SortOrder = 7, Type = 10)]
    public void Instance_NoParam_CustomSort() => Log.Add("Instance_NoParam_CustomSort");

    // ---------- 基本实例方法（有参，无 EventType） ----------
    [Event(SortOrder = 1, Type = 10)]
    public void Instance_Param(MyEvent e) => Log.Add($"Instance_Param Id={e.Id}");
    [Event(Parallel = true, Type = 10)]
    public void Instance_Param_Parallel(MyEvent e) => Log.Add($"Instance_Param_Parallel Id={e.Id} (并行)");
    [Event(Parallel = true, Queue = true, Type = 10)]
    public void Instance_Param_Parallel_Queue(MyEvent e) => Log.Add($"Instance_Param_Parallel_Queue Id={e.Id}");

    // ---------- 其他事件类型（有参，无 EventType） ----------
    [Event(SortOrder = 20, Type = 20)]
    public void OnECEvent(EC_Event e) => Log.Add($"OnECEvent: {e.Data}");

    // ---------- EventHandler 中断测试 (Type=30, 有参+无 EventType) ----------
    [Event(Type = 30, SortOrder = 5)]
    public static void BeforeBreak(MyEvent e, EventHandler handler) { handler.SetValue(123); Log.Add("BeforeBreak called, value set to 123"); }
    [Event(Type = 30, SortOrder = 10)]
    public static void BreakHandler(MyEvent e, EventHandler handler) { handler.BreakEvent(); handler.SetValue(999); Log.Add("BreakHandler called, value set to 999"); }
    [Event(Type = 30, SortOrder = 20)]
    public void AfterBreak(MyEvent e) => Log.Add("AfterBreak should not be called");

    // ---------- ActorId/gid 过滤 (Type=40, 有参) ----------
    [Event(Type = 40, SortOrder = 90)]
    public void OnActorIdEvent(MyEvent e) => Log.Add($"ActorId_{ActorId}");
    [Event(Type = 40, SortOrder = 91)]
    public void OnGidEvent(MyEvent e) => Log.Add($"Gid_{gid}");

    // ---------- 组合测试：有参方法覆盖 SortOrder, Type, Queue, Parallel ----------
    [Event(SortOrder = 100, Type = 50)]
    public void Combo_SortOnly(MyEvent e) => Log.Add("Combo_SortOnly");

    [Event(SortOrder = 101, Type = 50, Queue = true)]
    public void Combo_Sort_Queue(MyEvent e) => Log.Add("Combo_Sort_Queue");

    [Event(SortOrder = 102, Type = 50, Parallel = true)]
    public void Combo_Sort_Parallel(MyEvent e) => Log.Add("Combo_Sort_Parallel");

    [Event(SortOrder = 103, Type = 50, Parallel = true, Queue = true)]
    public void Combo_Sort_Parallel_Queue(MyEvent e) => Log.Add("Combo_Sort_Parallel_Queue");

    [Event(Type = 50, Parallel = true)]
    public void Combo_ParallelOnly(MyEvent e) => Log.Add("Combo_ParallelOnly");

    [Event(Type = 50, Queue = true)]
    public void Combo_QueueOnly(MyEvent e) => Log.Add("Combo_QueueOnly");

    [Event(Type = 50, Parallel = true, Queue = true)]
    public void Combo_Parallel_QueueOnly(MyEvent e) => Log.Add("Combo_Parallel_QueueOnly");

    [Event(Type = 50)]
    public void Combo_Default(MyEvent e) => Log.Add("Combo_Default");

    // ---------- 无参方法组合 (仅含 SortOrder, Type, Queue，不含 Parallel) ----------
    [Event(EventType = typeof(MyEvent), SortOrder = 200, Type = 60)]
    public void NoParam_SortOnly() => Log.Add("NoParam_SortOnly");
    [Event(EventType = typeof(MyEvent), SortOrder = 201, Type = 60, Queue = true)]
    public void NoParam_Sort_Queue() => Log.Add("NoParam_Sort_Queue");
}

// ---------- 基类 ----------
public class BaseEventClass : Core.SObject
{
    [Event(SortOrder = 0, Type = 10)]
    public virtual void BaseMethod(MyEvent e) => EventTestClass.Log.Add("BaseMethod");
}
public class DerivedEventClass : BaseEventClass { }

// ---------- 辅助类 ----------
public class ManualHandlerClass : Core.SObject
{
    [Event(SortOrder = 30, Type = 10)]
    public void OnMyEvent(MyEvent e) => EventTestClass.Log.Add($"Manual_InstanceMethod Id={e.Id}");
}

internal static class EventTest
{
    public static void Test()
    {
        EventTestClass.Log.Clear();

        // 重置字段/属性
        EventTestClass.StaticField_Type1 = null;
        EventTestClass.StaticProperty_Type3 = null;
        var testObj = new EventTestClass();
        testObj.InstanceField_Type2 = null;
        testObj.InstanceProperty_Type4 = null;

        // ===== 注册阶段 =====
        Game.World.AddChild(testObj);
        Game.World.AddChild(new DerivedEventClass());
        Game.World.AddChild(new ManualHandlerClass());

        // 委托注册（默认 Type=0）
        Action<MyEvent> act = EventTestClass.DelegateTarget;
        Delegate act1 = (Action<MyEvent>)EventTestClass.DelegateTarget;
        Game.Event.RigisteEvent<MyEvent>(act, 0, 40);
        Game.Event.RigisteEvent(act1, 70, 41);

        // 用于 ActorId/gid 测试的对象
        var obj1 = new EventTestClass { ActorId = 1001 };
        var obj2 = new EventTestClass { ActorId = 2002 };
        Game.World.AddChild(obj1);
        Game.World.AddChild(obj2);

        // ===== 触发事件 =====
        // 1. 触发字段/属性对应的 Type (1,2,3,4)
        Game.Event.RunEvent(new MyEvent { Id = 100 }, type: 1);
        Game.Event.RunEvent(new MyEvent { Id = 100 }, type: 2);
        Game.Event.RunEvent(new MyEvent { Id = 100 }, type: 3);
        Game.Event.RunEvent(new MyEvent { Id = 100 }, type: 4);

        // 2. Type=10 测试（基础排序、并行、队列）
        Game.Event.RunEvent(new MyEvent { Id = 100 }, type: 10);
        Game.Event.RunEvent(new EventA());   // 触发静态多事件（无Type，默认0）
        Game.Event.RunEvent(new EventB());
        Game.Event.RunEvent("hello");        // 触发静态字符串
        Game.Event.RunEvent(new EC_Event { Data = "manual" }, type: 20); // EC_Event，Type=20

        // 3. Type=30 中断测试
        Game.Event.RunEvent(new MyEvent { Id = 200 }, type: 30);

        // 4. Type=40 ActorId/gid 过滤
        Game.Event.RunEvent(new MyEvent { Id = 300 }, actorId: 1001, type: 40);
        Game.Event.RunEvent(new MyEvent { Id = 301 }, gid: obj1.gid, type: 40);
        Game.Event.RunEvent(new MyEvent { Id = 302 }, type: 40);

        // 5. Type=50 组合测试（有参方法的各种属性排列）
        Game.Event.RunEvent(new MyEvent { Id = 400 }, type: 50);

        // 6. Type=60 无参方法组合测试（无 Parallel）
        Game.Event.RunEvent(new MyEvent { Id = 500 }, type: 60);

        // 7. 额外触发 Type=0 默认事件（用于验证委托和静态方法）
        Game.Event.RunEvent(new MyEvent { Id = 600 });
        Game.Event.RunEvent(new MyEvent { Id = 700 }, actorId: 70);

        Game.Event.RemoveEvent(act);
        Game.Event.RemoveEvent(act1);

        // ===== 验证 =====
        Verify(testObj, obj1, obj2);

        Console.WriteLine("✅ 所有测试通过！");
    }

    private static void Verify(EventTestClass instance, EventTestClass obj1, EventTestClass obj2)
    {
        var log = EventTestClass.Log;

        // ---------- 验证 Type=10 顺序 ----------
        var type10Logs = log.Where(x => x.Contains("Instance_") || x == "BaseMethod" || x == "Manual_InstanceMethod Id=100").ToList();
        string[] expectedOrderType10 = {
            "Instance_NoParam_Default",          // SortOrder 0
            "BaseMethod",                        // SortOrder 0
            "Instance_Param Id=100",             // SortOrder 1
            "Instance_NoParam_CustomSort",       // SortOrder 7
            "Manual_InstanceMethod Id=100"       // SortOrder 30
        };
        int pos = 0;
        foreach (var exp in expectedOrderType10)
        {
            string prefix = exp.Split(' ')[0];
            int found = type10Logs.FindIndex(pos, x => x.StartsWith(prefix));
            if (found == -1)
                throw new Exception($"Type10 顺序错误: 未找到 '{exp}'");
            pos = found + 1;
        }

        // ---------- 验证委托注册（在 Type=0 事件中调用） ----------
        if (!log.Any(x => x == "DelegateTarget Id=600"))
            throw new Exception("委托注册未在 Type=0 事件中被调用");
        if (!log.Any(x => x == "DelegateTarget Id=700"))
            throw new Exception("委托注册未在 Type=70 事件中被调用");

        // ---------- 字段/属性赋值验证 (Type=1,2,3,4) ----------
        if (EventTestClass.StaticField_Type1 == null || EventTestClass.StaticField_Type1.Id != 100)
            throw new Exception("StaticField_Type1 错误");
        if (instance.InstanceField_Type2 == null || instance.InstanceField_Type2.Id != 100)
            throw new Exception("InstanceField_Type2 错误");
        if (EventTestClass.StaticProperty_Type3 == null || EventTestClass.StaticProperty_Type3.Id != 100)
            throw new Exception("StaticProperty_Type3 错误");
        if (instance.InstanceProperty_Type4 == null || instance.InstanceProperty_Type4.Id != 100)
            throw new Exception("InstanceProperty_Type4 错误");

        // ---------- EC_Event 测试 ----------
        if (!log.Any(x => x == "OnECEvent: manual"))
            throw new Exception("OnECEvent 未调用");

        // ---------- EventHandler 中断 (Type=30) ----------
        if (!log.Any(x => x.Contains("BeforeBreak called")))
            throw new Exception("BeforeBreak 未调用");
        if (!log.Any(x => x.Contains("BreakHandler called")))
            throw new Exception("BreakHandler 未调用");
        if (log.Any(x => x == "AfterBreak should not be called"))
            throw new Exception("中断失败");

        // ---------- ActorId/gid 过滤 (Type=40) ----------
        var actorLogs = log.Where(x => x.StartsWith("ActorId_")).ToList();
        if (actorLogs.Count != 5) // 1次actorId + 1次gid + 3次通用 = 5
            throw new Exception($"ActorId 日志数量错误: 期望 5，实际 {actorLogs.Count}");
        if (actorLogs.Count(x => x == "ActorId_1001") != 3)
            throw new Exception("ActorId_1001 出现次数错误（应为3）");
        if (actorLogs.Count(x => x == "ActorId_2002") != 1)
            throw new Exception("ActorId_2002 出现次数错误（应为1）");
        var gidLogs = log.Where(x => x.StartsWith("Gid_")).ToList();
        if (gidLogs.Count != 5)
            throw new Exception($"Gid 日志数量错误: 期望 5，实际 {gidLogs.Count}");
        string gid1 = $"Gid_{obj1.gid}";
        string gid2 = $"Gid_{obj2.gid}";
        if (gidLogs.Count(x => x == gid1) != 3)
            throw new Exception($"Gid_{obj1.gid} 出现次数错误（应为3）");
        if (gidLogs.Count(x => x == gid2) != 1)
            throw new Exception($"Gid_{obj2.gid} 出现次数错误（应为1）");

        // ---------- Type=50 组合测试（有参方法） ----------
        string[] expectedCombo50 = {
            "Combo_Default",                // SortOrder 0
            "Combo_SortOnly",               // SortOrder 100
            "Combo_Sort_Queue",             // 101
            "Combo_Sort_Parallel",          // 102
            "Combo_Sort_Parallel_Queue",    // 103
            "Combo_ParallelOnly",           // 默认0
            "Combo_QueueOnly",              // 0
            "Combo_Parallel_QueueOnly"      // 0
        };
        foreach (var exp in expectedCombo50)
            if (!log.Any(x => x == exp))
                throw new Exception($"Type50 组合方法 '{exp}' 未调用");

        // ---------- Type=60 无参组合测试（无 Parallel） ----------
        string[] expectedCombo60 = {
            "NoParam_SortOnly",
            "NoParam_Sort_Queue"
        };
        foreach (var exp in expectedCombo60)
            if (!log.Any(x => x == exp))
                throw new Exception($"Type60 无参方法 '{exp}' 未调用");

        // ---------- 静态多事件验证 ----------
        if (!log.Any(x => x == "Static_MultipleEvents"))
            throw new Exception("Static_MultipleEvents 未调用");
    }
}