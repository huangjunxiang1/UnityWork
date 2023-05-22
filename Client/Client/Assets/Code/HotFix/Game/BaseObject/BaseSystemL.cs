using Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    abstract class BaseSystemL : ObjectL
    {
        static List<BaseSystemL> sys = new List<BaseSystemL>();
        [Event((int)EventIDM.InScene)]
        static void _inScene(EventerContent e)
        {
            int sceneID = e.Value;
            int sceneType = (int)e.Data;
            var types = Types.GetAllAssignableTypes(typeof(BaseSystemL));
            int len = types.Length;
            for (int i = 0; i < len; i++)
            {
                var o = Types.GetAttribute(types[i], typeof(AutoCreateFormSceneTypeAttribute)) as AutoCreateFormSceneTypeAttribute;
                if (o != null && o.type == sceneType)
                {
                    var s = (BaseSystemL)Activator.CreateInstance(types[i]);
                    sys.Add(s);
                    s.OnCreate();
                }
            }
            for (int i = 0; i < len; i++)
            {
                var o = Types.GetAttribute(types[i], typeof(AutoCreateFormSceneIDAttribute)) as AutoCreateFormSceneIDAttribute;
                if (o != null && o.ID == sceneID)
                {
                    var s = (BaseSystemL)Activator.CreateInstance(types[i]);
                    sys.Add(s);
                    s.OnCreate();
                }
            }
        }
        [Event((int)EventIDM.OutScene)]
        static void _outScene(EventerContent e)
        {
            int sceneID = e.Value;
            int sceneType = (int)e.Data;
            int len = sys.Count;
            for (int i = 0; i < len; i++)
            {
                var o = Types.GetAttribute(sys[i].GetType(), typeof(AutoCreateFormSceneTypeAttribute)) as AutoCreateFormSceneTypeAttribute;
                if (o != null && o.type == sceneType)
                {
                    var s = sys[i];
                    sys.RemoveAt(i);
                    i--;
                    len--;
                    s.Dispose();
                }
            }
            for (int i = 0; i < len; i++)
            {
                var o = Types.GetAttribute(sys[i].GetType(), typeof(AutoCreateFormSceneIDAttribute)) as AutoCreateFormSceneIDAttribute;
                if (o != null && o.ID == sceneID)
                {
                    var s = sys[i];
                    sys.RemoveAt(i);
                    i--;
                    len--;
                    s.Dispose();
                }
            }
        }
        protected virtual void OnCreate() { }
    }
}
