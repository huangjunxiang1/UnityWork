using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Main;
using UnityEngine;

namespace Game
{
    public class WObject : TreeM<WObject>
    {
        public WObject(long id, GameObject root = null) : base(id)
        {
            if (!root)
            {
                this.isLogicRoot = true;
                this.Root = new GameObject();
            }
            else
            {
                this.isLogicRoot = false;
                this.Root = root;
                this.SetRes(root);
            }
#if UNITY_EDITOR
            this.Name = $"{this.GetType().FullName}_id={id}";
#endif
        }

        //挂载数据
        public int value;
        public object data;

        //缓存数据
        Eventer _onDispose;

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
        /// =false则表示 没有空逻辑节点  资源模型就是逻辑节点   =true 表示这是挂点方式 挂点下面是资源模型
        /// </summary>
        public bool isLogicRoot { get; }

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
            if (this.isLogicRoot)
            {
                res.transform.parent = this.Root.transform;
                res.transform.rotation = Quaternion.identity;
                res.transform.localPosition = default;
                if (this.Res)
                    AssetLoad.Release(this.Res);
                this.Res = res;
            }
            else
            {
                if (this.Root)
                {
                    res.transform.parent = this.Root.transform.parent;
                    res.transform.rotation = this.Root.transform.rotation;
                    res.transform.position = this.Root.transform.position;
                }
                this.Root = res;
            }
        }

        /// <summary>
        /// 主动加载资源
        /// </summary>
        /// <param name="url"></param>
        public async void LoadRes(string url)
        {
            SetRes(await AssetLoad.LoadGameObjectAsync(url, TaskCreater));
        }

        /// <summary>
        /// 释放对象 取消所有已注册的事件
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            if (this.isLogicRoot)
            {
                if (this.Res)
                    AssetLoad.Release(this.Res);
                //动态创建的节点 直接删除
                GameObject.DestroyImmediate(this.Root);
            }
            if (_onDispose != null) _onDispose.Call();
        }
    }
}
