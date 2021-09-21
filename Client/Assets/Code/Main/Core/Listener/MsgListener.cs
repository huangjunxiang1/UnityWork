using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    public class MsgListener
    {
        public bool Enable { get; set; } = true;
        public bool Disposed { get; private set; } = false;

        public void Add(int Cmd, Action callBack)
        {

        }
        public void Remove(int Cmd, Action callBack)
        {

        }
        public void Excute(int Cmd)
        {

        }
        public void Excute(int Cmd, long v)
        {

        }
        public void Excute(int Cmd, object o)
        {

        }
        public void Excute(int Cmd, long v, object o)
        {

        }
        public void Dispose()
        {
            SysEvent.RemoveListener(this);
            Disposed = true;
        }
    }
}
