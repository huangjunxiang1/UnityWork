using System;

#if FairyGUI
using FairyGUI;

public abstract class FUIBase : UIBase
{
    bool isShowing = false;
    STask showTask;
    bool isHiding = false;
    STask hideTask;

    public abstract GComponent ui { get; }

    public sealed override async void Hide(bool playAnimation = true, Action callBack = null)
    {
        if (isHiding)
        {
            await hideTask;
            callBack?.Invoke();
            return;
        }

        this.OnHide();
        base.Hide(playAnimation, callBack);
        if (playAnimation)
        {
            Transition close = this.ui.GetTransition("close");
            if (close != null)
            {
                isHiding = true;
                hideTask = new();
                UIGlobalConfig.EnableUIInput(false);
                close.Play(() =>
                {
                    isHiding = false;
                    UIGlobalConfig.EnableUIInput(true);
                    this.isShow = false;
                    callBack?.Invoke();
                    hideTask.TrySetResult();
                });
                return;
            }
            else
            {
                Transition open = this.ui.GetTransition("open");
                if (open != null)
                {
                    isHiding = true;
                    hideTask = new();
                    open.Stop(false, true);
                    UIGlobalConfig.EnableUIInput(false);
                    open.PlayReverse(() =>
                    {
                        isHiding = false;
                        UIGlobalConfig.EnableUIInput(true);
                        this.isShow = false;
                        callBack?.Invoke();
                        hideTask.TrySetResult();
                    });
                    return;
                }
            }
        }
        this.isShow = false;
        callBack?.Invoke();
    }
    public sealed override STask HideAsync(bool playAnimation = true)
    {
        if (isHiding)
            return hideTask;

        this.OnHide();
        base.HideAsync(playAnimation);
        if (playAnimation)
        {
            Transition close = this.ui.GetTransition("close");
            if (close != null)
            {
                isHiding = true;
                hideTask = new();
                UIGlobalConfig.EnableUIInput(false);
                close.Play(() =>
                {
                    isHiding = false;
                    UIGlobalConfig.EnableUIInput(true);
                    this.isShow = false;
                    hideTask.TrySetResult();
                });
                return hideTask;
            }
            else
            {
                Transition open = this.ui.GetTransition("open");
                if (open != null)
                {
                    isHiding = true;
                    hideTask = new();
                    open.Stop(false, true);
                    UIGlobalConfig.EnableUIInput(false);
                    open.PlayReverse(() =>
                    {
                        isHiding = false;
                        UIGlobalConfig.EnableUIInput(true);
                        this.isShow = false;
                        hideTask.TrySetResult();
                    });
                    return hideTask;
                }
            }
        }
        this.isShow = false;
        return STask.Completed;
    }
    public sealed override async void Show(bool playAnimation = true, Action callBack = null)
    {
        this.ui.parent.SetChildIndex(this.ui, this.ui.parent.numChildren);
        if (isShowing)
        {
            await showTask;
            callBack?.Invoke();
            return;
        }

        this.isShow = true;
        this.OnShow();
        base.Show(playAnimation, callBack);
        if (playAnimation)
        {
            Transition open = this.ui.GetTransition("open");
            if (open != null)
            {
                isShowing = true;
                showTask = new();
                open.Stop(false, true);
                UIGlobalConfig.EnableUIInput(false);
                open.Play(() =>
                {
                    isShowing = false;
                    UIGlobalConfig.EnableUIInput(true);
                    callBack?.Invoke();
                    showTask.TrySetResult();
                });
                return;
            }
        }
        callBack?.Invoke();
    }
    public sealed override STask ShowAsync(bool playAnimation = true)
    {
        this.ui.parent.SetChildIndex(this.ui, this.ui.parent.numChildren);
        if (isShowing)
            return showTask;

        this.isShow = true;
        this.OnShow();
        base.ShowAsync(playAnimation);
        if (playAnimation)
        {
            Transition open = this.ui.GetTransition("open");
            if (open != null)
            {
                isShowing = true;
                showTask = new();
                open.Stop(false, true);
                UIGlobalConfig.EnableUIInput(false);
                open.Play(() =>
                {
                    isShowing = false;
                    UIGlobalConfig.EnableUIInput(true);
                    showTask.TrySetResult();
                });
                return showTask;
            }
        }
        return STask.Completed;
    }
}
#endif