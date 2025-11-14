using System;
using System.Collections.Generic;
using System.Text;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Main
{
    [ExecuteInEditMode]
    public class WaterSystem : MonoBehaviour
    {
        public Vector2 waterSize = new Vector3(1000, 1000);
        public float meshDensity = 1f;

        MeshFilter mf;
        MeshRenderer mr;
        Mesh waterMesh;

#if UNITY_EDITOR
        private void OnValidate()
        {
            init();
        }
#endif
        private void OnEnable()
        {
            init();
        }
        private void OnDisable()
        {
            destroy();
        }

        void init()
        {
            destroy();

            if (!(mf = this.GetComponent<MeshFilter>()))
                mf = this.gameObject.AddComponent<MeshFilter>();
            if (!(mr = this.GetComponent<MeshRenderer>()))
                mr = this.gameObject.AddComponent<MeshRenderer>();

            waterMesh = new Mesh();
            int w = Mathf.CeilToInt(waterSize.x / meshDensity) + 1;
            int h = Mathf.CeilToInt(waterSize.y / meshDensity) + 1;
            Vector3 start = new Vector3(-waterSize.x / 2, 0, -waterSize.y / 2);
            Vector3[] verts = new Vector3[w * h];
            Vector2[] uv = new Vector2[w * h];
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    verts[y * w + x] = start + new Vector3(x * meshDensity, 0, y * meshDensity);
                    uv[y * w + x] = new Vector2((float)x / (w - 1), (float)y / (h - 1));
                }
            }
            int[] triangles = new int[(w - 1) * (h - 1) * 6];
            for (int x = 0; x < w - 1; x++)
            {
                for (int y = 0; y < h - 1; y++)
                {
                    triangles[(y * (w - 1) + x) * 6 + 0] = y * w + x;
                    triangles[(y * (w - 1) + x) * 6 + 1] = (y + 1) * w + x;
                    triangles[(y * (w - 1) + x) * 6 + 2] = (y + 1) * w + (x + 1);
                    triangles[(y * (w - 1) + x) * 6 + 3] = y * w + x;
                    triangles[(y * (w - 1) + x) * 6 + 4] = (y + 1) * w + (x + 1);
                    triangles[(y * (w - 1) + x) * 6 + 5] = y * w + (x + 1);
                }
            }
            waterMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            waterMesh.vertices = verts;
            waterMesh.triangles = triangles;
            waterMesh.uv = uv;
            mf.mesh = waterMesh;
        }
        void destroy()
        {
            if (waterMesh)
            {
                DestroyImmediate(waterMesh);
                waterMesh = null;
            }
        }
    }
}
