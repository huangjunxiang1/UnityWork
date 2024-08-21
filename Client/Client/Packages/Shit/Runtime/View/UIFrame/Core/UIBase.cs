using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Core;
using Game;

public abstract class UIBase : STree
{
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
        this.Parent.GetChildren().Sort((x, y) =>
        {
            int vx = (x is UIBase) ? ((UIBase)x).uiConfig.SortOrder : 0;
            int vy = (y is UIBase) ? ((UIBase)y).uiConfig.SortOrder : 0;
            return vx - vy;
        });
        onCompleted = completed;
        return STask.Completed;
    }
    public virtual STask LoadConfigAsync(UIConfig config, STask completed, params object[] data)
    {
        this.uiConfig = config;
        this.Parent.GetChildren().Sort((x, y) =>
        {
            int vx = (x is UIBase) ? ((UIBase)x).uiConfig.SortOrder : 0;
            int vy = (y is UIBase) ? ((UIBase)y).uiConfig.SortOrder : 0;
            return vx - vy;
        });
        onCompleted = completed;
        return STask.Completed;
    }
    public T Open<T>(params object[] data) where T : UIBase, new()
    {
        T ui = this.GetChild<T>();
        if (ui != null)
            return ui;

        UIConfig cfg = typeof(T).GetCustomAttribute<UIConfig>() ?? UIConfig.Default;

        ui = new();
        this.AddChild(ui);
        ui.LoadConfig(cfg, new STask<T>(), data).AddEvent(() =>
        {
            ui.Show();
            ui.onCompleted.TrySetResult(ui);
        });

        return ui;
    }
    public async STask<T> OpenAsync<T>(params object[] data) where T : UIBase, new()
    {
        T ui = this.GetChild<T>();
        if (ui != null)
        {
            await ui.onCompleted;
            return ui;
        }
        using (await STaskLocker.Lock(this))
        {
            UIConfig cfg = typeof(T).GetCustomAttribute<UIConfig>() ?? UIConfig.Default;

            InputHelper.EnableUIInput(false);
            ui = new();
            //在执行异步的过程中有可能会关闭这个UI
            ui.onDispose.Add(() =>
            {
                if (ui.uiStates < UIStates.Success)
                    InputHelper.EnableUIInput(true);
            });
            this.AddChild(ui);
            await ui.LoadConfigAsync(cfg, new STask<T>(), data);
            if (ui.Disposed)
                return ui;

            ui.Show();
            InputHelper.EnableUIInput(true);

            ui.onCompleted.TrySetResult(ui);
            return ui;
        }
    }
    public virtual void Hide(bool playAnimation = true, Action callBack = null)
    {
        var uis = this.GetChildren();
        for (int i = uis.Count - 1; i >= 0; i--)
        {
            if (uis[i] is UIBase)
                ((UIBase)uis[i]).Hide(playAnimation);
        }
    }
    public virtual STask HideAsync(bool playAnimation = true)
    {
        var uis = this.GetChildren();
        for (int i = uis.Count - 1; i >= 0; i--)
        {
            if (uis[i] is UIBase)
                ((UIBase)uis[i]).HideAsync(playAnimation);
        }
        return STask.Completed;
    }
    public virtual void Show(bool playAnimation = true, Action callBack = null)
    {
        var uis = this.GetChildren();
        for (int i = uis.Count - 1; i >= 0; i--)
        {
            if (uis[i] is UIBase)
                ((UIBase)uis[i]).Show(playAnimation);
        }
    }
    public virtual STask ShowAsync(bool playAnimation = true)
    {
        var uis = this.GetChildren();
        for (int i = uis.Count - 1; i >= 0; i--)
        {
            if (uis[i] is UIBase)
                ((UIBase)uis[i]).ShowAsync(playAnimation);
        }
        return STask.Completed;
    }
    public override void Dispose()
    {
        if (this.uiStates == UIStates.Success)
        {
            //enter异步正在执行过程中 关闭了UI 则不播放上一个动画的打开
            //先显示上一个UI 这样可以在_onDispose事件里面访问到当前显示的UI
            var lst = this.Parent?.GetChildren();
            if (lst?.FindLast(t => t is UIBase) == this)
            {
                for (int i = lst.Count - 2; i >= 0; i--)
                {
                    if (lst[i] is UIBase && !lst[i].Disposed)
                    {
                        if (!lst[i].As<UIBase>().isShow)
                            ((UIBase)lst[i]).Show();
                        break;
                    }
                }
            }
        }

        base.Dispose();

        //先执行退出逻辑
        if (this.uiStates >= UIStates.Success)
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
    public sealed override bool EventEnable { get => base.EventEnable && isShow; set => base.EventEnable = value; }
    public sealed override bool TimerEnable { get => base.TimerEnable && isShow; set => base.TimerEnable = value; }
    public sealed override void AddChild(SObject child) => base.AddChild(child);
    public sealed override int GetChildIndex(SObject child) => base.GetChildIndex(child);
    public sealed override void Remove(SObject child) => base.Remove(child);
    public sealed override bool Enable { get => base.Enable; set => base.Enable = value; }
}