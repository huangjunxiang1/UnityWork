using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using Main;

abstract class UIBase : EntityL
{
    Eventer _onDispose;

    public UIConfig uiConfig { get; private set; }

    /// <summary>
    /// dispose 监听
    /// </summary>
    public Eventer OnDispose
    {
        get
        {
            if (_onDispose == null) _onDispose = new Eventer(this);
            return _onDispose;
        }
    }

    /// <summary>
    /// 如果是异步加载 则await这个
    /// </summary>
    public TaskAwaiter LoadWaiter { get; private set; }

    /// <summary>
    /// UI层级
    /// </summary>
    public virtual int SortOrder
    {
        get; 
        set;
    }

    /// <summary>
    /// UI隐藏
    /// </summary>
    public virtual bool isHide
    {
        get;
        set;
    }

    protected abstract void OnEnter(params object[] data);
    protected abstract void OnExit();
    protected abstract void Binding();

    public virtual void LoadConfig(UIConfig config, params object[] data)
    {
        this.LoadWaiter = TaskAwaiter.Completed;
        this.uiConfig = config;
    }
    public virtual void LoadConfigAsync(UIConfig config, params object[] data)
    {
        this.LoadWaiter = TaskCreater.Create();
        this.uiConfig = config;
    }

    public virtual void Hide()
    {
        this.Hide(false);
    }
    public virtual void Hide(bool playAnimation)
    {
        if (playAnimation) this.isHide = true;
        else this.isHide = true;
    }

    public override void Dispose()
    {
        base.Dispose();
        UIS.Remove(this);
        if (_onDispose != null) _onDispose.Call();
    }
}