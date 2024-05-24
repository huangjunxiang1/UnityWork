using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Game;

public abstract class UIBase : STree
{
    public UIBase() : base()
    {
        this.EventEnable = false;
        this.TimerEnable = false;
    }

    public UIConfig uiConfig { get; private set; }
    public abstract string url { get; }

    public abstract UIStates uiStates { get; }
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

    public virtual STask LoadConfig(UIConfig config, STask completed, params object[] data)
    {
        this.uiConfig = config;
        this.EventEnable = true;
        this.TimerEnable = true;
        onCompleted = completed;
        return STask.Completed;
    }
    public virtual STask LoadConfigAsync(UIConfig config, STask completed, params object[] data)
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
        var uis = this.GetChildren();
        for (int i = uis.Count - 1; i >= 0; i--)
            ((UIBase)uis[i]).Hide(playAnimation);
    }
    public virtual STask HideAsync(bool playAnimation = true)
    {
        var uis = this.GetChildren();
        for (int i = uis.Count - 1; i >= 0; i--)
            ((UIBase)uis[i]).HideAsync(playAnimation);
        return STask.Completed;
    }
    public virtual void Show(bool playAnimation = true, Action callBack = null)
    {
        var uis = this.GetChildren();
        for (int i = uis.Count - 1; i >= 0; i--)
            ((UIBase)uis[i]).Show(playAnimation);
    }
    public virtual STask ShowAsync(bool playAnimation = true)
    {
        var uis = this.GetChildren();
        for (int i = uis.Count - 1; i >= 0; i--)
            ((UIBase)uis[i]).ShowAsync(playAnimation);
        return STask.Completed;
    }
    public override void Dispose()
    {
        //enter异步正在执行过程中 关闭了UI 则不播放上一个动画的打开
        //先显示上一个UI 这样可以在_onDispose事件里面访问到当前显示的UI
        var arr = this.Parent?.As<STree>().GetChildren();
        if (this.uiStates == UIStates.Success && arr?.LastOrDefault() == this)
        {
            for (int i = arr.Count - 2; i >= 0; i--)
            {
                if (!arr[i].Disposed && (arr[i] is FUI || arr[i] is UUI))
                    ((UIBase)arr[i]).Show();
            }
        }

        base.Dispose();

        //先执行退出逻辑
        if (this.uiStates >= UIStates.OnTask)
            this.OnExit();
    }

    protected virtual void OnAwake(params object[] data) { }//open 的时候立刻调用
    protected virtual STask OnTask(params object[] data)//自定义何时界面可以打开
    {
        return STask.Completed;
    }
    protected virtual void OnEnter(params object[] data) { }//UI加载完毕调用
    protected virtual void OnExit() { }//UI关闭调用
    protected virtual void OnShow() { }//UI每次重显示调用 包括第一次打开
    protected virtual void OnHide() { }//UI每次隐藏时调用 
    protected abstract void Binding();//UI元件绑定

    public sealed override void AcceptedEvent() { base.AcceptedEvent(); }
    public sealed override bool EventEnable { get => base.EventEnable; set => base.EventEnable = value; }
}