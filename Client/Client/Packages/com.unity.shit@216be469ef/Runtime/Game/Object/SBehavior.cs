using Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public abstract class SBehavior : SObject
    {
        static List<SBehavior> sys = new List<SBehavior>();
        [Event]
        static void _inScene(EC_InScene e)
        {
            int sceneID = e.sceneId;
            int sceneType = (int)e.sceneType;
            var types = Types.GetAllAssignableTypes(typeof(SBehavior));
            int len = types.Length;
            for (int i = 0; i < len; i++)
            {
                var o = types[i].GetCustomAttribute<AutoCreateFormSceneTypeAttribute>();
                if (o != null && o.type == sceneType)
                {
                    var s = (SBehavior)Activator.CreateInstance(types[i]);
                    sys.Add(s);
                    s.OnCreate();
                }
            }
            for (int i = 0; i < len; i++)
            {
                var o = types[i].GetCustomAttribute<AutoCreateFormSceneIDAttribute>();
                if (o != null && o.ID == sceneID)
                {
                    var s = (SBehavior)Activator.CreateInstance(types[i]);
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
                var o = sys[i].GetType().GetCustomAttribute<AutoCreateFormSceneTypeAttribute>();
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
                var o = sys[i].GetType().GetCustomAttribute<AutoCreateFormSceneIDAttribute>();
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
