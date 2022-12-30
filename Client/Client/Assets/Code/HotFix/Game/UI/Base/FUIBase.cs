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
    public abstract GComponent UI { get; }

    public sealed override void Hide(bool playAnimation = true, Action callBack = null)
    {
        this.OnHide();
        if (playAnimation)
        {
            Transition close = this.UI.GetTransition("close");
            if (close != null)
            {
                UIHelper.EnableUIInput(false);
                close.Play(() =>
                {
                    UIHelper.EnableUIInput(true);
                    this.IsShow = false;
                    callBack?.Invoke();
                });
                return;
            }
            else
            {
                Transition open = this.UI.GetTransition("open");
                if (open != null)
                {
                    UIHelper.EnableUIInput(false);
                    open.PlayReverse(() =>
                    {
                        UIHelper.EnableUIInput(true);
                        this.IsShow = false;
                        callBack?.Invoke();
                    });
                    return;
                }
            }
        }
        this.IsShow = false;
        callBack?.Invoke();
    }
    public sealed override TaskAwaiter HideAsync(bool playAnimation = true)
    {
        this.OnHide();
        if (playAnimation)
        {
            Transition close = this.UI.GetTransition("close");
            if (close != null)
            {
                UIHelper.EnableUIInput(false);
                TaskAwaiter task = TaskCreater.Create();
                close.Play(() =>
                {
                    UIHelper.EnableUIInput(true);
                    this.IsShow = false;
                    task.TrySetResult();
                });
                return task;
            }
            else
            {
                Transition open = this.UI.GetTransition("open");
                if (open != null)
                {
                    UIHelper.EnableUIInput(false);
                    TaskAwaiter task = TaskCreater.Create();
                    open.PlayReverse(() =>
                    {
                        UIHelper.EnableUIInput(true);
                        this.IsShow = false;
                        task.TrySetResult();
                    });
                    return task;
                }
            }
        }
        this.IsShow = false;
        return TaskAwaiter.Completed;
    }
    public sealed override void Show(bool playAnimation = true, Action callBack = null)
    {
        this.IsShow = true;
        this.OnShow();
        if (playAnimation)
        {
            Transition open = this.UI.GetTransition("open");
            if (open != null)
            {
                UIHelper.EnableUIInput(false);
                open.Play(() =>
                {
                    UIHelper.EnableUIInput(true);
                    callBack?.Invoke();
                });
                return;
            }
        }
        callBack?.Invoke();
    }
    public sealed override TaskAwaiter ShowAsync(bool playAnimation = true)
    {
        this.IsShow = true;
        this.OnShow();
        if (playAnimation)
        {
            Transition open = this.UI.GetTransition("open");
            if (open != null)
            {
                UIHelper.EnableUIInput(false);
                TaskAwaiter task = TaskCreater.Create();
                open.Play(() =>
                {
                    UIHelper.EnableUIInput(true);
                    task.TrySetResult();
                });
                return task;
            }
        }
        return TaskAwaiter.Completed;
    }
}
