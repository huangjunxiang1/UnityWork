using UnityEngine;
using Game;
using System.Threading;
using Core;
using System;
using System.Reflection;
using YooAsset;

public class GameStart : MonoBehaviour
{
    public bool Debug;
    public CodeRuntime Runtime;
    public EPlayMode playMode;
    public string resUrl;

    public static GameStart Inst { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
        var tsc = ThreadSynchronizationContext.GetOrCreate(System.Environment.CurrentManagedThreadId);
        ThreadSynchronizationContext.SetMainThread(tsc);

        DontDestroyOnLoad(this.gameObject);

        GameObject.FindFirstObjectByType<IngameDebugConsole.DebugLogManager>()?.gameObject.SetActive(Debug && !Application.isEditor);

        new Loading().Enter();
    }
}