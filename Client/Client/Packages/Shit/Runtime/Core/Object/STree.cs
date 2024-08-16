using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class STree : SObject
    {
        [ShowInInspector]
        Dictionary<long, SObject> _childrenGMap = ObjectPool.Get<Dictionary<long, SObject>>();
        [ShowInInspector]
        Dictionary<long, SObject> _childrenRMap = ObjectPool.Get<Dictionary<long, SObject>>();
        [ShowInInspector]
        Dictionary<int, List<SObject>> _typeMap = ObjectPool.Get<Dictionary<int, List<SObject>>>();
        internal List<SObject> _children = new();//外部能访问到的对象不用池

        /// <summary>
        /// 关键节点
        /// </summary>
        [ShowInInspector]
        [PropertyOrder(-100)]
        public bool isCrucialRoot { get; init; }

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
            if (this.World == null)
            {
                Loger.Error($"{this} Not Add To Any World");
                return;
            }
            if (child.Parent == this)
                return;

#if DebugEnable
            SObject o = this;
            do
            {
                if (o == child)
                {
                    Loger.Error("STree Node Loop");
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
                    Loger.Error($"Already Contains Child this={this} child={child}");
            }
            if (child.objType != 0)
            {
                if (!this._typeMap.TryGetValue(child.objType, out var lst))
                    this._typeMap[child.objType] = lst = new();
                lst.Add(child);
            }
            _children.Add(child);

            child.Parent = this;
            child.World = this.World;
            child.View = this.View;
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
        public List<SObject> GetChildrenByObjType(int objType)
        {
            _typeMap.TryGetValue(objType, out var lst);
            return lst;
        }
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
            if (child.objType != 0)
            {
                if (this._typeMap.TryGetValue(child.objType, out var lst))
                    lst.Remove(child);
            }
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

                _typeMap.Clear();
                ObjectPool.Return(_typeMap);
                _typeMap = null;

                List<SObject> tmp = _children;
                _children = null;
                for (int i = tmp.Count - 1; i >= 0; i--)
                {
                    if (!tmp[i].Disposed && tmp[i].Parent == this)
                    {
                        tmp[i].Parent = null;
                        tmp[i].Dispose();
                    }
                }
            }
            else
            {
                if (_children.Count <= 0)
                    return;

                _childrenGMap.Clear();
                _childrenRMap.Clear();
                _typeMap.Clear();

                List<SObject> tmp = _children;
                _children = new();
                for (int i = tmp.Count - 1; i >= 0; i--)
                {
                    if (!tmp[i].Disposed && tmp[i].Parent == this)
                        tmp[i].Dispose();
                }
            }
        }
    }
}
