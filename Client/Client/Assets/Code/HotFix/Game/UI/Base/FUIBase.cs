using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Main;
using Game;
using FairyGUI;

abstract class FUIBase : UIBase
{
    bool isShowing = false;
    TaskAwaiter showTask;
    bool isHiding = false;
    TaskAwaiter hideTask;

    public abstract GComponent UI { get; }

    public sealed override void Hide(bool playAnimation = true, Action callBack = null)
    {
        if (isHiding)
        {
            hideTask.AddEvent(callBack);
            return;
        }

        this.OnHide();
        base.Hide(playAnimation, callBack);
        if (playAnimation)
        {
            Transition close = this.UI.GetTransition("close");
            if (close != null)
            {
                isHiding = true;
                hideTask = TaskCreater.Create();
                UIHelper.EnableUIInput(false);
                close.Play(() =>
                {
                    isHiding = false;
                    UIHelper.EnableUIInput(true);
                    this.isShow = false;
                    callBack?.Invoke();
                    hideTask.TrySetResult();
                });
                return;
            }
            else
            {
                Transition open = this.UI.GetTransition("open");
                if (open != null)
                {
                    isHiding = true;
                    hideTask = TaskCreater.Create();
                    open.Stop(false, true);
                    UIHelper.EnableUIInput(false);
                    open.PlayReverse(() =>
                    {
                        isHiding = false;
                        UIHelper.EnableUIInput(true);
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
    public sealed override TaskAwaiter HideAsync(bool playAnimation = true)
    {
        if (isHiding)
            return hideTask;

        this.OnHide();
        base.HideAsync(playAnimation);
        if (playAnimation)
        {
            Transition close = this.UI.GetTransition("close");
            if (close != null)
            {
                isHiding = true;
                hideTask = TaskCreater.Create();
                UIHelper.EnableUIInput(false);
                close.Play(() =>
                {
                    isHiding = false;
                    UIHelper.EnableUIInput(true);
                    this.isShow = false;
                    hideTask.TrySetResult();
                });
                return hideTask;
            }
            else
            {
                Transition open = this.UI.GetTransition("open");
                if (open != null)
                {
                    isHiding = true;
                    hideTask = TaskCreater.Create();
                    open.Stop(false, true);
                    UIHelper.EnableUIInput(false);
                    open.PlayReverse(() =>
                    {
                        isHiding = false;
                        UIHelper.EnableUIInput(true);
                        this.isShow = false;
                        hideTask.TrySetResult();
                    });
                    return hideTask;
                }
            }
        }
        this.isShow = false;
        return TaskAwaiter.Completed;
    }
    public sealed override void Show(bool playAnimation = true, Action callBack = null)
    {
        if (isShowing)
        {
            showTask.AddEvent(callBack);
            return;
        }

        this.isShow = true;
        this.OnShow();
        base.Show(playAnimation, callBack);
        if (playAnimation)
        {
            Transition open = this.UI.GetTransition("open");
            if (open != null)
            {
                isShowing = true;
                showTask = TaskCreater.Create();
                open.Stop(false, true);
                UIHelper.EnableUIInput(false);
                open.Play(() =>
                {
                    isShowing = false;
                    UIHelper.EnableUIInput(true);
                    callBack?.Invoke();
                    showTask.TrySetResult();
                });
                return;
            }
        }
        callBack?.Invoke();
    }
    public sealed override TaskAwaiter ShowAsync(bool playAnimation = true)
    {
        if (isShowing)
            return showTask;

        this.isShow = true;
        this.OnShow();
        base.ShowAsync(playAnimation);
        if (playAnimation)
        {
            Transition open = this.UI.GetTransition("open");
            if (open != null)
            {
                isShowing = true;
                showTask = TaskCreater.Create();
                open.Stop(false, true);
                UIHelper.EnableUIInput(false);
                open.Play(() =>
                {
                    isShowing = false;
                    UIHelper.EnableUIInput(true);
                    showTask.TrySetResult();
                });
                return showTask;
            }
        }
        return TaskAwaiter.Completed;
    }
}
