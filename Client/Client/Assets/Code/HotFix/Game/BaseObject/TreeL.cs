using System.Collections.Generic;

namespace Game
{
    abstract class TreeL<T> : ObjectL  where T : TreeL<T>
    {
        public TreeL(long cid) : base(cid) { }
        public TreeL() : base() { }

        Dictionary<long, T> _childGMap = new();
        Dictionary<long, T> _childCMap = new();
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
                TreeL<T> parent = this.Parent;
                while (parent != null)
                {
                    layer++;
                    parent = this.Parent;
                }
                return layer;
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

            if (ConstDefM.Debug)
            {
                TreeL<T> o = this;
                do
                {
                    if (o == child)
                    {
                        Loger.Error("树节点循环");
                        return;
                    }
                } while ((o = o.Parent) != null);
            }

            if (child.Parent != null)
                child.Parent.Remove(child);

            _childGMap.Add(child.GID, child);
            if (child.CID > 0)
            {
                if (!_childCMap.ContainsKey(child.CID))
                    _childCMap[child.CID] = child;
                else
                    Loger.Error($"已经包含子对象 this={this.GetType()} cid={this.CID}  child={child.GetType()} cid={child.CID}");
            }
       
            _childLst.Add(child);
            child.Parent = (T)this;
        }

        /// <summary>
        /// 子获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetChildGID(long gid)
        {
            _childGMap.TryGetValue(gid, out T child);
            return child;
        }
        public T GetChildCID(long cid)
        {
            if (cid == 0)
            {
                Loger.Error("无效cid");
                return null;
            }
            _childCMap.TryGetValue(cid, out T child);
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
            _childGMap.Remove(child.GID);
            if (child.CID > 0)
                _childCMap.Remove(child.CID);
            _childLst.Remove(child);
            child.Parent = null;
        }
        public virtual void RemoveGID(long gid)
        {
            if (!_childGMap.TryGetValue(gid, out T child))
                return;
            _childGMap.Remove(gid);
            if (child.CID > 0)
                _childCMap.Remove(child.CID);
            _childLst.Remove(child);
            child.Parent = null;
        }
        public virtual void RemoveCID(long cid)
        {
            if (cid == 0)
            {
                Loger.Error("无效cid");
                return;
            }
            if (!_childCMap.TryGetValue(cid, out T child))
                return;
            _childGMap.Remove(child.GID);
            _childCMap.Remove(cid);
            _childLst.Remove(child);
            child.Parent = null;
        }
        public void DisposeAllChildren()
        {
            if (_childLst.Count <= 0)
                return;

            var lst = ToChildren();
            _childGMap.Clear();
            _childCMap.Clear();
            _childLst.Clear();
            for (int i = lst.Count - 1; i >= 0; i--)
                lst[i].Dispose();
        }
    }
}
