using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;


#if Native
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
#endif

public enum PathFindingMethod
{
    AStar,//A星算法
    Breadth,//广度搜索
}
public enum PathFindingRound
{
    R4,//四方位
    R8,//八方位
}
public enum PathFindingSolve
{
    Best,//最优解
    Fast,//最快解
}
public enum PathGridType
{
    Rect,//矩形
    Hex,//六边形
}
public enum PathGridHexParityType
{
    [InspectorName("偶数行低位")]
    Even,//偶数行左偏
    [InspectorName("奇数行低位")]
    Odd,//奇数行左偏
}
public enum PathGridHexFacingType
{
    [InspectorName("尖角上下")]
    Up,//尖角上下
    [InspectorName("尖角左右")]
    Right,//尖角左右
}
public struct AStarGrid
{
    public int step;//步长

    public byte vs;//搜索标记
    public byte data;//每个值 低位第一个bit是 是否激活 后续bit是消耗
    public byte Occupation;//动态占用计数（单位站在上面）
    public byte PathOccupation;//路线占用  低4位是执行路径 高4位是遍历路径
}

public class AStarData
{
    public AStarData(int width, int height, byte[] data, float3 start, float3 size, 
        PathGridType gridType = PathGridType.Rect,
        PathGridHexParityType hexParityType = PathGridHexParityType.Even,
        PathGridHexFacingType hexFacingType = PathGridHexFacingType.Up)
    {
        if (data.Length < width * height)
        {
            Loger.Error(new IndexOutOfRangeException());
            return;
        }
        this.size = new(width, height);
        this.start = start;
        this.gridSize = size;
        this.gridType = gridType;
        this.hexParityType = hexParityType;
        this.hexFacingType = hexFacingType;

#if Native
        if (width > 0 && height > 0)
        {
            int len = width * height;
            this.data = new UnsafeList<AStarGrid>(len, AllocatorManager.Persistent);
            for (int i = 0; i < len; i++)
            {
                var item = new AStarGrid();
                item.data = data[i];
                this.data.Add(item);
            }
        }
#else
        this.data = new AStarGrid[width * height];
        for (int i = 0; i < data.Length; i++)
            this.data[i].data = data[i];
#endif
    }
    public AStarData(DBuffer buffer)
    {
        this.gridType = (PathGridType)buffer.Readint();
        this.hexParityType = (PathGridHexParityType)buffer.Readint();
        this.hexFacingType = (PathGridHexFacingType)buffer.Readint();
        this.start = buffer.Readfloat3();
        this.gridSize = buffer.Readfloat3();
        this.size = buffer.Readint2();
        buffer.Readint();

#if Native
        if (size.x > 0 && size.y > 0)
        {
            int len = size.x * size.y;
            this.data = new UnsafeList<AStarGrid>(len, AllocatorManager.Persistent);
            for (int i = 0; i < len; i++)
            {
                var item = new AStarGrid();
                item.data = buffer.Readbyte();
                this.data.Add(item);
            }
        }
#else
        this.data = new AStarGrid[size.x * size.y];
        for (int i = 0; i < data.Length; i++)
            this.data[i].data = buffer.Readbyte();
#endif
    }

    public static readonly AStarData Empty = new AStarData(0, 0, Array.Empty<byte>(), 0, 1);

    public int2 size { get; private set; }
#if Native
    public UnsafeList<AStarGrid> data;
#else
    public AStarGrid[] data { get; private set; }
#endif

    public PathGridType gridType { get; private set; } = PathGridType.Rect;
    public PathGridHexParityType hexParityType { get; private set; } = PathGridHexParityType.Even;
    public PathGridHexFacingType hexFacingType { get; private set; } = PathGridHexFacingType.Right;
    public float3 start { get; private set; }//起始坐标
    public float3 gridSize { get; private set; } = new float3(1, 0, 1);//块间隔

    internal byte vs;
    internal bool isFinding;
#if UNITY_EDITOR
    internal Action<int2> gridChange;
    internal Action change;
#endif
    int2[] array;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AddOccupation(int2 xy)
    {
        this.data.ElementAt(xy.y * size.x + xy.x).Occupation++;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void RemoveOccupation(int2 xy)
    {
        this.data.ElementAt(xy.y * size.x + xy.x).Occupation--;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetPathOccupation(int2 xy, bool value, bool isPath = true)
    {
        var grid = this.data[xy.y * size.x + xy.x];
        if (isPath)
        {
            if (value)
                grid.PathOccupation++;
            else
                grid.PathOccupation--;
        }
        else
        {
            if (value)
                grid.PathOccupation += (1 << 4);
            else
                grid.PathOccupation -= (1 << 4);
        }
        this.data[xy.y * size.x + xy.x] = grid;
    }
    public void GridChangeHandle(int2 xy)
    {
#if UNITY_EDITOR
        gridChange?.Invoke(xy);
#endif
    }
    public void ChangeHandle()
    {
#if UNITY_EDITOR
        change?.Invoke();
#endif
    }

    public float3 GetPosition(int2 xy) => start + gridSize * new float3(xy.x, 0, xy.y) + gridSize / 2;
    public int2 GetXY(float3 position) => ((int3)((position - start) / gridSize)).xz;
    public bool isEnable(int2 xy)
    {
        int index = xy.y * size.x + xy.x;
        return (data[index].data & 1) == 1 && data[index].Occupation == 0;
    }
    public bool isEnableExceptSelfVolume(int2 xy, AStarVolume volume, int2 self)
    {
        int index = xy.y * size.x + xy.x;
        return (data[index].data & 1) == 1 && (data[index].Occupation == 0 || (volume.isInScope(self, xy) && data[index].Occupation == 1));
    }
    public bool isEnable(int index) => (data[index].data & 1) == 1 && data[index].Occupation == 0;
    public bool isInScope(int2 xy) => xy.x >= 0 && xy.y >= 0 && xy.x < size.x && xy.y < size.y;
    public bool FindTarget(Func<int2, bool> func, int2 origin, out int2 value, PathFindingRound r = PathFindingRound.R4)
    {
        value = origin;
        if (func(origin))
            return true;
        if (this.isFinding)
        {
            Loger.Error("cannot finding in mul thread");
            return false;
        }
        this.isFinding = true;
        this.CheckVersionValue();
        ++vs;
        int currentIndex = 0;
        int index = 0;
        array ??= new int2[this.size.x * this.size.y];
        array[index++] = origin;
        data.ElementAt(origin.y * size.x + origin.x).vs = vs;

        bool ret = false;
        do
        {
            var v2 = array[currentIndex];

            if (v2.x > 0)
            {
                if (breadth(func, ref index, new int2(v2.x - 1, v2.y), out value))
                {
                    ret = true;
                    break;
                }
            }
            if (v2.x < size.x - 1)
            {
                if (breadth(func, ref index, new int2(v2.x + 1, v2.y), out value))
                {
                    ret = true;
                    break;
                }
            }
            if (v2.y > 0)
            {
                if (breadth(func, ref index, new int2(v2.x, v2.y - 1), out value))
                {
                    ret = true;
                    break;
                }
            }
            if (v2.y < size.y - 1)
            {
                if (breadth(func, ref index, new int2(v2.x, v2.y + 1), out value))
                {
                    ret = true;
                    break;
                }
            }

            if (r == PathFindingRound.R8)
            {
                if (v2.x > 0 && v2.y > 0)
                {
                    if (breadth(func, ref index, new int2(v2.x - 1, v2.y - 1), out value))
                    {
                        ret = true;
                        break;
                    }
                }
                if (v2.x > 0 && v2.y < size.y - 1)
                {
                    if (breadth(func, ref index, new int2(v2.x - 1, v2.y + 1), out value))
                    {
                        ret = true;
                        break;
                    }
                }
                if (v2.x < size.x - 1 && v2.y > 0)
                {
                    if (breadth(func, ref index, new int2(v2.x + 1, v2.y - 1), out value))
                    {
                        ret = true;
                        break;
                    }
                }
                if (v2.x < size.x - 1 && v2.y < size.y - 1)
                {
                    if (breadth(func, ref index, new int2(v2.x + 1, v2.y + 1), out value))
                    {
                        ret = true;
                        break;
                    }
                }
            }

            currentIndex++;
        } while (currentIndex < index);
        this.isFinding = false;
        return ret;
    }
    public void CheckVersionValue()
    {
        if (vs == byte.MaxValue)
        {
            vs = 0;
            for (int i = 0; i < data.Length; i++)
            {
                data.ElementAt(i).vs = 0;
                data.ElementAt(i).step = 0;
            }
        }
    }

    bool breadth(Func<int2, bool> func, ref int index, int2 xy, out int2 value)
    {
        value = xy;
        if (data[xy.y * size.x + xy.x].vs == vs)
            return false;
        data.ElementAt(xy.y * size.x + xy.x).vs = vs;
        if (!isEnable(xy))
            return false;
        bool isTarget = func(xy);
        if (!isTarget)
            array[index++] = xy;
        return isTarget;
    }

    public void Dispose()
    {
#if Native
        data.Dispose();
#endif
    }
    ~AStarData()
    {
        this.Dispose();
    }
}
