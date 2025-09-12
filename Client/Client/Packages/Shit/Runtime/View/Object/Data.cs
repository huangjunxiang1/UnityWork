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
        internal Dictionary<Type, object> _dataMap = new();

        public T Get<T>(bool log = true) where T : class
        {
            if (!_dataMap.TryGetValue(typeof(T), out var value) && log)
                Loger.Error($"不包含数据 type={typeof(T)}");
            return value as T;
        }
        public bool TryGet<T>(out T value) where T : class
        {
            value = default;
            if (!_dataMap.TryGetValue(typeof(T), out var v))
                return false;
            value = (T)v;
            return true;
        }
        public void Add<T>(T o) where T : class
        {
            if (o == null) return;
            _dataMap[o.GetType()] = o;
        }
        public void Remove<T>() where T : class
        {
            _dataMap.Remove(typeof(T));
        }
        public void Clear()
        {
            _dataMap.Clear();
        }
    }
}
