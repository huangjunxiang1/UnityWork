using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Main;
using Game;
abstract class UUIBase : UIBase
{
    public abstract Canvas Canvas { get; }

    public abstract RectTransform UI { get; }

    public sealed override int SortOrder
    {
        get { return this.Canvas.sortingOrder; }
        set { this.Canvas.sortingOrder = value; }
    }

    public sealed override bool IsShow
    {
        get { return this.UI.gameObject.activeSelf; }
        set { this.UI.gameObject.SetActive(value); }
    }

    public sealed override void Hide(bool playAnimation = true, Action callBack = null)
    {
        if (playAnimation)
        {
            Animation ani = this.UI.GetComponent<Animation>();
            if (ani)
            {
                if (ani.Play("close"))
                {
                    UIHelper.EnableUIInput(false);
                    Timer.Add(ani["close"].length + 0.1f, 1, () =>
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
        if (playAnimation)
        {
            Animation ani = this.UI.GetComponent<Animation>();
            if (ani)
            {
                if (ani.Play("close"))
                {
                    UIHelper.EnableUIInput(false);
                    TaskAwaiter task = TaskCreater.Create();
                    Timer.Add(ani["close"].length + 0.1f, 1, () =>
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
    public override void Show(bool playAnimation = true, Action callBack = null)
    {
        this.IsShow = true;
        if (playAnimation)
        {
            Animation ani = this.UI.GetComponent<Animation>();
            if (ani)
            {
                if (ani.Play("open"))
                {
                    if (callBack != null)
                    {
                        UIHelper.EnableUIInput(false);
                        Timer.Add(ani["open"].length + 0.1f, 1, () =>
                        {
                            UIHelper.EnableUIInput(true);
                            callBack?.Invoke();
                        });
                    }
                    return;
                }
            }
        }
        callBack?.Invoke();
    }
    public override TaskAwaiter ShowAsync(bool playAnimation = true)
    {
        this.IsShow = true;
        if (playAnimation)
        {
            Animation ani = this.UI.GetComponent<Animation>();
            if (ani)
            {
                if (ani.Play("open"))
                {
                    UIHelper.EnableUIInput(false);
                    TaskAwaiter task = TaskCreater.Create();
                    Timer.Add(ani["open"].length + 0.1f, 1, () =>
                    {
                        UIHelper.EnableUIInput(true);
                        this.IsShow = true;
                        task.TrySetResult();
                    });
                    return task;
                }
            }
        }
        return TaskAwaiter.Completed;
    }

    public sealed override void Dispose()
    {
        base.Dispose();

        //ui不做池管理
        if (this.UI)
        {
            this.Hide(true, () =>
            {
                AssetLoad.ReleaseTextureRef(this.UI.gameObject);
                AssetLoad.Release(this.UI.gameObject);
            });
        }
    }
}