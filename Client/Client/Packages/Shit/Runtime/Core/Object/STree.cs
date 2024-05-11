using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class STree : SObject
    {
        public STree(long rpc = 0) : base(rpc) { }

        Dictionary<long, SObject> _childrenGMap = ObjectPool.Get<Dictionary<long, SObject>>();
        Dictionary<long, SObject> _childrenRMap = ObjectPool.Get<Dictionary<long, SObject>>();
        internal List<SObject> _children = ObjectPool.Get<List<SObject>>();

        public override void Dispose()
        {
            this._disposeAllChildren(true);
            base.Dispose();
        }

        /// <summary>
        /// 子添加
        /// </summary>
        /// <param name="child"></param>
        public override void AddChild(SObject child)
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
        public override int GetChildIndex(SObject child) => _children.IndexOf(child);

        /// <summary>
        /// 子获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SObject GetChildGid(long gid)
        {
            _childrenGMap.TryGetValue(gid, out var child);
            return child;
        }
        public SObject GetChildRpc(long rpc)
        {
            if (rpc == 0)
            {
                Loger.Error("无效rpc");
                return null;
            }
            _childrenRMap.TryGetValue(rpc, out var child);
            return child;
        }
        public bool TryGetChildGid(long gid, out SObject child) => _childrenGMap.TryGetValue(gid, out child);
        public bool TryGetChildRpc(long rpc, out SObject child)
        {
            if (rpc == 0)
            {
                Loger.Error("无效rpc");
                child = null;
                return false;
            }
            return _childrenRMap.TryGetValue(rpc, out child);
        }
        public List<SObject> ToChildren() => new(_children);
        public List<SObject> GetChildren() => _children;
        public T GetChild<T>() where T : SObject => _children.Find(t => t is T) as T;

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
            _children.Remove(child);
            child.Parent = null;
        }
        public void RemoveGid(long gid, bool dispose = true)
        {
            if (!_childrenGMap.TryGetValue(gid, out var child))
                return;
            if (dispose)
            {
                child.Dispose();
                return;
            }
            this.Remove(child);
        }
        public void RemoveRpc(long rpc, bool dispose = true)
        {
            if (rpc == 0)
            {
                Loger.Error("无效rpc");
                return;
            }
            if (!_childrenRMap.TryGetValue(rpc, out var child))
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

                List<SObject> tmp = _children;
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

                List<SObject> tmp = _children;
                _children = ObjectPool.Get<List<SObject>>();
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
