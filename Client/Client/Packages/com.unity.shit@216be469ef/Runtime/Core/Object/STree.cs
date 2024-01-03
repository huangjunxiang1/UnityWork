using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public abstract class STree<T> : SObject  where T : STree<T>
    {
        public STree(long rpc) : base(rpc) { }
        public STree() : base() { }

        Dictionary<long, T> _childGMap = new();
        Dictionary<long, T> _childRMap = new();
        List<T> _childLst = new();

        /// <summary>
        /// 父节点
        /// </summary>
        public T Parent { get; private set; }

        /// <summary>
        /// 根
        /// </summary>
        public T Root
        {
            get
            {
                T root = (T)this;
                while (root.Parent != null)
                    root = root.Parent;
                return root;
            }
        }

        /// <summary>
        /// 节点层级
        /// </summary>
        public int Layer
        {
            get
            {
                int layer = 0;
                STree<T> parent = this.Parent;
                while (parent != null)
                {
                    layer++;
                    parent = this.Parent;
                }
                return layer;
            }
        }

        /// <summary>
        /// 在父节点的下标
        /// </summary>
        public int SiblingIndex
        {
            get
            {
                if (Parent == null)
                    return 0;
                return Parent._childLst.IndexOf((T)this);
            }
        }

        public override void Dispose()
        {
            this.DisposeAllChildren();
            if (this.Parent != null)
                this.Parent.Remove((T)this);
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
            STree<T> o = this;
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

            _childGMap.Add(child.gid, child);
            if (child.rpc > 0)
            {
                if (!_childRMap.ContainsKey(child.rpc))
                    _childRMap[child.rpc] = child;
                else
                    Loger.Error($"已经包含子对象 this={this.GetType()} rpc={this.rpc}  child={child.GetType()} rpc={child.rpc}");
            }

            _childLst.Add(child);
            child.Parent = (T)this;
        }

        /// <summary>
        /// 子获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetChildGid(long gid)
        {
            _childGMap.TryGetValue(gid, out T child);
            return child;
        }
        public T GetChildRpc(long rpc)
        {
            if (rpc == 0)
            {
                Loger.Error("无效rpc");
                return null;
            }
            _childRMap.TryGetValue(rpc, out T child);
            return child;
        }
        public List<T> ToChildren()
        {
            List<T> lst = new(_childLst.Count);
            lst.AddRange(_childLst);
            return lst;
        }
        public List<T> GetChildren()
        {
            return _childLst;
        }

        /// <summary>
        /// 子移除
        /// </summary>
        /// <param name="child"></param>
        public void Remove(T child)
        {
            _childGMap.Remove(child.gid);
            if (child.rpc > 0)
                _childRMap.Remove(child.rpc);
            _childLst.Remove(child);
            child.Parent = null;
        }
        public virtual void RemoveGid(long gid)
        {
            if (!_childGMap.TryGetValue(gid, out T child))
                return;
            _childGMap.Remove(gid);
            if (child.rpc > 0)
                _childRMap.Remove(child.rpc);
            _childLst.Remove(child);
            child.Parent = null;
        }
        public virtual void RemoveRpc(long rpc)
        {
            if (rpc == 0)
            {
                Loger.Error("无效rpc");
                return;
            }
            if (!_childRMap.TryGetValue(rpc, out T child))
                return;
            _childGMap.Remove(child.gid);
            _childRMap.Remove(rpc);
            _childLst.Remove(child);
            child.Parent = null;
        }
        public void DisposeAllChildren()
        {
            if (_childLst.Count <= 0)
                return;

            var lst = ToChildren();
            _childGMap.Clear();
            _childRMap.Clear();
            _childLst.Clear();
            for (int i = lst.Count - 1; i >= 0; i--)
                lst[i].Dispose();
        }
    }
}
