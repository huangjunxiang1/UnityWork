using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class DataComponent<T> : SComponent
    {
        T _value;
        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                this.SetChangeFlag();
            }
        }
    }
}
