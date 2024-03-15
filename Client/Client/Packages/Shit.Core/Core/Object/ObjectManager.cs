using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class ObjectManager
    {
        Queue<SObject> newQueue = ObjectPool.Get<Queue<SObject>>();
        Dictionary<long, SObject> gidMap = new();
        Dictionary<long, SObject> rpcMap = new();

        public SObject GetWithRpc(long rpc)
        {
            rpcMap.TryGetValue(rpc, out var obj);
            return obj;
        }
        public SObject GetWithGid(long gid)
        {
            gidMap.TryGetValue(gid, out var obj);
            return obj;
        }
        public bool TryGetWithRpc(long rpc, out SObject obj)
        {
            return rpcMap.TryGetValue(rpc, out obj);
        }
        public bool TryGetWithGid(long gid, out SObject obj)
        {
            return gidMap.TryGetValue(gid, out obj);
        }

        internal void Add(SObject obj)
        {
            newQueue.Enqueue(obj);
            gidMap.Add(obj.gid, obj);
            if (obj.rpc != 0)
            {
                if (!rpcMap.TryGetValue(obj.rpc, out var o))
                    rpcMap.Add(obj.rpc, obj);
                else
                    Loger.Error($"已经创建rpc对象 " + obj.ToString());
            }
        }
        internal void Remove(SObject obj)
        {
            gidMap.Remove(obj.gid);
            if (obj.rpc != 0)
                rpcMap.Remove(obj.rpc);
        }
        internal void Update()
        {
            if (newQueue.Count > 0)
            {
                var queue = newQueue;
                newQueue = ObjectPool.Get<Queue<SObject>>();
                var types = ObjectPool.Get<List<Type>>();
                Type lastType = null;
                while (queue.TryDequeue(out var item))
                {
                    if (item.Disposed) continue;

                    var type = item.GetType();
                    if (lastType != type)
                    {
                        lastType = type;
                        types.Clear();
                        types.Add(type);
                        do
                        {
                            if (type == typeof(SObject)) break;
                            type = type.BaseType;
                            types.Add(type);
                        } while (true);
                    }

                    for (int i = types.Count - 1; i >= 0; i--)
                    {
                        item.world.System.Awake(types[i], item);
                        if (item.Disposed) break;
                    }
                }
                ObjectPool.Return(queue);
                types.Clear();
                ObjectPool.Return(types);
            }
        }
    }
}
