using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Core;
using Event;
using Game;

public abstract class UIBase : STree
{
    public UIConfig uiConfig { get; private set; }
    public abstract string url { get; }

    public abstract UIStatus uiStates { get; }
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
    public object[] ParamObjects { get; private set; }

    public virtual STask LoadConfig(UIConfig config, STask completed, params object[] data)
    {
        this.ParamObjects = data;
        this.uiConfig = config;
        onCompleted = completed;
        return STask.Completed;
    }
    public virtual STask LoadConfigAsync(UIConfig config, STask completed, params object[] data)
    {
        this.ParamObjects = data;
        this.uiConfig = config;
        onCompleted = completed;
        return STask.Completed;
    }
    public T Open<T>(params object[] data) where T : UIBase, new()
    {
        T ui = this.GetChild<T>();
        if (ui != null)
        {
            if (ui.onCompleted != null && ui.onCompleted.IsCompleted)
                ui.Show();
            this.GetChildren().Remove(ui);
            this.GetChildren().Add(ui);
            return ui;
        }

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
            if (ui.onCompleted != null)
            {
                await ui.onCompleted;
                ui.Show();
            }
            this.GetChildren().Remove(ui);
            this.GetChildren().Add(ui);
            return ui;
        }

        using (await STaskLocker.Lock(this))
        {
            UIConfig cfg = typeof(T).GetCustomAttribute<UIConfig>() ?? UIConfig.Default;

            UIGlobalConfig.EnableUIInput(false);
            ui = new();
            //在执行异步的过程中有可能会关闭这个UI
            ui.onDispose.Add(() =>
            {
                if (ui.uiStates < UIStatus.Success)
                    UIGlobalConfig.EnableUIInput(true);
            });
            this.AddChild(ui);
            await ui.LoadConfigAsync(cfg, new STask<T>(), data);
            if (ui.Disposed)
                return ui;

            ui.Show();
            UIGlobalConfig.EnableUIInput(true);

            ui.onCompleted.TrySetResult(ui);
            return ui;
        }
    }
    public virtual void Hide(bool playAnimation = true, Action callBack = null)
    {
        var uis = this.GetChildren();
        for (int i = uis.Count - 1; i >= 0; i--)
        {
            if (uis[i] is UIBase ui)
                ui.Hide(playAnimation);
        }
    }
    public virtual STask HideAsync(bool playAnimation = true)
    {
        var uis = this.GetChildren();
        for (int i = uis.Count - 1; i >= 0; i--)
        {
            if (uis[i] is UIBase ui)
                ui.HideAsync(playAnimation);
        }
        return STask.Completed;
    }
    public virtual void Show(bool playAnimation = true, Action callBack = null)
    {
        var uis = this.GetChildren();
        for (int i = uis.Count - 1; i >= 0; i--)
        {
            if (uis[i] is UIBase ui)
                ui.Show(playAnimation);
        }
        this.OnView();
    }
    public virtual STask ShowAsync(bool playAnimation = true)
    {
        var uis = this.GetChildren();
        for (int i = uis.Count - 1; i >= 0; i--)
        {
            if (uis[i] is UIBase ui)
                ui.ShowAsync(playAnimation);
        }
        this.OnView();
        return STask.Completed;
    }
    public override void Dispose()
    {
        if (this.uiStates == UIStatus.Success)
        {
            //enter异步正在执行过程中 关闭了UI 则不播放上一个动画的打开
            //先显示上一个UI 这样可以在_onDispose事件里面访问到当前显示的UI
            var lst = this.Parent?.GetChildren();
            if (lst?.FindLast(t => t is UIBase) == this)
            {
                for (int i = lst.Count - 2; i >= 0; i--)
                {
                    if (lst[i] is UIBase ui && !ui.Disposed)
                    {
                        if (!ui.isShow)
                            ui.Show();
                        break;
                    }
                }
            }
        }

        base.Dispose();

        //先执行退出逻辑
        if (this.uiStates >= UIStatus.Success)
            this.OnExit();
    }
    public void CloseUI(Func<UIBase, bool> test = null)
    {
        foreach (var ui in this.ToChildren().FindAll(t => t is UIBase u && (test == null || test(u))))
        {
            if (ui.Disposed) continue;
            ui.Dispose();
        }
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

    protected virtual void OnView() { }//UI刷新
    protected abstract void Binding();//UI元件绑定

    /// <summary>
    /// 网络重连上则重新刷新UI
    /// </summary>
    /// <param name="e"></param>
    [Event(10)] void EC_NetworkReconnection(EC_NetworkReconnection e) => this.OnView();

    public sealed override void AcceptedEvent() { base.AcceptedEvent(); }
    public sealed override bool EventEnable { get => base.EventEnable && isShow; set => base.EventEnable = value; }
    public sealed override bool TimerEnable { get => base.TimerEnable && isShow; set => base.TimerEnable = value; }
    public sealed override void AddChild(SObject child) => base.AddChild(child);
    public sealed override int GetChildIndex(SObject child) => base.GetChildIndex(child);
    public sealed override void Remove(SObject child) => base.Remove(child);
    public sealed override bool Enable { get => base.Enable; set => base.Enable = value; }
}