using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Game;
using Core;
abstract class UUIBase : UIBase
{
    bool isShowing = false;
    STask showTask;
    bool isHiding = false;
    STask hideTask;

    public abstract Canvas Canvas { get; }
    public abstract RectTransform ui { get; }
    public sealed override int sortOrder
    {
        get { return this.Canvas.sortingOrder; }
        set { this.Canvas.sortingOrder = value; }
    }
    public sealed override bool isShow
    {
        get { return this.ui.gameObject.activeSelf; }
        set { this.ui.gameObject.SetActive(value); }
    }

    public sealed override async void Hide(bool playAnimation = true, Action callBack = null)
    {
        if (isHiding)
        {
            await hideTask;
            callBack?.Invoke();
            return;
        }

        this.OnHide();
        base.Hide(playAnimation,callBack);
        if (playAnimation)
        {
            Animation ani = this.ui.GetComponent<Animation>();
            if (ani)
            {
                if (ani.GetClip("close") != null)
                {
                    ani.Stop();
                    ani.Play("close");
                    isHiding = true;
                    hideTask = new();
                    UIHelper.EnableUIInput(false);
                    World.Timer.Add(ani["close"].length + 0.1f, 1, () =>
                    {
                        isHiding = false;
                        UIHelper.EnableUIInput(true);
                        this.isShow = false;
                        callBack?.Invoke();
                        hideTask.TrySetResult();
                    });
                    return;
                }
                if (ani.GetClip("open") != null)
                {
                    ani.Stop();
                    ani.Rewind("open");
                    isHiding = true;
                    hideTask = new();
                    UIHelper.EnableUIInput(false);
                    World.Timer.Add(ani["open"].length + 0.1f, 1, () =>
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
    public sealed override STask HideAsync(bool playAnimation = true)
    {
        if (isHiding)
            return showTask;

        this.OnHide();
        base.HideAsync(playAnimation);
        if (playAnimation)
        {
            Animation ani = this.ui.GetComponent<Animation>();
            if (ani)
            {
                ani.Stop();
                if (ani.GetClip("close") != null)
                {
                    ani.Stop();
                    ani.Play("close");
                    isHiding = true;
                    hideTask = new();
                    UIHelper.EnableUIInput(false);
                    World.Timer.Add(ani["close"].length + 0.1f, 1, () =>
                    {
                        isHiding = false;
                        UIHelper.EnableUIInput(true);
                        this.isShow = false;
                        hideTask.TrySetResult();
                    });
                    return hideTask;
                }
                if (ani.GetClip("open") != null)
                {
                    ani.Stop();
                    ani.Rewind("open");
                    isHiding = true;
                    hideTask = new();
                    UIHelper.EnableUIInput(false);
                    World.Timer.Add(ani["open"].length + 0.1f, 1, () =>
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
        base.Show(playAnimation,callBack);
        if (playAnimation)
        {
            Animation ani = this.ui.GetComponent<Animation>();
            if (ani)
            {
                ani.Stop();
                if (ani.Play("open"))
                {
                    isShowing = true;
                    showTask = new();
                    UIHelper.EnableUIInput(false);
                    World.Timer.Add(ani["open"].length + 0.1f, 1, () =>
                    {
                        isShowing = false;
                        UIHelper.EnableUIInput(true);
                        callBack?.Invoke();
                        showTask.TrySetResult();
                    });
                    return;
                }
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
            Animation ani = this.ui.GetComponent<Animation>();
            if (ani)
            {
                ani.Stop();
                if (ani.Play("open"))
                {
                    isShowing = true;
                    showTask = new();
                    UIHelper.EnableUIInput(false);
                    World.Timer.Add(ani["open"].length + 0.1f, 1, () =>
                    {
                        UIHelper.EnableUIInput(true);
                        this.isShow = true;
                        showTask.TrySetResult();
                    });
                    return showTask;
                }
            }
        }
        return STask.Completed;
    }
    public override void Dispose()
    {
        //ui不做池管理
        if (this.ui)
            this.Hide(true, () => SAsset.Release(this.ui.gameObject));
        base.Dispose();
    }
}