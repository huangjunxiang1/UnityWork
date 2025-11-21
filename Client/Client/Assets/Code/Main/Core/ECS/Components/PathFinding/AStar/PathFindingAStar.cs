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
        public AStarData astar;
        int[] cost;
        int[] tempSet = new int[1];

#if UNITY_EDITOR
        private void OnEnable()
        {
            this.View(view);
        }
        void gridChange(int2 xy)
        {
            int index = xy.y * astar.width + xy.x;
            tempSet[0] = astar.data[index].data | (astar.data[index].Occupation << 8) | (astar.data[index].PathOccupation << 16);
            buffer.SetData(tempSet, 0, index, 1);
        }
        void change()
        {
            if (cost == null || cost.Length != astar.width * astar.height)
                cost = new int[astar.width * astar.height];
            if (buffer == null || buffer.count != astar.width * astar.height)
            {
                buffer?.Dispose();
                buffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, astar.width * astar.height, 4);
            }
            for (int i = 0; i < astar.data.Length; i++)
            {
                var b = astar.data[i];
                cost[i] = b.data;
                cost[i] |= b.Occupation << 8;
                cost[i] |= b.PathOccupation << 16;
            }
            buffer.SetData(cost);
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
            {
                astar.gridChange -= gridChange;
                astar.change -= change;
            }
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
#if UNITY_EDITOR
            if (astar != null)
            {
                astar.gridChange -= gridChange;
                astar.change -= change;
            }
#endif
            if (Application.isPlaying)
                astar = Client.Data?.Get<AStarData>(false);
            if (astar == null) return;

            Mesh mesh = new Mesh();
            Vector3[] verts = new Vector3[4]
            {
               (float3)0f,
               new float3(astar.size.x * astar.width,0,0),
               new float3(0,0,astar.size.z * astar.height),
               new float3(astar.size.x * astar.width,0,astar.size.z * astar.height)
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
            {
                astar.gridChange += gridChange;
                astar.change += change;
            }
#endif

            change();

            // 应用 Mesh
            quad.GetComponent<MeshFilter>().mesh = mesh;
            var r = quad.GetComponent<MeshRenderer>();
            var mat = GameObject.Instantiate(Resources.Load<Material>("Shit/AStarView_Mat"));
            mat.SetVector("_Size", new Vector4(astar.width, astar.height, 0, 0));
            mat.SetBuffer("_Data", buffer);
            r.sharedMaterial = mat;
            var box = quad.AddComponent<BoxCollider>();
            box.center = mesh.bounds.center;
            box.size = new Vector3(mesh.bounds.size.x * 2, 0.001f, mesh.bounds.size.z * 2);
        }
    }
}
