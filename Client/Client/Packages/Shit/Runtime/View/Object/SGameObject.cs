using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SGameObject : STree
{
    public SGameObject(SGameObjectType type = SGameObjectType.LogicRoot)
    {
        this.KV = this.AddComponent<KVComponent>();
        this.Transform = this.AddComponent<TransformComponent>();
        this.GameObject = this.AddComponent(new GameObjectComponent(type));
        this.Playing = this.AddComponent<PlayingComponent>();
        this.PlayingStep = this.AddComponent<PlayingStepComponent>();
        this.PlayingStep.Enable = false;
    }

    public KVComponent KV { get; private set; }
    public TransformComponent Transform { get; private set; }
    public GameObjectComponent GameObject { get; private set; }
    public PlayingComponent Playing { get; private set; }
    public PlayingStepComponent PlayingStep { get; private set; }
}
