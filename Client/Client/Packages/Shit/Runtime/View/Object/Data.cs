using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Data
    {
        internal Data(World world)
        {
            world.Event.getEvent += Add;
        }

        internal Dictionary<Type, IData> _dataMap = new();

        public T Get<T>() where T : class
        {
            if (!_dataMap.TryGetValue(typeof(T), out var value))
                Loger.Error($"不包含数据 type={typeof(T)}");
            return value as T;
        }
        public bool TryGet<T>(out T value) where T : IData
        {
            value = default;
            if (!_dataMap.TryGetValue(typeof(T), out var v))
                return false;
            value = (T)v;
            return true;
        }
        public void Add(object o)
        {
            if (o is not IData) return;
            _dataMap[o.GetType()] = (IData)o;
        }
        public void Remove<T>() where T : IData
        {
            _dataMap.Remove(typeof(T));
        }
        public void Clear()
        {
            _dataMap.Clear();
        }
    }
}
