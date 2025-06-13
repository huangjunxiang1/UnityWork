using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    [ExecuteInEditMode]
    class PathFindingNode : MonoBehaviour
    {
        static PathFindingNode()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public static MulNode MulRoot;
        static HashSet<PathFindingNode> Nodes = new();

        public long id;
        public List<PathFindingNode> Nexts = new();

        private void OnEnable()
        {
            Nodes.Add(this);
            if (Application.isEditor)
                Clear();
        }
        private void OnDisable()
        {
            Nodes.Remove(this);
            if (Application.isEditor)
                Clear();
        }
        private void OnValidate()
        {
            if (Application.isEditor)
            {
                this.gameObject.name = this.id.ToString();
                Clear();
            }
        }

#if UNITY_EDITOR
        private Vector3 lastPosition;
        private void Update()
        {
            if (Vector3.Distance(transform.position, lastPosition) > 0.0001f)
            {
                lastPosition = transform.position;
                Clear();
            }
        }
#endif

        static void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
        {
            Clear();
        }
        public static void Clear()
        {
            MulRoot = null;
        }

        public static MulNode GetCurrent()
        {
            if (MulRoot == null)
            {
                MulRoot = new MulNode();
                foreach (var item in Nodes)
                    MulRoot.CreateNode((item.id == 0 || MulRoot.Nodes.ContainsKey(item.id)) ? (item.id = Util.RandomLong()) : item.id, item.transform.position);
                foreach (var item in Nodes)
                {
                    var n = MulRoot.GetNode(item.id);
                    for (int i = 0; i < item.Nexts.Count; i++)
                    {
                        var next = MulRoot.GetNode(item.Nexts[i].id);
                        var d = Vector3.Distance(item.transform.position, item.Nexts[i].transform.position);
                        n.AddNext(next.id, d);
                        next.AddNext(n.id, d);
                    }
                }
            }
            return MulRoot;
        }
    }
}
