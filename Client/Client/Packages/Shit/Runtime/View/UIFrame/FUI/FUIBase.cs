using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Game;
using FairyGUI;

public abstract class FUIBase : UIBase
{
    bool isShowing = false;
    STask showTask;
    bool isHiding = false;
    STask hideTask;

    public static Func<FUIBase, string, GComponent> CreateUI;
    public static Func<FUIBase, string, STask<GComponent>> CreateUIAsync;
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
                InputHelper.EnableUIInput(false);
                close.Play(() =>
                {
                    isHiding = false;
                    InputHelper.EnableUIInput(true);
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
                    InputHelper.EnableUIInput(false);
                    open.PlayReverse(() =>
                    {
                        isHiding = false;
                        InputHelper.EnableUIInput(true);
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
                InputHelper.EnableUIInput(false);
                close.Play(() =>
                {
                    isHiding = false;
                    InputHelper.EnableUIInput(true);
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
                    InputHelper.EnableUIInput(false);
                    open.PlayReverse(() =>
                    {
                        isHiding = false;
                        InputHelper.EnableUIInput(true);
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
                InputHelper.EnableUIInput(false);
                open.Play(() =>
                {
                    isShowing = false;
                    InputHelper.EnableUIInput(true);
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
                InputHelper.EnableUIInput(false);
                open.Play(() =>
                {
                    isShowing = false;
                    InputHelper.EnableUIInput(true);
                    showTask.TrySetResult();
                });
                return showTask;
            }
        }
        return STask.Completed;
    }
}