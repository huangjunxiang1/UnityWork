using Core;
using Event;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Game
{
    public class Scene : STree
    {
        static Scene()
        {
            var lst = ObjectPool.Get<List<Type>>();
            for (int i = 0; i < Client.World.Types.types.Count; i++)
            {
                if (typeof(Scene).IsAssignableFrom(Client.World.Types.types[i]) && Client.World.Types.types[i] != typeof(Scene))
                    lst.Add(Client.World.Types.types[i]);
            }
            scenes = lst.Count == 0 ? Array.Empty<Type>() : lst.ToArray();
        }

        static Type[] scenes;

        [Event]
        static void _inScene(EC_InScene e)
        {
            int sceneID = e.sceneId;
            int sceneType = e.sceneType;
            int len = scenes.Length;
            bool isCreate = false;
            for (int i = 0; i < len; i++)
            {
                var o = scenes[i].GetCustomAttribute<SceneAttribute>();
                if (o != null && (o.type == 0 || o.type == sceneType) && (o.ID == 0 || o.ID == sceneID))
                {
                    isCreate = true;
                    var s = (Scene)Activator.CreateInstance(scenes[i]);
                    Client.Scene.AddChild(s);
                    s.OnCreate(Client.Scene.objects);
                }
            }
            if (!isCreate)
            {
                var s = new Scene();
                Client.Scene.AddChild(s);
                s.OnCreate(Client.Scene.objects);
            }
        }
        protected virtual void OnCreate(params object[] os) { }
    }
}
