using Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public abstract class SSystem : SObject
    {
        static List<SSystem> sys = new List<SSystem>();
        [Event]
        static void _inScene(EC_InScene e)
        {
            int sceneID = e.sceneId;
            int sceneType = (int)e.sceneType;
            var types = Types.GetAllAssignableTypes(typeof(SSystem));
            int len = types.Length;
            for (int i = 0; i < len; i++)
            {
                var o = Types.GetAttribute(types[i], typeof(AutoCreateFormSceneTypeAttribute)) as AutoCreateFormSceneTypeAttribute;
                if (o != null && o.type == sceneType)
                {
                    var s = (SSystem)Activator.CreateInstance(types[i]);
                    sys.Add(s);
                    s.OnCreate();
                }
            }
            for (int i = 0; i < len; i++)
            {
                var o = Types.GetAttribute(types[i], typeof(AutoCreateFormSceneIDAttribute)) as AutoCreateFormSceneIDAttribute;
                if (o != null && o.ID == sceneID)
                {
                    var s = (SSystem)Activator.CreateInstance(types[i]);
                    sys.Add(s);
                    s.OnCreate();
                }
            }
        }
        [Event]
        static void _outScene(EC_OutScene e)
        {
            int sceneID = e.sceneId;
            int sceneType = (int)e.sceneType;
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
