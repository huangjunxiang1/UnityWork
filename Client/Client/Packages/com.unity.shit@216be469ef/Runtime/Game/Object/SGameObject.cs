using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Main;
using UnityEngine;

namespace Game
{
    public enum SGameObjectLoadStyle
    {
        Static = 0,//场景静态类型
        Resource,//资源类型
        LogicRoot,//根节点是空节点 下面挂资源
    }
    public class SGameObject : STree<SGameObject>
    {
        public SGameObject(long cid, SGameObjectLoadStyle style = SGameObjectLoadStyle.LogicRoot) : base(cid)
        {
            this.ObjectStyle = style;
            if (ObjectStyle == SGameObjectLoadStyle.LogicRoot)
            {
                this.GameRoot = new GameObject();
#if UNITY_EDITOR
                this.Name = $"{this.GetType().Name}_id={cid}";
#endif
                if (this is not SWorld)
                {
                    this.GameRoot.transform.SetParent(SGameM.World.GameRoot.transform);
                    SGameM.World.AddChild(this);
                }
            }
        }

        //缓存数据
        SEventListener _onDispose;
        string _url;
        int _resVersion;

        /// <summary>
        /// 逻辑节点 WObjectLoadStyle.Resource模式 Root==Res
        /// </summary>
        public GameObject GameRoot { get; private set; }

        /// <summary>
        /// 资源模型
        /// </summary>
        public GameObject GameObject { get; private set; }

        public virtual string Name
        {
            get => GameRoot?.name;
            set
            {
                if (!GameRoot)
                    return;
                GameRoot.name = value;
            }
        }

        /// <summary>
        ///  
        /// </summary>
        public SGameObjectLoadStyle ObjectStyle { get; }

        /// <summary>
        /// 坐标
        /// </summary>
        public Vector3 Position
        {
            get
            {
                if (this.GameRoot)
                    return this.GameRoot.transform.position;
                return default;
            }
            set
            {
                if (ObjectStyle == SGameObjectLoadStyle.Static)
                {
                    Loger.Error("静态物体不能设置坐标");
                    return;
                }
                if (this.GameRoot)
                    this.GameRoot.transform.position = value;
            }
        }

        /// <summary>
        /// dispose 监听
        /// </summary>
        public SEventListener OnDispose => _onDispose ??= new SEventListener(this);

        /// <summary>
        /// 设置加载的资源模型
        /// </summary>
        /// <param name="res"></param>
        public virtual void SetGameObject(GameObject res, bool release = true)
        {
            ++_resVersion;
            switch (ObjectStyle)
            {
                case SGameObjectLoadStyle.Static:
                    if (this.GameRoot)
                    {
                        Loger.Error("已set 游戏对象");
                        return;
                    }
                    this.GameRoot = res;
                    this.GameObject = res;
                    break;
                case SGameObjectLoadStyle.Resource:
                    if (res == this.GameObject)
                        return;

                    if (this.GameObject)
                    {
                        if (res)
                        {
                            res.transform.parent = this.GameObject.transform.parent;
                            res.transform.SetPositionAndRotation(this.GameObject.transform.position, this.GameObject.transform.rotation);
                        }
                        if (release)
                            SAsset.Release(this.GameObject);
                        else
                            this.GameObject.transform.parent = SGameM.World.GameRoot.transform;
                    }
                    else
                    {
                        if (res)
                        {
                            res.transform.SetParent(SGameM.World.GameRoot.transform);
                            res.transform.SetPositionAndRotation(default, Quaternion.identity);
                        }
                    }

                    this.GameRoot = res;
                    this.GameObject = res;
                    break;
                case SGameObjectLoadStyle.LogicRoot:
                    if (res == this.GameObject)
                        return;

                    if (res)
                    {
                        res.transform.parent = this.GameRoot.transform;
                        res.transform.SetLocalPositionAndRotation(default, Quaternion.identity);
                    }

                    if (this.GameObject)
                    {
                        if (release)
                            SAsset.Release(this.GameObject);
                        else
                            this.GameObject.transform.parent = SGameM.World.GameRoot.transform;
                    }
                    this.GameObject = res;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 主动加载资源
        /// </summary>
        /// <param name="url"></param>
        public void LoadGameObject(string url, ReleaseMode releaseMode = ReleaseMode.Destroy)
        {
            if (this.ObjectStyle == SGameObjectLoadStyle.Static)
            {
                Loger.Error("静态类型不能动态加载");
                return;
            }
            if (_url == url)
                return;
            _url = url;
            ++_resVersion;
            SetGameObject(SAsset.LoadGameObject(_url, releaseMode));
        }
        public async STask LoadGameObjectAsync(string url, ReleaseMode releaseMode = ReleaseMode.Destroy)
        {
            if (this.ObjectStyle == SGameObjectLoadStyle.Static)
            {
                Loger.Error("静态类型不能动态加载");
                return;
            }
            if (_url == url)
                return;
            _url = url;
            int ver = ++_resVersion;
            GameObject res = await SAsset.LoadGameObjectAsync(_url, releaseMode);
            if (ver != _resVersion)
            {
                SAsset.Release(res);
                return;
            }
            SetGameObject(res);
        }

        /// <summary>
        /// 释放对象 取消所有已注册的事件
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();

            switch (this.ObjectStyle)
            {
                case SGameObjectLoadStyle.Static:
                    break;
                case SGameObjectLoadStyle.Resource:
                    if (this.GameObject)
                        SAsset.Release(this.GameObject);
                    break;
                case SGameObjectLoadStyle.LogicRoot:
                    if (this.GameObject)
                        SAsset.Release(this.GameObject);
                    GameObject.DestroyImmediate(this.GameRoot);
                    break;
                default:
                    break;
            }

            _onDispose?.Call();
        }
    }
}
