using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;


public class AStarData
{
    public AStarData(int width, int height, byte[] data, float3 start, float3 size)
    {
        if (data.Length < width * height)
        {
            Loger.Error(new IndexOutOfRangeException());
            return;
        }
        this.width = width;
        this.height = height;
        this.data = data;
        this.start = start;
        this.size = size;

        this.vsArray = new byte[width * height];
        this.Occupation = new byte[width * height];
    }
    public AStarData(DBuffer buffer)
    {
        this.start = buffer.Readfloat3();
        this.size = buffer.Readfloat3();
        int2 wh = buffer.Readint2();
        this.width = wh.x;
        this.height = wh.y;
        this.data = buffer.Readbytes();

        this.vsArray = new byte[width * height];
        this.Occupation = new byte[width * height];
    }

    public int width { get; private set; }
    public int height { get; private set; }
    public byte[] data { get; private set; }//每个值 低位第一个bit是 是否激活 后续bit是消耗
    public byte[] Occupation { get; private set; }//动态占用计数（单位站在上面）

    public float3 start { get; private set; }//起始坐标
    public float3 size { get; private set; } = new float3(1, 0, 1);//块间隔

    internal byte vs;
    internal byte[] vsArray;
    internal bool isFinding;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AddOccupation(int2 xy)
    {
        Occupation[xy.y * width + xy.x]++;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void RemoveOccupation(int2 xy)
    {
        Occupation[xy.y * width + xy.x]--;
    }
    public float3 GetPosition(int2 xy) => start + size * new float3(xy.x, 0, xy.y) + size / 2;
    public int2 GetXY(float3 position) => ((int3)((position - start) / size)).xz;
    public bool isEnable(int2 xy)
    {
        int index = xy.y * width + xy.x;
        return (data[index] & 1) == 1 && Occupation[index] == 0;
    }
    public bool isEnable(int index) => (data[index] & 1) == 1 && Occupation[index] == 0;
    public bool isEnable(int2 xy, AStarVolume volume, int2 self)
    {
        int index = xy.y * width + xy.x;
        return (data[index] & 1) == 1 && (Occupation[index] == 0 || xy.Equals(self) || (volume.isInScope(self, xy) && Occupation[index] == 1));
    }
    public bool isInScope(int2 xy) => xy.x >= 0 && xy.y >= 0 && xy.x < width && xy.y < height;
}
