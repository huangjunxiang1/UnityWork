using Core;
using Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class SceneManager
    {
        public SceneManager(World world) { this.world = world; }
        World world;

        public Scene Current { get; private set; }

        public async STask InScene<T>(params object[] os) where T : Scene, new()
        {
            var attr = typeof(T).GetCustomAttributes(typeof(SceneAttribute), true).FirstOrDefault() as SceneAttribute;
            if (attr == null)
                throw new Exception($"Scene {typeof(T).FullName} need SceneAttribute");
            if (string.IsNullOrEmpty(attr.name))
                throw new Exception($"Scene {typeof(T).FullName} SceneAttribute name is null or empty");

            var old = Current;
            Current = new T();
            world.Root.AddChild(Current);
            old?.Dispose();
            Client.UI.CloseUI();
            await world.Event.RunEventAsync(new EC_OutScene { });

            SAsset.ReleasePoolsGameObjects();
            await SAsset.ReleaseAllUnuseObjects();
            await SAsset.LoadSceneAsync($"scene_{attr.name}");
            await SValueTask.Delay(100);

            Current._paramObjects = os;
            Current.OnEnter();

            await world.Event.RunEventAsync(new EC_InScene { });
        }
    }
}
