using System;
using System.Reflection;
using UnityEngine;

namespace Game
{
    public abstract class SBehavior : STree
    {
        [Event]
        static void _inScene(EC_InScene e)
        {
            int sceneID = e.sceneId;
            int sceneType = e.sceneType;
            var types = Types.GetAllAssignableTypes(typeof(SBehavior));
            int len = types.Length;
            for (int i = 0; i < len; i++)
            {
                var o = types[i].GetCustomAttribute<Create>();
                if (o != null && (o.type == 0 || o.type == sceneType) && (o.ID == 0 || o.ID == sceneID))
                {
                    var s = (SBehavior)Activator.CreateInstance(types[i]);
                    GameM.World.AddChild(s);
                    s.OnCreate();
                }
            }
        }
        protected virtual void OnCreate() { }
    }
}
