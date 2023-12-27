using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class SServerData : SObject
    {
        Dictionary<Type, PB.PBMessage> _dataMap = new();

        [Event(-1)]
        void receiveMessage(EC_ReceiveMessage r)
        {
            _dataMap[r.message.GetType()] = r.message;
        }
        public T Get<T>() where T : PB.PBMessage
        {
            if (!_dataMap.TryGetValue(typeof(T), out PB.PBMessage value))
                Loger.Error($"不包含数据 type={typeof(T)}");
            return value as T;
        }
        public void Clear()
        {
            _dataMap.Clear();
        }
    }
}
