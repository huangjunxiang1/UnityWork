using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    [ExecuteInEditMode]
    public class PathFindingNode : MonoBehaviour
    {
        public static MulNode MulRoot;
        static HashSet<PathFindingNode> Nodes = new();

        public long id;
        public List<PathFindingNode> Nexts = new();

        private void OnEnable()
        {
            Nodes.Add(this);
        }
        private void OnDisable()
        {
            Nodes.Remove(this);
        }
        private void OnValidate()
        {
            MulRoot = null;
        }
#if UNITY_EDITOR
        private Vector3 lastPosition;
        private void Update()
        {
            if (Vector3.Distance(transform.position, lastPosition) > 0.0001f)
            {
                lastPosition = transform.position;
                OnValidate();
            }
        }
#endif
        public static MulNode GetCurrentRoot()
        {
            if (MulRoot == null)
            {
                MulRoot = new MulNode();
                foreach (var item in Nodes)
                    MulRoot.CreateNode(item.id == 0 ? (item.id = Util.RandomLong()) : item.id, item.transform.position);
                foreach (var item in Nodes)
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
