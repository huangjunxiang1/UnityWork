using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class STree : STree<SObject>
    {
        public STree(long rpc = 0) : base(rpc) { }
    }
    public class STree<T> : SObject where T : SObject
    {
        public STree(long rpc = 0) : base(rpc) { }

        Dictionary<long, T> _childrenGMap = ObjectPool.Get<Dictionary<long, T>>();
        Dictionary<long, T> _childrenRMap = ObjectPool.Get<Dictionary<long, T>>();
        internal List<T> _children = ObjectPool.Get<List<T>>();

        public sealed override World World
        {
            get => base.World;
            set
            {
                base.World = value;
                for (int i = 0; i < _children.Count; i++)
                    _children[i].World = value;
            }
        }

        public override void Dispose()
        {
            this._disposeAllChildren(true);
            base.Dispose();
        }

        /// <summary>
        /// 子添加
        /// </summary>
        /// <param name="child"></param>
        public virtual void AddChild(T child)
        {
            if (child.Parent == this)
                return;

#if DebugEnable
            SObject o = this;
            do
            {
                if (o == child)
                {
                    Loger.Error("树节点循环");
                    return;
                }
            } while ((o = o.Parent) != null);
#endif

            if (child.Parent != null)
                child.Parent.Remove(child);

            _childrenGMap.Add(child.gid, child);
            if (child.rpc != 0)
            {
                if (!_childrenRMap.ContainsKey(child.rpc))
                    _childrenRMap[child.rpc] = child;
                else
                    Loger.Error($"已经包含子对象 this={this} child={child}");
            }

            _children.Add(child);
            child.Parent = this;
            child.World = this.World;
        }
        public override int GetChildIndex(SObject child) => _children.IndexOf((T)child);

        /// <summary>
        /// 子获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetChildGid(long gid)
        {
            _childrenGMap.TryGetValue(gid, out T child);
            return child;
        }
        public T GetChildRpc(long rpc)
        {
            if (rpc == 0)
            {
                Loger.Error("无效rpc");
                return null;
            }
            _childrenRMap.TryGetValue(rpc, out T child);
            return child;
        }
        public bool TryGetChildGid(long gid, out T child) => _childrenGMap.TryGetValue(gid, out child);
        public bool TryGetChildRpc(long rpc, out T child)
        {
            if (rpc == 0)
            {
                Loger.Error("无效rpc");
                child = null;
                return false;
            }
            return _childrenRMap.TryGetValue(rpc, out child);
        }
        public List<T> ToChildren() => new(_children);
        public List<T> GetChildren() => _children;
        public K GetChild<K>() where K : T => _children.Find(t => t is K) as K;

        /// <summary>
        /// 子移除
        /// </summary>
        /// <param name="child"></param>
        public override void Remove(SObject child)
        {
            if (!_childrenGMap.ContainsKey(child.gid))
                return;
            _childrenGMap.Remove(child.gid);
            if (child.rpc != 0)
                _childrenRMap.Remove(child.rpc);
            _children.Remove((T)child);
            child.Parent = null;
        }
        public virtual void RemoveGid(long gid, bool dispose = true)
        {
            if (!_childrenGMap.TryGetValue(gid, out T child))
                return;
            if (dispose)
            {
                child.Dispose();
                return;
            }
            this.Remove(child);
        }
        public virtual void RemoveRpc(long rpc, bool dispose = true)
        {
            if (rpc == 0)
            {
                Loger.Error("无效rpc");
                return;
            }
            if (!_childrenRMap.TryGetValue(rpc, out T child))
                return;
            if (dispose)
            {
                child.Dispose();
                return;
            }
            this.Remove(child);
        }
        public void DisposeAllChildren() => this._disposeAllChildren(false);

        void _disposeAllChildren(bool _isDisposeSelf)
        {
            if (_isDisposeSelf)
            {
                _childrenGMap.Clear();
                ObjectPool.Return(_childrenGMap);
                _childrenGMap = null;

                _childrenRMap.Clear();
                ObjectPool.Return(_childrenRMap);
                _childrenRMap = null;

                List<T> tmp = _children;
                _children = null;
                for (int i = tmp.Count - 1; i >= 0; i--)
                {
                    tmp[i].Parent = null;
                    if (!tmp[i].Disposed)
                        tmp[i].Dispose();
                }
                tmp.Clear();
                ObjectPool.Return(tmp);
            }
            else
            {
                if (_children.Count <= 0)
                    return;

                _childrenGMap.Clear();
                _childrenRMap.Clear();

                List<T> tmp = _children;
                _children = ObjectPool.Get<List<T>>();
                for (int i = tmp.Count - 1; i >= 0; i--)
                {
                    if (!tmp[i].Disposed)
                        tmp[i].Dispose();
                }
                tmp.Clear();
                ObjectPool.Return(tmp);
            }
        }
    }
}
