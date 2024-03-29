using Core;
using Event;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Game
{
    public abstract class SBehavior : SObject
    {
        static SBehavior()
        {
            var lst = ObjectPool.Get<List<Type>>();
            for (int i = 0; i < Client.World.Types.types.Count; i++)
            {
                if (typeof(SBehavior).IsAssignableFrom(Client.World.Types.types[i]) && Client.World.Types.types[i] != typeof(SBehavior))
                    lst.Add(Client.World.Types.types[i]);
            }
            behaviors = lst.Count == 0 ? Array.Empty<Type>() : lst.ToArray();
        }

        static Type[] behaviors;

        [Event]
        static void _inScene(EC_InScene e)
        {
            int sceneID = e.sceneId;
            int sceneType = e.sceneType;
            int len = behaviors.Length;
            for (int i = 0; i < len; i++)
            {
                var o = behaviors[i].GetCustomAttribute<Create>();
                if (o != null && (o.type == 0 || o.type == sceneType) && (o.ID == 0 || o.ID == sceneID))
                {
                    var s = (SBehavior)Activator.CreateInstance(behaviors[i]); 
                    Client.Scene.AddChild(s);
                    s.OnCreate();
                }
            }
        }
        protected virtual void OnCreate() { }
    }
}
