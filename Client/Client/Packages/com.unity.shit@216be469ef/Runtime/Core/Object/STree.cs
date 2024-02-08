using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class STree : SObject
    {
        public STree(long rpc = 0) : base(rpc) { }

        Dictionary<long, SObject> _childrenGMap = ObjectPool<Dictionary<long, SObject>>.Get();
        Dictionary<long, SObject> _childrenRMap = ObjectPool<Dictionary<long, SObject>>.Get();
        internal List<SObject> _children = ObjectPool<List<SObject>>.Get();

        public override void Dispose()
        {
            this._disposeAllChildren(true);
            base.Dispose();
        }

        /// <summary>
        /// 子添加
        /// </summary>
        /// <param name="child"></param>
        public virtual void AddChild(SObject child)
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
        }

        /// <summary>
        /// 子获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SObject GetChildGid(long gid)
        {
            _childrenGMap.TryGetValue(gid, out SObject child);
            return child;
        }
        public SObject GetChildRpc(long rpc)
        {
            if (rpc == 0)
            {
                Loger.Error("无效rpc");
                return null;
            }
            _childrenRMap.TryGetValue(rpc, out SObject child);
            return child;
        }
        public List<SObject> ToChildren() => new(_children);
        public List<SObject> GetChildren() => _children;

        /// <summary>
        /// 子移除
        /// </summary>
        /// <param name="child"></param>
        public void Remove(SObject child)
        {
            if (!_childrenGMap.ContainsKey(child.gid))
                return;
            _childrenGMap.Remove(child.gid);
            if (child.rpc != 0)
                _childrenRMap.Remove(child.rpc);
            _children.Remove(child);
            child.Parent = null;
        }
        public virtual void RemoveGid(long gid)
        {
            if (!_childrenGMap.TryGetValue(gid, out SObject child))
                return;
            _childrenGMap.Remove(gid);
            if (child.rpc != 0)
                _childrenRMap.Remove(child.rpc);
            _children.Remove(child);
            child.Parent = null;
        }
        public virtual void RemoveRpc(long rpc)
        {
            if (rpc == 0)
            {
                Loger.Error("无效rpc");
                return;
            }
            if (!_childrenRMap.TryGetValue(rpc, out SObject child))
                return;
            _childrenGMap.Remove(child.gid);
            _childrenRMap.Remove(rpc);
            _children.Remove(child);
            child.Parent = null;
        }
        public void DisposeAllChildren() => this._disposeAllChildren(false);

        void _disposeAllChildren(bool _isDisposeSelf)
        {
            if (_isDisposeSelf)
            {
                _childrenGMap.Clear();
                ObjectPool<Dictionary<long, SObject>>.Return(_childrenGMap);
                _childrenGMap = null;

                _childrenRMap.Clear();
                ObjectPool<Dictionary<long, SObject>>.Return(_childrenRMap);
                _childrenRMap = null;

                List<SObject> tmp = _children;
                _children = null;
                for (int i = tmp.Count - 1; i >= 0; i--)
                {
                    if (!tmp[i].Disposed)
                        tmp[i].Dispose();
                }
                tmp.Clear();
                ObjectPool<List<SObject>>.Return(tmp);
            }
            else
            {
                if (_children.Count <= 0)
                    return;

                _childrenGMap.Clear();
                _childrenRMap.Clear();

                List<SObject> tmp = _children;
                _children = ObjectPool<List<SObject>>.Get();
                for (int i = tmp.Count - 1; i >= 0; i--)
                {
                    if (!tmp[i].Disposed)
                        tmp[i].Dispose();
                }
                tmp.Clear();
                ObjectPool<List<SObject>>.Return(tmp);
            }
        }
    }
}
