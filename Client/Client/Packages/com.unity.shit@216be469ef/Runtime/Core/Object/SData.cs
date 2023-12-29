using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class SData : SObject
    {
        Dictionary<Type, object> _dataMap = new();

        [Event(-1)]
        void receiveMessage(EC_ReceiveMessage r)
        {
            _dataMap[r.message.GetType()] = r.message;
        }
        public T Get<T>() where T : class
        {
            if (!_dataMap.TryGetValue(typeof(T), out object value))
                Loger.Error($"不包含数据 type={typeof(T)}");
            return value as T;
        }
        public void Add(object o)
        {
            if (o == null) return;
            _dataMap[o.GetType()] = o;
        }
        public void Remove<T>()
        {
            _dataMap.Remove(typeof(T));
        }
        public void Clear()
        {
            _dataMap.Clear();
        }
    }
}
