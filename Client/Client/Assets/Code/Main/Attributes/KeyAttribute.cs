using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class KeyAttribute : Attribute
    {
        public uint Key { get; }
        public KeyAttribute(uint key)
        {
            this.Key = key;
        }
    }
}
