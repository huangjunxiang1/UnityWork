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
        bool view;
        GraphicsBuffer buffer;
        [NonSerialized]
        public AStarData astar;
        int[] cost;
        int[] tempSet = new int[1];

        void change()
        {
            if (cost == null || cost.Length != astar.size.x * astar.size.y)
                cost = new int[astar.size.x * astar.size.y];
            if (buffer == null || buffer.count != astar.size.x * astar.size.y)
            {
                buffer?.Dispose();
                buffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, astar.size.x * astar.size.y, 4);
            }
            for (int i = 0; i < astar.data.Length; i++)
            {
                var b = astar.data[i];
                cost[i] = data[i];
                cost[i] |= b.Occupation << 8;
                cost[i] |= b.PathOccupation << 16;
            }
            buffer.SetData(cost);
        }
#if UNITY_EDITOR
        private void OnEnable()
        {
            this.View(view);
        }
        void gridChange(int2 xy)
        {
            int index = xy.y * astar.size.x + xy.x;
            tempSet[0] = astar.data[index].data | (astar.data[index].Occupation << 8) | (astar.data[index].PathOccupation << 16);
            buffer.SetData(tempSet, 0, index, 1);
        }
#endif
        private void OnDisable()
        {
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
                if (!this.gameObject.GetComponent<MeshFilter>())
                    this.gameObject.AddComponent<MeshFilter>();
                if (!this.gameObject.GetComponent<MeshRenderer>())
                    this.gameObject.AddComponent<MeshRenderer>();
                Init();
            }
            else
            {
                var mesh = this.gameObject.GetComponent<MeshFilter>();
                if (mesh && mesh.sharedMesh)
                    GameObject.DestroyImmediate(mesh.sharedMesh);
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
               new float3(astar.gridSize.x * astar.size.x,0,0),
               new float3(0,0,astar.gridSize.z * astar.size.y),
               new float3(astar.gridSize.x * astar.size.x,0,astar.gridSize.z * astar.size.y)
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
            this.GetComponent<MeshFilter>().sharedMesh = mesh;
            var mat = GameObject.Instantiate(Resources.Load<Material>("Shit/AStarView_Mat"));
            mat.SetVector("_Size", new Vector4(astar.size.x, astar.size.y, 0, 0));
            mat.SetBuffer("_Data", buffer);
            this.GetComponent<MeshRenderer>().sharedMaterial = mat;
            var box = this.gameObject.GetComponent<BoxCollider>();
            if (!box)
                box = this.gameObject.AddComponent<BoxCollider>();
            box.center = mesh.bounds.center;
            box.size = new Vector3(mesh.bounds.size.x * 2, 0.001f, mesh.bounds.size.z * 2);
        }
    }
}
