using Newtonsoft.Json.Linq;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Unity.Mathematics;
using UnityEngine;

class WorldData
{
    static string key;
    static string path = $"{Application.persistentDataPath}/sdata/world_data_{{0}}.bytes";
    const int newvs = 0 ;
    public static WorldData Inst { get; private set; } = new();

    public int seed;

    public int vs;//版本号
    public float2 offset;

    public Dictionary<int2, WorldQuadData> quad = new();

    public static WorldData GetOrCreate(string key)
    {
        WorldData.key = key;
         var p = string.Format(path, WorldData.key);
        if (File.Exists(p))
            Inst.Read(new DBuffer(new MemoryStream(File.ReadAllBytes(p))));
        else
            Inst.Range(Util.RandomInt());
        return Inst;
    }
    void Read(DBuffer buffer)
    {
        this.seed = buffer.Readint();
        var r = new System.Random(seed);
        this.vs = buffer.Readint();
        if (this.vs != newvs)
        {
            Box.Tips("数据版本号已过时".ToLanx());
            return;
        }
        this.offset = buffer.Readfloat2();
    }
    void Range(int seed)
    {
        var r = new System.Random(seed);

        this.seed = seed;
        this.vs = newvs;
        this.offset = new float2((float)r.NextDouble() * 1000f, (float)r.NextDouble() * 1000f);
    }
    public void Save()
    {
        DBuffer buffer = new(10000);
        buffer.Write(this.seed);
        buffer.Write(this.vs);
        buffer.Write(this.offset);
        File.WriteAllBytes(string.Format(path, WorldData.key), buffer.ToBytes());
    }
    public float FractalNoise(float2 position, float lacunarity = 2.0f, float persistence = 0.5f)
    {
        position += this.offset;
        float value = 0f;
        float amplitude = 1f;
        float frequency = 0.01f;

        int octaves = 6; // 迭代次数
        for (int i = 0; i < octaves; i++)
        {
            // 核心：叠加基础噪声（这里以snoise为例）
            value += amplitude * noise.snoise(position * frequency);

            // 频率增加（控制细节密度），振幅减小（控制细节强度）
            frequency *= lacunarity;
            amplitude *= persistence;
        }

        return math.max(value, 0) * 2;
    }
    public bool Logging(int2 xy)
    {
        int2 center = Hex.GetCenterGrid(xy);
        if (!quad.TryGetValue(center, out var value))
            quad[center] = value = new();
        int2 qxy = Hex.GetQuadLocalxy(xy);
        int index = qxy.y * Hex.QuadSize.x + qxy.x;

        uint mask = ((uint)1) << (index % 32);
        bool ret = (value.tree_logging[index / 32] & mask) == 0;
        if (ret)
            value.tree_logging[index / 32] |= mask;
        return ret;
    }
    public void CopyVisibleToGraphicsBuffer(GraphicsBuffer buffer, int offset, int2 xy)
    {
        int2 center = Hex.GetCenterGrid(xy);
        int len = (Hex.GridCount - 1) / 32 + 1;
        uint[] visible = ArrayPool<uint>.Shared.Rent(len * 9);
        for (int j = -1; j < 2; j++)
        {
            for (int i = -1; i < 2; i++)
            {
                int2 cc = center + new int2(i, j) * Hex.QuadSize;
                int2 quadxy = new(i + 1, j + 1);
                int index = (quadxy.y * 3 + quadxy.x) * len;
                if (quad.TryGetValue(cc, out var value))
                {
                    Array.Copy(value.tree_logging, 0, visible, index, value.tree_logging.Length);
                }
                else
                {
                    Array.Clear(visible, index, len);
                }
            }
        }
        buffer.SetData(visible, 0, offset, len * 9);
        ArrayPool<uint>.Shared.Return(visible);
    }
}
class WorldQuadData
{
    public uint[] tree_logging = new uint[(Hex.GridCount - 1) / 32 + 1];
    public Dictionary<int2, WorldGridData> grids = new();
}
class WorldGridData
{

}