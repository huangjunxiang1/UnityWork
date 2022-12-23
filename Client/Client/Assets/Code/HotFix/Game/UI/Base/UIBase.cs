using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using Main;

[DisableAutoRegisteredEvent]
abstract class UIBase : TreeL<UIBase>
{
    Eventer _onDispose;

    public Main.UIConfig uiConfig { get; private set; }
    public abstract string url { get; }

    /// <summary>
    /// dispose 监听
    /// </summary>
    public Eventer OnDispose
    {
        get
        {
            if (_onDispose == null)
                _onDispose = new Eventer(this);
            return _onDispose;
        }
    }

    /// <summary>
    /// UI层级
    /// </summary>
    public virtual int SortOrder { get; set; }

    /// <summary>
    /// UI隐藏
    /// </summary>
    public virtual bool IsShow { get; set; }

    /// <summary>
    /// 异步加载等待
    /// </summary>
    public TaskAwaiter LoadWaiter { get; private set; }

    /// <summary>
    /// Enter异步初始化
    /// </summary>
    public TaskAwaiter EnterWaiter { get; protected set; }

    protected virtual void OnAwake(params object[] data) { }
    protected virtual void OnEnter(params object[] data) { }
    protected virtual TaskAwaiter OnEnterAsync(params object[] data)
    {
        return TaskAwaiter.Completed;
    }
    protected virtual void OnExit() { }
    protected abstract void Binding();

    public virtual void LoadConfig(Main.UIConfig config, params object[] data)
    {
        this.uiConfig = config;
        this.LoadWaiter = TaskAwaiter.Completed;
        this.ListenerEnable = true;
    }
    public virtual async void LoadConfigAsync(Main.UIConfig config, params object[] data)
    {
        this.uiConfig = config;
        this.LoadWaiter = TaskCreater.Create();
        await this.LoadWaiter;
        this.ListenerEnable = true;
    }

    public virtual void Hide()
    {
        this.Hide(false);
    }
    public virtual void Hide(bool playAnimation)
    {
        if (playAnimation) this.IsShow = true;
        else this.IsShow = true;
    }

    public override void Dispose()
    {
        //先从列表移除
        GameL.UI.Remove(this);
        base.Dispose();
        //先执行退出逻辑
        this.OnExit();
        if (_onDispose != null) _onDispose.Call();
    }
}