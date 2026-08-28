using Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.Mathematics;

internal static class AStarTest
{
    [Test]
    public static void test()
    {
        TestRectNoObstacle();
        TestRectWithObstacle();
        TestHexEvenUp();
        TestHexOddRight();
        TestNearTarget();
        TestUnreachable();
        Console.WriteLine("✅ 所有 A* 测试通过");
    }

    // ---------- 辅助：创建矩形网格 ----------
    private static AStarData CreateRectData(int width, int height, Func<int, int, bool> walkable)
    {
        byte[] data = new byte[width * height];
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                data[y * width + x] = (byte)(walkable(x, y) ? 1 : 0);

        return new AStarData(width, height, data, float3.zero, new float3(1, 0, 1),
            PathGridType.Rect, PathGridHexParityType.Even, PathGridHexFacingType.Up);
    }

    // ---------- 辅助：创建六边形网格 ----------
    private static AStarData CreateHexData(int width, int height,
        PathGridHexParityType parity, PathGridHexFacingType facing,
        Func<int, int, bool> walkable)
    {
        byte[] data = new byte[width * height];
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                data[y * width + x] = (byte)(walkable(x, y) ? 1 : 0);

        return new AStarData(width, height, data, float3.zero, new float3(1, 0, 1),
            PathGridType.Hex, parity, facing);
    }

    // ---------- 辅助：验证路径连续性（矩形四方向） ----------
    private static void ValidateRectPath(FastList<int2> path, int2 from, int2 to)
    {
        if (path.Count == 0)
            throw new Exception("路径为空");
        if (path[0].x != from.x || path[0].y != from.y)
            throw new Exception($"起点错误: 期望 ({from.x},{from.y}) 实际 ({path[0].x},{path[0].y})");
        int last = path.Count - 1;
        if (path[last].x != to.x || path[last].y != to.y)
            throw new Exception($"终点错误: 期望 ({to.x},{to.y}) 实际 ({path[last].x},{path[last].y})");
        for (int i = 0; i < last; i++)
        {
            int dx = Math.Abs(path[i + 1].x - path[i].x);
            int dy = Math.Abs(path[i + 1].y - path[i].y);
            if (dx + dy != 1)
                throw new Exception($"路径不连续: {path[i]} -> {path[i + 1]}");
        }
    }

    // ---------- 测试 1: 矩形无障碍 ----------
    private static void TestRectNoObstacle()
    {
        var data = CreateRectData(5, 5, (x, y) => true);
        var finder = new AStarFinder();
        finder.Init(data);

        bool result = finder.Finding(new int2(0, 0), new int2(4, 4));
        if (!result) throw new Exception("RectNoObstacle: 寻路失败");

        var path = new FastList<int2>();
        finder.GetGrids(path);
        ValidateRectPath(path, new int2(0, 0), new int2(4, 4));
        // 曼哈顿距离 8 + 起点自身 = 9
        if (path.Count != 9)
            throw new Exception($"RectNoObstacle: 路径长度 {path.Count} 期望 9");
        Console.WriteLine("✅ RectNoObstacle 通过");
    }

    // ---------- 测试 2: 矩形有障碍 ----------
    private static void TestRectWithObstacle()
    {
        var data = CreateRectData(5, 5, (x, y) => !(x == 2 && y == 2));
        var finder = new AStarFinder();
        finder.Init(data);

        bool result = finder.Finding(new int2(0, 0), new int2(4, 4));
        if (!result) throw new Exception("RectWithObstacle: 寻路失败");

        var path = new FastList<int2>();
        finder.GetGrids(path);
        ValidateRectPath(path, new int2(0, 0), new int2(4, 4));

        // 检查是否绕过障碍 (2,2)
        foreach (var p in path)
            if (p.x == 2 && p.y == 2)
                throw new Exception("RectWithObstacle: 路径经过障碍 (2,2)");

        Console.WriteLine("✅ RectWithObstacle 通过");
    }

    // ---------- 测试 3: 六边形 偶数行左偏 尖角上下 ----------
    private static void TestHexEvenUp()
    {
        var data = CreateHexData(5, 5, PathGridHexParityType.Even, PathGridHexFacingType.Up, (x, y) => true);
        var finder = new AStarFinder();
        finder.Init(data);

        bool result = finder.Finding(new int2(0, 0), new int2(4, 4));
        if (!result) throw new Exception("HexEvenUp: 寻路失败");

        var path = new FastList<int2>();
        finder.GetGrids(path);

        if (path.Count == 0) throw new Exception("HexEvenUp: 路径为空");
        if (path[0].x != 0 || path[0].y != 0)
            throw new Exception($"HexEvenUp: 起点错误 ({path[0].x},{path[0].y})");
        int last = path.Count - 1;
        if (path[last].x != 4 || path[last].y != 4)
            throw new Exception($"HexEvenUp: 终点错误 ({path[last].x},{path[last].y})");

        // 简单检查：路径无重复点
        var set = new HashSet<int2>();
        foreach (var p in path)
            if (!set.Add(p))
                throw new Exception($"HexEvenUp: 路径存在重复点 {p}");

        Console.WriteLine("✅ HexEvenUp 通过");
    }

    // ---------- 测试 4: 六边形 奇数行左偏 尖角左右 ----------
    private static void TestHexOddRight()
    {
        var data = CreateHexData(5, 5, PathGridHexParityType.Odd, PathGridHexFacingType.Right, (x, y) => true);
        var finder = new AStarFinder();
        finder.Init(data);

        bool result = finder.Finding(new int2(0, 0), new int2(4, 4));
        if (!result) throw new Exception("HexOddRight: 寻路失败");

        var path = new FastList<int2>();
        finder.GetGrids(path);

        if (path.Count == 0) throw new Exception("HexOddRight: 路径为空");
        if (path[0].x != 0 || path[0].y != 0)
            throw new Exception($"HexOddRight: 起点错误 ({path[0].x},{path[0].y})");
        int last = path.Count - 1;
        if (path[last].x != 4 || path[last].y != 4)
            throw new Exception($"HexOddRight: 终点错误 ({path[last].x},{path[last].y})");

        var set = new HashSet<int2>();
        foreach (var p in path)
            if (!set.Add(p))
                throw new Exception($"HexOddRight: 路径存在重复点 {p}");

        Console.WriteLine("✅ HexOddRight 通过");
    }

    // ---------- 测试 5: near 参数 ----------
    private static void TestNearTarget()
    {
        // 起点 (0,0)，目标 (4,4) 可达
        var data = CreateRectData(5, 5, (x, y) => true);
        var finder = new AStarFinder();
        finder.Init(data);

        // near=0 直接到 (4,4)
        bool result = finder.Finding(new int2(0, 0), new int2(4, 4), near: 0);
        if (!result) throw new Exception("Near0: 寻路失败");
        var path = new FastList<int2>();
        finder.GetGrids(path);
        int last = path.Count - 1;
        if (path[last].x != 4 || path[last].y != 4)
            throw new Exception($"Near0: 终点应为 (4,4) 实际 ({path[last].x},{path[last].y})");

        // near=1 应找到附近点，但目标 (4,4) 本身可行走，仍应到 (4,4)
        // 此处仅测试功能不崩溃
        result = finder.Finding(new int2(0, 0), new int2(4, 4), near: 1);
        if (!result) throw new Exception("Near1: 寻路失败");
        path.Clear();
        finder.GetGrids(path);
        if (path.Count == 0) throw new Exception("Near1: 路径为空");
        Console.WriteLine("✅ Near 测试通过");
    }

    // ---------- 测试 6: 不可达 ----------
    private static void TestUnreachable()
    {
        // 起点不可走
        var data = CreateRectData(5, 5, (x, y) => !(x == 0 && y == 0));
        var finder = new AStarFinder();
        finder.Init(data);
        bool result = finder.Finding(new int2(0, 0), new int2(4, 4));
        if (result) throw new Exception("Unreachable: 起点不可走但返回 true");

        // 终点不可走
        data = CreateRectData(5, 5, (x, y) => !(x == 4 && y == 4));
        finder = new AStarFinder();
        finder.Init(data);
        result = finder.Finding(new int2(0, 0), new int2(4, 4));
        if (result) throw new Exception("Unreachable: 终点不可走但返回 true");

        // 起点和终点都被障碍隔开（如中间一堵墙）
        data = CreateRectData(5, 5, (x, y) => !(x == 2));
        finder = new AStarFinder();
        finder.Init(data);
        result = finder.Finding(new int2(0, 0), new int2(4, 4));
        if (result) throw new Exception("Unreachable: 被墙隔开但返回 true");

        Console.WriteLine("✅ Unreachable 测试通过");
    }
}