using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
using Main;

[DisableAutoRegisteredEvent]
[DisableAutoRegisteredRPCEvent]
[DisableAutoRegisteredTimer]
abstract class UIBase : STree<UIBase>
{
    SEventListener _onDispose;

    public Main.SUIConfig uiConfig { get; private set; }
    public abstract string url { get; }


    public abstract UIStates uiStates { get; }
    /// <summary>
    /// dispose 监听
    /// </summary>
    public SEventListener onDispose => _onDispose ??= new SEventListener(this);
    /// <summary>
    /// UI层级
    /// </summary>
    public virtual int sortOrder { get; set; }
    /// <summary>
    /// UI隐藏
    /// </summary>
    public virtual bool isShow { get; set; }
    /// <summary>
    /// 自定义界面是否已经可以打开
    /// </summary>
    public virtual STask onTask { get; }
    /// <summary>
    /// 加载中异步
    /// </summary>
    public STask onCompleted { get; private set; }

    public virtual STask LoadConfig(Main.SUIConfig config, STask completed, params object[] data)
    {
        this.uiConfig = config;
        this.ListenerEnable = true;
        STimer.AutoRigisterTimer(this);
        onCompleted = completed;
        return STask.Completed;
    }
    public virtual STask LoadConfigAsync(Main.SUIConfig config, STask completed, params object[] data)
    {
        this.uiConfig = config;
        onCompleted = completed;
        return STask.Completed;
    }
    public T GetSubUI<T>() where T : UIBase
    {
        return this.GetChildren().Find(t => t is T) as T;
    }
    public virtual void Hide(bool playAnimation = true, Action callBack = null)
    {
        List<UIBase> uis = this.GetChildren();
        for (int i = uis.Count - 1; i >= 0; i--)
            uis[i].Hide(playAnimation);
    }
    public virtual STask HideAsync(bool playAnimation = true)
    {
        List<UIBase> uis = this.GetChildren();
        for (int i = uis.Count - 1; i >= 0; i--)
            uis[i].HideAsync(playAnimation);
        return STask.Completed;
    }
    public virtual void Show(bool playAnimation = true, Action callBack = null)
    {
        List<UIBase> uis = this.GetChildren();
        for (int i = uis.Count - 1; i >= 0; i--)
            uis[i].Show(playAnimation);
    }
    public virtual STask ShowAsync(bool playAnimation = true)
    {
        List<UIBase> uis = this.GetChildren();
        for (int i = uis.Count - 1; i >= 0; i--)
            uis[i].ShowAsync(playAnimation);
        return STask.Completed;
    }
    public override void Dispose()
    {  
        //先从列表移除
        SGameL.UI.Remove(this);
        STimer.AutoRemoveTimer(this);
        base.Dispose();
        //enter异步正在执行过程中 关闭了UI 则不播放上一个动画的打开
        //先显示上一个UI 这样可以在_onDispose事件里面访问到当前显示的UI
        if (this.uiStates == UIStates.Success)
            SGameL.UI.ShowLastUI();
        //先执行退出逻辑
        if (this.uiStates >= UIStates.OnTask)
            this.OnExit();
        _onDispose?.Call();
    }

    protected virtual void OnAwake(params object[] data) { }//open 的时候立刻调用
    protected virtual STask OnTask(params object[] data)//自定义何时界面可以打开
    {
        return STask.Completed;
    }
    protected virtual void VMBinding() { }//显示绑定
    protected virtual void OnEnter(params object[] data) { }//UI加载完毕调用
    protected virtual void OnExit() { }//UI关闭调用
    protected virtual void OnShow() { }//UI每次重显示调用 包括第一次打开
    protected virtual void OnHide() { }//UI每次隐藏时调用 
    protected abstract void Binding();//UI元件绑定

}