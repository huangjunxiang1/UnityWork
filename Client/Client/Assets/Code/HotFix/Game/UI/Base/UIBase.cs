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
    /// isPage=true 是页签UI  isPage=false 是页签UI的弹窗
    /// </summary>
    public bool IsPage => this.Parent == null;

    /// <summary>
    /// 异步加载等待
    /// </summary>
    public TaskAwaiter LoadWaiter { get; private set; }

    /// <summary>
    /// Enter异步初始化
    /// </summary>
    public TaskAwaiter EnterWaiter { get; protected set; }

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

    public abstract void Hide(bool playAnimation = true, Action callBack = null);
    public abstract TaskAwaiter HideAsync(bool playAnimation = true);
    public abstract void Show(bool playAnimation = true, Action callBack = null);
    public abstract TaskAwaiter ShowAsync(bool playAnimation = true);

    public override void Dispose()
    {  
        //先从列表移除
        GameL.UI.Remove(this);
        base.Dispose();
        //先执行退出逻辑
        this.OnExit();
        //先显示上一个UI 这样可以在_onDispose事件里面访问到当前显示的UI
        GameL.UI.ShowLastPageUI();
        if (_onDispose != null) _onDispose.Call();
    }

    protected virtual void OnAwake(params object[] data) { }//open 的时候立刻调用
    protected virtual void OnEnter(params object[] data) { }//UI加载完毕调用
    protected virtual TaskAwaiter OnEnterAsync(params object[] data)
    {
        return TaskAwaiter.Completed;
    }//UI加载完毕调用
    protected virtual void OnExit() { }//UI关闭调用
    protected virtual void OnShow() { }//UI每次重显示调用 包括第一次打开
    protected virtual void OnHide() { }//UI每次隐藏时调用 
    protected abstract void Binding();//UI元件绑定

}