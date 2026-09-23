using Core;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Scene : STree
{
    public Scene()
    {
        root = new(this.GetType().Name);
        root.transform.parent = Game.World.transform;
        Loader = new(this.GetType().Name, null);
        this.SetPackage();
    }

    internal object[] _paramObjects;

    public GameObject root { get; private set; }
    public SLoader Loader { get; private set; }

    public virtual void OnEnter() { }
    protected virtual void SetPackage() { }

    public T GetParam<T>(int index)
    {
        if (_paramObjects != null && index < _paramObjects.Length)
            return (T)_paramObjects[index];
        return default;
    }
    public override void Dispose()
    {
        base.Dispose();
        Loader.Dispose();
        GameObject.Destroy(root);
    }
}
