using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Main;
using UnityEngine;

namespace Game
{
    public enum WObjectLoadStyle
    {
        Static = 0,//场景静态类型
        Resource,//资源类型
        LogicRoot,//根节点是空节点 下面挂资源
    }
    public class WObjectM : TreeM<WObjectM>
    {
        public WObjectM(long cid, WObjectLoadStyle style = WObjectLoadStyle.LogicRoot) : base(cid)
        {
            this.ObjectStyle = style;
            if (ObjectStyle == WObjectLoadStyle.LogicRoot)
            {
                this.goRoot = new GameObject();
#if UNITY_EDITOR
                this.Name = $"{this.GetType().Name}_id={cid}";
#endif
                if (this is not WorldM)
                {
                    this.goRoot.transform.SetParent(GameM.World.goRoot.transform);
                    GameM.World.AddChild(this);
                }
            }
        }

        //缓存数据
        Eventer _onDispose;
        string _url;
        int _resVersion;

        /// <summary>
        /// 逻辑节点 WObjectLoadStyle.Resource模式 Root==Res
        /// </summary>
        public GameObject goRoot { get; private set; }

        /// <summary>
        /// 资源模型
        /// </summary>
        public GameObject goRes { get; private set; }

        public virtual string Name
        {
            get => goRoot?.name;
            set
            {
                if (!goRoot)
                    return;
                goRoot.name = value;
            }
        }

        /// <summary>
        ///  
        /// </summary>
        public WObjectLoadStyle ObjectStyle { get; }

        /// <summary>
        /// 坐标
        /// </summary>
        public Vector3 Position
        {
            get
            {
                if (this.goRoot)
                    return this.goRoot.transform.position;
                return default;
            }
            set
            {
                if (ObjectStyle == WObjectLoadStyle.Static)
                {
                    Loger.Error("静态物体不能设置坐标");
                    return;
                }
                if (this.goRoot)
                    this.goRoot.transform.position = value;
            }
        }

        /// <summary>
        /// dispose 监听
        /// </summary>
        public Eventer OnDispose => _onDispose ??= new Eventer(this);

        /// <summary>
        /// 设置加载的资源模型
        /// </summary>
        /// <param name="res"></param>
        public virtual void SetRes(GameObject res, bool release = true)
        {
            ++_resVersion;
            switch (ObjectStyle)
            {
                case WObjectLoadStyle.Static:
                    if (this.goRoot)
                    {
                        Loger.Error("已set 游戏对象");
                        return;
                    }
                    this.goRoot = res;
                    this.goRes = res;
                    break;
                case WObjectLoadStyle.Resource:
                    if (res == this.goRes)
                        return;

                    if (this.goRes)
                    {
                        if (res)
                        {
                            res.transform.parent = this.goRes.transform.parent;
                            res.transform.SetPositionAndRotation(this.goRes.transform.position, this.goRes.transform.rotation);
                        }
                        if (release)
                            AssetLoad.Release(this.goRes);
                        else
                            this.goRes.transform.parent = GameM.World.goRoot.transform;
                    }
                    else
                    {
                        if (res)
                        {
                            res.transform.SetParent(GameM.World.goRoot.transform);
                            res.transform.SetPositionAndRotation(default, Quaternion.identity);
                        }
                    }

                    this.goRoot = res;
                    this.goRes = res;
                    break;
                case WObjectLoadStyle.LogicRoot:
                    if (res == this.goRes)
                        return;

                    if (res)
                    {
                        res.transform.parent = this.goRoot.transform;
                        res.transform.SetLocalPositionAndRotation(default, Quaternion.identity);
                    }

                    if (this.goRes)
                    {
                        if (release)
                            AssetLoad.Release(this.goRes);
                        else
                            this.goRes.transform.parent = GameM.World.goRoot.transform;
                    }
                    this.goRes = res;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 主动加载资源
        /// </summary>
        /// <param name="url"></param>
        public void LoadRes(string url, ReleaseMode releaseMode = ReleaseMode.Destroy)
        {
            if (this.ObjectStyle == WObjectLoadStyle.Static)
            {
                Loger.Error("静态类型不能动态加载");
                return;
            }
            if (_url == url)
                return;
            _url = url;
            ++_resVersion;
            SetRes(AssetLoad.LoadGameObject(_url, releaseMode));
        }
        public async TaskAwaiter LoadResAsync(string url, ReleaseMode releaseMode = ReleaseMode.Destroy)
        {
            if (this.ObjectStyle == WObjectLoadStyle.Static)
            {
                Loger.Error("静态类型不能动态加载");
                return;
            }
            if (_url == url)
                return;
            _url = url;
            int ver = ++_resVersion;
            GameObject res = await AssetLoad.LoadGameObjectAsync(_url, releaseMode);
            if (ver != _resVersion)
            {
                AssetLoad.Release(res);
                return;
            }
            SetRes(res);
        }

        /// <summary>
        /// 释放对象 取消所有已注册的事件
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();

            switch (this.ObjectStyle)
            {
                case WObjectLoadStyle.Static:
                    break;
                case WObjectLoadStyle.Resource:
                    if (this.goRes)
                        AssetLoad.Release(this.goRes);
                    break;
                case WObjectLoadStyle.LogicRoot:
                    if (this.goRes)
                        AssetLoad.Release(this.goRes);
                    GameObject.DestroyImmediate(this.goRoot);
                    break;
                default:
                    break;
            }

            _onDispose?.Call();
        }
    }
}
