using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

namespace Game
{
    [ExecuteInEditMode]
    public class PathFindingAStar : MonoBehaviour
    {
        public float3 size = new(1, 0, 1);
        public int2 aStarSize = new int2(10, 10);
        public string savePath = "Res/Config/raw/Map/AStarData/";

        internal byte[] data;
        internal int2 dataSize;
        GameObject quad;
        Vector3 point;
        bool view;
        GraphicsBuffer buffer;
        AStarData astar;
        int[] cost;

#if UNITY_EDITOR
        private void OnEnable()
        {
            this.View(view);
        }
        void change(int2 xy)
        {
            int index = xy.y * astar.width + xy.x;
            cost[0] = astar.data[index] | (astar.Occupation[index] << 8);
            buffer.SetData(cost, 0, index, 1);
        }
        private void OnValidate()
        {
            this.View(view);
        }
        private void Update()
        {
            if (quad)
            {
                if (Vector3.Distance(this.point, this.transform.position) > 0.001f)
                    this.point = quad.transform.position = this.transform.position;
            }
        }
#endif
        private void OnDisable()
        {
            if (quad)
                GameObject.DestroyImmediate(quad);
            if (buffer != null)
            {
                buffer.Dispose();
                buffer = null;
            }
#if UNITY_EDITOR
            if (astar != null)
                astar.occupationChange -= change;
#endif
        }

        public void View(bool view)
        {
            this.view = view;
            if (view && this.enabled && this.gameObject.activeSelf)
            {
                if (!quad)
                {
                    quad = new GameObject("", typeof(MeshFilter), typeof(MeshRenderer));
                    quad.hideFlags = HideFlags.HideAndDontSave;
                }
                Init();
                this.point = quad.transform.position = this.transform.position;
            }
            else
            {
                if (quad)
                    GameObject.DestroyImmediate(quad);
                quad = null;
            }
        }
        void Init()
        {
            Mesh mesh = new Mesh();
            Vector3[] verts = new Vector3[4]
            {
               (float3)0f,
               new float3(size.x * aStarSize.x,0,0),
               new float3(0,0,size.z * aStarSize.y),
               new float3(size.x * aStarSize.x,0,size.z * aStarSize.y)
            };
            mesh.vertices = verts;
            mesh.triangles = new int[] { 0, 2, 1, 1, 2, 3 };
            mesh.uv = new Vector2[4]
            {
               new Vector2(0,0),
               new Vector2(1,0),
               new Vector2(0,1),
               new Vector2(1,1),
            };

#if UNITY_EDITOR
            if (astar != null)
                astar.occupationChange -= change;
#endif
            astar = Client.Data?.Get<AStarData>(false);
#if UNITY_EDITOR
            if (astar != null)
                astar.occupationChange += change;
#endif

            if (cost == null || cost.Length != aStarSize.x * aStarSize.y)
                cost = new int[aStarSize.x * aStarSize.y];
            for (int i = 0; i < data.Length; i++)
            {
                var b = data[i];
                var oc = (astar != null && i < astar.Occupation.Length) ? astar.Occupation[i] : 0;
                cost[i] = b;
                cost[i] |= oc << 8;
            }

            // 应用 Mesh
            quad.GetComponent<MeshFilter>().mesh = mesh;
            var r = quad.GetComponent<MeshRenderer>();
            var mat = GameObject.Instantiate(Resources.Load<Material>("Shit/AStarView_Mat"));
            mat.SetVector("_Size", new Vector4(aStarSize.x, aStarSize.y, 0, 0));
            if (buffer == null || buffer.count != aStarSize.x * aStarSize.y)
            {
                buffer?.Dispose();
                buffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, aStarSize.x * aStarSize.y, 4);
            }
            buffer.SetData(cost);
            mat.SetBuffer("_Cost", buffer);
            r.sharedMaterial = mat;
            var box = quad.AddComponent<BoxCollider>();
            box.center = mesh.bounds.center;
            box.size = new Vector3(mesh.bounds.size.x * 2, 0.001f, mesh.bounds.size.z * 2);
        }
    }
}
