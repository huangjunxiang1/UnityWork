using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

[ExecuteInEditMode]
public class PathFindingAStar : MonoBehaviour
{
    public PathGridType gridType = PathGridType.Rect;
    public PathGridHexParityType hexParityType = PathGridHexParityType.Even;
    public PathGridHexFacingType hexFacingType = PathGridHexFacingType.Up;
    public float3 gridSize = new(1, 0, 1);
    public int2 aStarSize = new int2(10, 10);
    public string savePath = "Res/Config/raw/Map/AStarData/";
    public bool view = true;

    internal byte[] data;
    internal int2 dataSize;
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
    public void Load()
    {
        if (string.IsNullOrEmpty(savePath))
        {
            Debug.LogError("path is null");
            return;
        }
        var currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        var dir = $"{Application.dataPath}/{savePath.Split("Assets").LastOrDefault()}";
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);
        string path = $"{dir}/{currentScene.name}.bytes";
        if (File.Exists(path))
        {
            var buffer = new DBuffer(File.ReadAllBytes(path));
            gridType = (PathGridType)buffer.Readint();
            hexParityType = (PathGridHexParityType)buffer.Readint();
            hexFacingType = (PathGridHexFacingType)buffer.Readint();
            transform.position = buffer.Readfloat3();
            gridSize = buffer.Readfloat3();
            aStarSize = math.max(buffer.Readint2(), 1);
            data = buffer.Readbytes();
            buffer.Seek(0);
            astar = new(buffer);
        }
        else
        {
            data = new byte[aStarSize.x * aStarSize.y];
            astar = new(aStarSize.x, aStarSize.y, data, transform.position, gridSize);
        }
        dataSize = aStarSize;
    }
    private void OnEnable()
    {
        Load();
        this.View(view);
    }
    private void OnValidate()
    {
        this.View(view);
    }

#if UNITY_EDITOR
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
            if (!this.gameObject.GetComponent<BoxCollider>())
                this.gameObject.AddComponent<BoxCollider>();
#if UNITY_EDITOR
            if (!this.GetComponent<MeshRenderer>().sharedMaterial)
                this.GetComponent<MeshRenderer>().sharedMaterial = new Material((Material)UnityEditor.AssetDatabase.LoadMainAssetAtPath("Assets/Code/Main/Core/ECS/Components/PathFinding/AStar/res/AStarView_Mat.mat"));
#endif
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
        if (Application.isPlaying && astar == null)
        {
            astar = Game.Data?.Get<AStarData>(false);
            if (astar != null)
            {
                this.gridType = astar.gridType;
                this.hexParityType = astar.hexParityType;
                this.hexFacingType = astar.hexFacingType;
                this.transform.position = astar.start;
                this.gridSize = astar.gridSize;
                this.aStarSize = astar.size;
            }
        }

        if (astar == null) return;

        Mesh mesh = new Mesh();
        float3 size = gridType == PathGridType.Rect ? gridSize : gridSize * new float3((float2)1, 1.1f);
        Vector3[] verts = new Vector3[4]
        {
               size*new float3(-2,0,-2),
               size*new float3(astar.size.x + 2,0,-2),
               size*new float3(-2,0,astar.size.y + 2),
               size*new float3(astar.size.x + 2,0,astar.size.y + 2)
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
        var mat = this.GetComponent<MeshRenderer>().sharedMaterial;
        if (mat)
        {
            mat.SetVector("_StartPos", new float4(astar.start.xz, 0, 0));
            mat.SetVector("_GridSize", new float4(gridSize.xz, 0, 0));
            mat.SetVector("_Size", new float4(astar.size.xy, 0, 0));
            mat.SetBuffer("_Data", buffer);
            if (this.gridType == PathGridType.Rect)
                mat.EnableKeyword("GridType_Rect");
            else
            {
                mat.DisableKeyword("GridType_Rect");
                mat.SetFloat("_hexParityType", (int)hexParityType);
                mat.SetFloat("_hexFacingType", (int)hexFacingType);
            }
        }
        var box = this.gameObject.GetComponent<BoxCollider>();
        box.center = mesh.bounds.center;
        box.size = new Vector3(mesh.bounds.size.x * 2, 0.001f, mesh.bounds.size.z * 2);
    }
}
