using UnityEngine;
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
    public string fallBackResUrl;

    public static GameStart Inst { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
        ThreadSynchronizationContext.GetOrCreate(System.Environment.CurrentManagedThreadId);

        DontDestroyOnLoad(this.gameObject);

        GameObject.FindAnyObjectByType<IngameDebugConsole.DebugLogManager>()?.gameObject.SetActive(Debug && !Application.isEditor);

        new Loading().Enter();
    }
}