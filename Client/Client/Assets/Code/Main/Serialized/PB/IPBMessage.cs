using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB
{
    public interface IPBMessage
    {
        void Write(PBWriter writer);
        void Read(PBReader reader);
    }
}
