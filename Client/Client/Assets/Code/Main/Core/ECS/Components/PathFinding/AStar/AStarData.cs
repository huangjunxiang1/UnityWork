using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;

#if Native
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
#endif


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
    public AStarData(int width, int height, byte[] data, float3 start, float3 size)
    {
        if (data.Length < width * height)
        {
            Loger.Error(new IndexOutOfRangeException());
            return;
        }
        this.width = width;
        this.height = height;
        this.start = start;
        this.size = size;

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
        this.start = buffer.Readfloat3();
        this.size = buffer.Readfloat3();
        int2 wh = buffer.Readint2();
        this.width = wh.x;
        this.height = wh.y;
        buffer.Readint();

#if Native
        if (width > 0 && height > 0)
        {
            int len = width * height;
            this.data = new UnsafeList<AStarGrid>(len, AllocatorManager.Persistent);
            for (int i = 0; i < len; i++)
            {
                var item = new AStarGrid();
                item.data = buffer.Readbyte();
                this.data.Add(item);
            }
        }
#else
        this.data = new AStarGrid[width * height];
        for (int i = 0; i < data.Length; i++)
            this.data[i].data = buffer.Readbyte();
#endif
    }

    public static readonly AStarData Empty = new AStarData(0, 0, Array.Empty<byte>(), 0, 1);

    public int width { get; private set; }
    public int height { get; private set; }
#if Native
    public UnsafeList<AStarGrid> data;
#else
    public AStarGrid[] data { get; private set; }
#endif

    public float3 start { get; private set; }//起始坐标
    public float3 size { get; private set; } = new float3(1, 0, 1);//块间隔

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
        this.data.ElementAt(xy.y * width + xy.x).Occupation++;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void RemoveOccupation(int2 xy)
    {
        this.data.ElementAt(xy.y * width + xy.x).Occupation--;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetPathOccupation(int2 xy, bool value, bool isPath = true)
    {
        var grid = this.data[xy.y * width + xy.x];
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
        this.data[xy.y * width + xy.x] = grid;
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

    public float3 GetPosition(int2 xy) => start + size * new float3(xy.x, 0, xy.y) + size / 2;
    public int2 GetXY(float3 position) => ((int3)((position - start) / size)).xz;
    public bool isEnable(int2 xy)
    {
        int index = xy.y * width + xy.x;
        return (data[index].data & 1) == 1 && data[index].Occupation == 0;
    }
    public bool isEnableExceptSelfVolume(int2 xy, AStarVolume volume, int2 self)
    {
        int index = xy.y * width + xy.x;
        return (data[index].data & 1) == 1 && (data[index].Occupation == 0 || (volume.isInScope(self, xy) && data[index].Occupation == 1));
    }
    public bool isEnable(int index) => (data[index].data & 1) == 1 && data[index].Occupation == 0;
    public bool isInScope(int2 xy) => xy.x >= 0 && xy.y >= 0 && xy.x < width && xy.y < height;
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
        array ??= new int2[this.width * this.height];
        array[index++] = origin;
        data.ElementAt(origin.y * width + origin.x).vs = vs;

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
            if (v2.x < width - 1)
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
            if (v2.y < height - 1)
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
                if (v2.x > 0 && v2.y < height - 1)
                {
                    if (breadth(func, ref index, new int2(v2.x - 1, v2.y + 1), out value))
                    {
                        ret = true;
                        break;
                    }
                }
                if (v2.x < width - 1 && v2.y > 0)
                {
                    if (breadth(func, ref index, new int2(v2.x + 1, v2.y - 1), out value))
                    {
                        ret = true;
                        break;
                    }
                }
                if (v2.x < width - 1 && v2.y < height - 1)
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
        if (data[xy.y * width + xy.x].vs == vs)
            return false;
        data.ElementAt(xy.y * width + xy.x).vs = vs;
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
