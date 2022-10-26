using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public static class Objects
    {
        static readonly Dictionary<long, object> objMap = new(8192);

        public static void Add(long id, object obj)
        {
#if DebugEnable
            if (objMap.ContainsKey(id))
                Loger.Error("重复添加obj");
#endif
            objMap[id] = obj;
        }
        public static void Remove(long id)
        {
#if DebugEnable
            if (!objMap.ContainsKey(id))
                Loger.Error("未包含obj");
#endif
            objMap.Remove(id);
        }
        public static object Get(long id)
        {
#if DebugEnable
            if (!objMap.ContainsKey(id))
                Loger.Error("未包含obj");
#endif
            return objMap[id];
        }
        public static bool TryGet(long id, out object obj)
        {
            return objMap.TryGetValue(id, out obj);
        }
    }
}
