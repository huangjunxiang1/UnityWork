using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

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
        Texture2D texture;
        Vector3 point;
        bool view;

#if UNITY_EDITOR
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
            if (texture)
                GameObject.DestroyImmediate(texture);
            if (quad)
                GameObject.DestroyImmediate(quad);
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

            if (texture)
                texture.Reinitialize(aStarSize.x, aStarSize.y);
            else
                texture = new Texture2D(aStarSize.x, aStarSize.y);
            texture.filterMode = FilterMode.Point;
            for (int i = 0; i < data.Length; i++)
            {
                var b = data[i];
                texture.SetPixel(i % aStarSize.x, i / aStarSize.x, ((b & 1) == 0) ? new Color(1, 0, 0, 0.5f) : new Color(0, 1, 0, 0.5f));
            }
            texture.Apply();

            // 应用 Mesh
            quad.GetComponent<MeshFilter>().mesh = mesh;
            var r = quad.GetComponent<MeshRenderer>();
            var mat = GameObject.Instantiate(Resources.Load<Material>("Shit/AStarView_Mat"));
            mat.SetTexture("_Mask", texture);
            mat.SetVector("_Size", new Vector4(aStarSize.x, aStarSize.y, 0, 0));
            r.sharedMaterial = mat;
            var box = quad.AddComponent<BoxCollider>();
            box.center = mesh.bounds.center;
            box.size = new Vector3(mesh.bounds.size.x * 2, 0.001f, mesh.bounds.size.z * 2);
        }
    }
}
