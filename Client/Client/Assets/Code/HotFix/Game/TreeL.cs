using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    abstract class TreeL<T> : EntityL  where T : TreeL<T>
    {
        public TreeL(long id) : base(id) { }
        public TreeL() : base() { }

        Dictionary<long, T> _childMap = new();
        List<T> _childLst = new();

        /// <summary>
        /// 父节点
        /// </summary>
        public T Parent { get; private set; }

        public override void Dispose()
        {
            this.RemoveAllChildren();
            if (this.Parent != null)
                this.Parent.Remove(this.ID);
            base.Dispose();
        }

        /// <summary>
        /// 设置父管理节点
        /// </summary>
        /// <param name="parent"></param>
        public void SetParent(T parent)
        {
            if (this.Parent == parent)
                return;

            if (this.Parent != null)
                this.Parent.Remove(this.ID);
            if (parent != null)
                parent.AddChild((T)this);
        }

        /// <summary>
        /// 子添加
        /// </summary>
        /// <param name="child"></param>
        public virtual void AddChild(T child)
        {
            if (child.Parent == this)
                return;
            if (child.Parent != null)
                child.Parent.Remove(child.ID);
            _childMap.Add(child.ID, child);
            _childLst.Add(child);
            child.Parent = (T)this;
        }

        public virtual void AddChild<K>() where K : T, new()
        {
            K child = new();
            this.AddChild(child);
        }


        /// <summary>
        /// 子获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetChild(long id)
        {
            _childMap.TryGetValue(id, out T child);
            return child;
        }
        public List<T> GetChildren()
        {
            List<T> lst = new(_childLst.Count);
            lst.AddRange(_childLst);
            return lst;
        }

        /// <summary>
        /// 子移除
        /// </summary>
        /// <param name="child"></param>
        public void Remove(T child)
        {
            Remove(child.ID);
        }
        public virtual void Remove(long id)
        {
            if (!_childMap.TryGetValue(id, out T child))
                return;
            _childMap.Remove(id);
            _childLst.Remove(child);
            child.Parent = null;
        }
        public void RemoveAllChildren()
        {
            if (_childLst.Count <= 0)
                return;

            var lst = GetChildren();
            for (int i = lst.Count - 1; i >= 0; i--)
                lst[i].Dispose();
            _childMap.Clear();
            _childLst.Clear();
        }
    }
}
