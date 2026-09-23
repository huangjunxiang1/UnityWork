using Core;
using Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SceneManager
{
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
        Game.World.AddChild(Current);
        old?.Dispose();
        Game.UI.CloseUI();
        await Game.Event.RunEventAsync(new EC_OutScene { });

        await Game.Loader.LoadSceneAsync($"scene_{attr.name}");
        await SValueTask.Delay(100);

        Current._paramObjects = os;
        Current.OnEnter();

        await Game.Event.RunEventAsync(new EC_InScene { });
    }
}
