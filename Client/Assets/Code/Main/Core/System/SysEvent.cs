using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    public static class SysEvent 
    {
        static List<MsgListener> msgListeners = new List<MsgListener>();
        public static void RigisterListener(MsgListener listener)
        {
            if (MGameSetting.Debug)
            {
                if (msgListeners.Contains(listener))
                {

                }
            }
            msgListeners.Add(listener);
        }
        public static void RemoveListener(MsgListener listener)
        {
            msgListeners.Remove(listener);
        }
        public static void Excute(int Cmd)
        {

        }
        public static void Excute(int Cmd,long v)
        {

        }
        public static void Excute(int Cmd, object o)
        {

        }
    }
}
