using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Main;
using UnityEngine;

namespace Game
{
    public class WObject : EntityM
    {
        public WObject(long id, GameObject gameObject) : base(id)
        {
            this.GameObject = gameObject;
        }

        //挂载数据
        public double value;
        public object data;

        //缓存数据
        Dictionary<long, WObject> _childMap = new Dictionary<long, WObject>();
        List<WObject> _childLst = new List<WObject>();
        Eventer _onDispose;

        /// <summary>
        /// 对应物体
        /// </summary>
        public GameObject GameObject { get; }

        /// <summary>
        /// 坐标
        /// </summary>
        public Vector3 Position
        {
            get { return GameObject.transform.position; }
            set { GameObject.transform.position = value; }
        }

        /// <summary>
        /// 父节点
        /// </summary>
        public WObject Parent { get; private set; }

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
        /// 释放对象 取消所有已注册的事件
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            for (int i = 0; i < _childLst.Count; i++)
                _childLst[i].Dispose();
            _childMap.Clear();
            _childLst.Clear();
            if (this.GameObject)
                AssetLoad.Return(this.GameObject);
            if (_onDispose != null) _onDispose.Call();
        }

        /// <summary>
        /// 设置父管理节点
        /// </summary>
        /// <param name="parent"></param>
        public void SetParent(WObject parent)
        {
            if (this.Parent == parent)
                return;

            if (this.Parent != null)
                this.Parent.Remove(this.ID);
            if (parent != null)
                parent.AddChild(this);
        }

        /// <summary>
        /// 子添加
        /// </summary>
        /// <param name="child"></param>
        public virtual void AddChild(WObject child)
        {
            if (child.Parent == this)
                return;
            if (child.Parent != null)
                child.Parent.Remove(child.ID);
            _childMap.Add(child.ID, child);
            _childLst.Add(child);
            child.Parent = this;
        }

        /// <summary>
        /// 子获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WObject GetChild(long id)
        {
            _childMap.TryGetValue(id, out WObject child);
            return child;
        }
        public List<WObject> GetChildren()
        {
            List<WObject> lst = new List<WObject>(_childLst.Count);
            lst.AddRange(_childLst);
            return lst;
        }

        /// <summary>
        /// 子移除
        /// </summary>
        /// <param name="child"></param>
        public void Remove(WObject child)
        {
            Remove(child.ID);
        }
        public virtual void Remove(long id)
        {
            if (!_childMap.TryGetValue(id, out WObject child))
                return;
            _childMap.Remove(id);
            _childLst.Remove(child);
            child.Parent = null;
        }
    }
}
