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
    public class WObject : TreeM<WObject>
    {
        public WObject(long cid, WObjectLoadStyle style = WObjectLoadStyle.LogicRoot) : base(cid)
        {
            this.ObjectStyle = style;
            if (ObjectStyle == WObjectLoadStyle.LogicRoot)
            {
                this.Root = new GameObject();
#if UNITY_EDITOR
                this.Name = $"{this.GetType().FullName}_id={cid}";
#endif
            }
        }

        //缓存数据
        Eventer _onDispose;
        string _url;
        bool _isLogicRoot;

        //挂载数据
        public int value;
        public object data;

        /// <summary>
        /// 逻辑节点
        /// </summary>
        public GameObject Root { get; private set; }

        /// <summary>
        /// 资源模型
        /// </summary>
        public GameObject Res { get; private set; }

        public virtual string Name
        {
            get => Root?.name;
            set
            {
                if (!Root)
                    return;
                Root.name = value;
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
            get { return Root.transform.position; }
            set { Root.transform.position = value; }
        }

        /// <summary>
        /// dispose 监听
        /// </summary>
        public Eventer OnDispose
        {
            get
            {
                if (_onDispose == null) _onDispose = new Eventer(this);
                return _onDispose;
            }
        }

        /// <summary>
        /// 设置加载的资源模型
        /// </summary>
        /// <param name="res"></param>
        public virtual void SetRes(GameObject res)
        {
            switch (ObjectStyle)
            {
                case WObjectLoadStyle.Static:
                    this.Root = res;
                    break;
                case WObjectLoadStyle.Resource:
                    if (res == this.Root)
                        return;

                    if (this.Root)
                    {
                        res.transform.parent = this.Root.transform.parent;
                        res.transform.rotation = this.Root.transform.rotation;
                        res.transform.position = this.Root.transform.position;
                    }
                    this.Root = res;
                    this.Res = res;
                    break;
                case WObjectLoadStyle.LogicRoot:
                    if (res == this.Res)
                        return;

                    res.transform.parent = this.Root.transform;
                    res.transform.rotation = Quaternion.identity;
                    res.transform.localPosition = default;

                    if (this.Res)
                        AssetLoad.Release(this.Res);
                    this.Res = res;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 主动加载资源
        /// </summary>
        /// <param name="url"></param>
        public async TaskAwaiter LoadRes(string url)
        {
            if (this.ObjectStyle == WObjectLoadStyle.Static)
            {
                Loger.Error("静态类型不能动态加载");
                return;
            }
            if (_url == url)
                return;
            _url = url;
            SetRes(await AssetLoad.LoadGameObjectAsync(_url, TaskCreater));
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
                    if (this.Res)
                        AssetLoad.Release(this.Res);
                    break;
                case WObjectLoadStyle.LogicRoot:
                    if (this.Res)
                        AssetLoad.Release(this.Res);
                    GameObject.DestroyImmediate(this.Root);
                    break;
                default:
                    break;
            }

            if (_onDispose != null) _onDispose.Call();
        }
    }
}
