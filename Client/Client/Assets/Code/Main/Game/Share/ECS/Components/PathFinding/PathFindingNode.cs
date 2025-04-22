using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public class PathFindingNode : MonoBehaviour
    {
        public static PathFindingNode Root;
        static MulNode MulRoot;

        public bool isRoot = false;
        public long id;
        public List<PathFindingNode> Nexts = new();

        private void OnEnable()
        {
            if (this.isRoot)
                Root = this;
        }
        private void OnDisable()
        {
            if (this.isRoot)
            {
                Root = null;
                MulRoot = null;
            }
        }
        public static MulNode GetCurrentRoot()
        {
            if (!Root) return null;
            if (MulRoot == null)
            {
                MulRoot = new MulNode();
                HashSet<PathFindingNode> nodes = new();
                List<PathFindingNode> lst = new();
                nodes.Add(Root);
                lst.Add(Root);
                int index = 0;
                while (index < lst.Count)
                {
                    var n = lst[index];
                    for (int i = 0; i < n.Nexts.Count; i++)
                    {
                        var next = n.Nexts[i];
                        if (nodes.Contains(next))
                            continue;
                        nodes.Add(next);
                        lst.Add(next);
                    }
                    ++index;
                }
                foreach (var item in nodes)
                    MulRoot.CreateNode(item.id == 0 ? (item.id = Util.RandomLong()) : item.id, item.transform.position);
                foreach (var item in nodes)
                {
                    var nn = MulRoot.GetNode(item.id);
                    for (int i = 0; i < item.Nexts.Count; i++)
                    {
                        var next = MulRoot.GetNode(item.Nexts[i].id);
                        var d = Vector3.Distance(item.transform.position, item.Nexts[i].transform.position);
                        nn.AddNext(next.id, d);
                        next.AddNext(nn.id, d);
                    }
                }
            }
            return MulRoot;
        }
    }
}
