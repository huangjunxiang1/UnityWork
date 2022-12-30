using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;
using Main;
using Game;
using Unity.Physics;
using Unity.Burst;
using Unity.Jobs;


public class Demo3DF
{
    public const int size = 100;
}



[MaterialProperty("_EmissiveColor")]
public struct HDRPMaterialPropertyEmissiveColor1 : IComponentData { public float4 Value; }
public struct Demo1Delay : IComponentData
{
    public float dTime;
    public float lTime;
    public float3 v3;
}
public struct AI_XunLuo : IComponentData
{
    public float3 last;
    public float3 target;
    public float dTime;
}
[BurstCompile]
public struct Demo3Com : IComponentData
{
    public int2 target;
}
[BurstCompile]
public struct FindData
{
    public int index;
    public int2 xy;
}
[BurstCompile]
public struct Point
{
    public int2 xy;
    public int2 link;
}

public partial struct Demo1Sys : ISystem
{
    public void OnCreate(ref SystemState state)
    {

    }

    public void OnDestroy(ref SystemState state)
    {

    }

    public void OnUpdate(ref SystemState state)
    {
        var dt = SystemAPI.Time.DeltaTime;
        foreach (var t in SystemAPI.Query<Demo1Asp>())
            t.Move(dt);
    }
}
public readonly partial struct Demo1Asp : IAspect
{
    readonly Entity self;
    readonly RefRW<Demo1Delay> target;
    readonly RefRW<LocalToWorld> pos;

    public void Move(float dt)
    {
        if (target.ValueRW.dTime > 0)
        {
            target.ValueRW.dTime -= dt;
            return;
        }
        target.ValueRW.lTime = math.max(0, target.ValueRW.lTime - dt / 3f);
        float4x4 f44 = pos.ValueRW.Value;
        f44.c3.xyz = math.lerp(target.ValueRW.v3, f44.c3.xyz, target.ValueRW.lTime);
        pos.ValueRW.Value = f44;
    }
}

public partial struct AI_XunLuoSys : ISystem
{
    public static bool start = false;
    public static NativeArray<int> road;
    public static int2 roadSize = new int2(200, 200);
    public void OnCreate(ref SystemState state)
    {

    }

    public void OnDestroy(ref SystemState state)
    {

    }


    public void OnUpdate(ref SystemState state)
    {
        if (!start)
            return;
        var dt = SystemAPI.Time.DeltaTime;
        var random = new Unity.Mathematics.Random((uint)DateTime.Now.Ticks);
        foreach (var t in SystemAPI.Query<AI_XunLuoAsp>())
            t.Move(dt, road, roadSize, ref random);
    }
}


public readonly partial struct AI_XunLuoAsp : IAspect
{
    readonly RefRW<AI_XunLuo> target;
    readonly RefRW<LocalToWorld> pos;


    public void Move(float dt, NativeArray<int> roads, int2 size, ref Unity.Mathematics.Random random)
    {
        float4x4 f44 = pos.ValueRO.Value;
        int2 now = math.clamp((int2)f44.c3.xz, 0, size.x);
        int2 t = (int2)target.ValueRO.target.xz;
        int num = roads[t.y * size.x + t.x];
        if (num == 1)
        {
            int2 r = random.NextInt2(-1, 2);
            target.ValueRW.last = f44.c3.xyz;
            target.ValueRW.target = math.clamp(f44.c3.xyz + new float3(r.x, 0, r.y), 0, size.x - 1);
            target.ValueRW.dTime = 0;
        }
        if (math.all(now == t) || num == 0)
        {
            f44.c3.xyz = math.lerp(target.ValueRO.last, target.ValueRO.target, math.clamp(target.ValueRW.dTime += dt, 0, 1));
            pos.ValueRW.Value = f44;
            int2 next = (int2)f44.c3.xz;
            if (math.any(now != next) || math.all(now == t))
            {
                --roads[now.y * size.x + now.x];
                ++roads[next.y * size.x + next.x];
                int2 r = random.NextInt2(-1, 2);
                target.ValueRW.last = f44.c3.xyz;
                target.ValueRW.target = math.clamp(f44.c3.xyz + new float3(r.x, 0, r.y), 0, size.x - 1);
                target.ValueRW.dTime = 0;
            }
        }
    }
}

[DisableAutoCreation]
public partial struct Demo3Sys : ISystem
{
    public static int playerCount;
    public static NativeArray<int> road;

    EntityQuery query;
    ComponentTypeHandle<Demo3Com> dTypeHandle;
    ComponentTypeHandle<LocalToWorld> LTypeHandle;
    static int tmpMv;
    NativeArray<int3> ps;
    public void OnCreate(ref SystemState state)
    {
        ps = new NativeArray<int3>(playerCount, Allocator.Persistent);
        query = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(typeof(Demo3Com), typeof(LocalToWorld));
        dTypeHandle = state.GetComponentTypeHandle<Demo3Com>();
        LTypeHandle = state.GetComponentTypeHandle<LocalToWorld>();
    }

    public void OnDestroy(ref SystemState state)
    {
        ps.Dispose();
        query.Dispose();
    }

    public void OnUpdate(ref SystemState state)
    {
        var chunks = query.ToArchetypeChunkArray(Allocator.TempJob);
        dTypeHandle.Update(ref state);
        LTypeHandle.Update(ref state);

        int mv = ++tmpMv;
        for (int cc = 0; cc < chunks.Length; cc++)
        {
            var chunk = chunks[cc];
            var ds = chunk.GetNativeArray(ref dTypeHandle);
            var ls = chunk.GetNativeArray(ref LTypeHandle);

            for (int c = 0; c < chunk.Count; c++)
            {
                Demo3Com t1 = ds[c];
                LocalToWorld pos = ls[c];

                float4x4 f44 = pos.Value;

                PathFindJob2 job = new PathFindJob2();
                job.Size = new int2(Demo3DF.size, Demo3DF.size);
                job.road = road;
                job.now = (int2)f44.c3.xz;
                job.target = t1.target;

                job.ps = ps;
                job.pIdx = cc * chunk.Capacity + c;

                job.mv = mv;

                job.Schedule();
            }
        }

        JobforMove move = new JobforMove();
        move.chunks = chunks;
        move.dTypeHandle = dTypeHandle;
        move.LTypeHandle = LTypeHandle;
        move.road = road;
        move.random = SharedStatic<Unity.Mathematics.Random>.GetOrCreate<Unity.Mathematics.Random>();
        move.random.Data.state = (uint)(SystemAPI.Time.ElapsedTime * 1000);
        move.paths = ps;
        state.Dependency = move.Schedule(chunks.Length, 64, state.Dependency);

        chunks.Dispose();
    }
}

[BurstCompile]
public partial struct JobforMove : IJobParallelFor
{
    [ReadOnly]
    [Unity.Collections.LowLevel.Unsafe.NativeDisableContainerSafetyRestriction]
    public NativeArray<ArchetypeChunk> chunks;

    public ComponentTypeHandle<Demo3Com> dTypeHandle;
    public ComponentTypeHandle<LocalToWorld> LTypeHandle;

    [Unity.Collections.LowLevel.Unsafe.NativeDisableContainerSafetyRestriction]
    public NativeArray<int> road;

    [ReadOnly]
    [Unity.Collections.LowLevel.Unsafe.NativeDisableUnsafePtrRestriction]
    public SharedStatic<Unity.Mathematics.Random> random;

    [ReadOnly]
    [Unity.Collections.LowLevel.Unsafe.NativeDisableContainerSafetyRestriction]
    public NativeArray<int3> paths;

    const float speed = 0.1f;
    [BurstCompile]
    public void Execute(int index)
    {
        var chunk = chunks[index];
        var ds = chunk.GetNativeArray(ref dTypeHandle);
        var ls = chunk.GetNativeArray(ref LTypeHandle);
        for (int c = 0; c < chunk.Count; c++)
        {
            Demo3Com t1 = ds[c];
            LocalToWorld pos = ls[c];

            float4x4 f44 = pos.Value;
            int2 now = (int2)f44.c3.xz;
            int pIdx = index * chunk.Capacity + c;
            int3 r3 = paths[pIdx];

            if (r3.z == 0 || math.all(t1.target == now) || math.all(t1.target == int2.zero))
            {
                int2 r = random.Data.NextInt2(0, Demo3DF.size);
                int2 xy = default;
                bool find = false;
                for (int i = r.y * Demo3DF.size + r.x; i < road.Length; i++)
                {
                    if (road[i] > 0 || i == now.y * Demo3DF.size + now.x)
                        continue;
                    int x = i % Demo3DF.size;
                    int y = i / Demo3DF.size;
                    xy.x = x;
                    xy.y = y;
                    find = true;
                    break;
                }
                if (!find)
                {
                    for (int i = 0; i < r.y * Demo3DF.size + r.x; i++)
                    {
                        if (road[i] > 0 || i == now.y * Demo3DF.size + now.x)
                            continue;
                        int x = i % Demo3DF.size;
                        int y = i / Demo3DF.size;
                        xy.x = x;
                        xy.y = y;
                        break;
                    }
                }

                t1.target = xy;
            }
            else
            {
                f44.c3.xz += speed * math.normalize(((float2)r3.xy + 0.5f - f44.c3.xz));
                /*int2 next = (int2)f44.c3.xz;
                if (math.any(now != next))
                {
                    --road[now.y * FUIFighting3.size + now.x];
                    ++road[next.y * FUIFighting3.size + next.x];
                }*/
                pos.Value = f44;
            }
            ds[c] = t1;
            ls[c] = pos;
        }
    }
}

//广度搜索
[BurstCompile]
public struct PathFindJob2 : IJob
{


    [ReadOnly]
    public int2 Size;
    [ReadOnly]
    public int2 now;
    [ReadOnly]
    public int2 target;
    [ReadOnly]
    public NativeArray<int> road;

    [WriteOnly]
    [Unity.Collections.LowLevel.Unsafe.NativeDisableContainerSafetyRestriction]
    public NativeArray<int3> ps;
    [ReadOnly]
    public int pIdx;

    [ReadOnly]
    public int mv;

    [BurstCompile]
    public void Execute()
    {
        NativeArray<int> mark = new NativeArray<int>(Demo3DF.size * Demo3DF.size, Allocator.Temp);
        NativeArray<Point> temp = new NativeArray<Point>(Demo3DF.size * Demo3DF.size, Allocator.Temp, NativeArrayOptions.UninitializedMemory);

        Point p = new Point();
        p.xy = now;
        p.link = now;
        int2 link = now;
        bool find = false;
        int sIdx = 0;
        int eIdx = 0;

        {
            Point pp = p;
            if (pp.xy.x > 0)
            {
                int2 xy = new int2(pp.xy.x - 1, pp.xy.y);
                int idx = xy.y * Size.x + xy.x;
                if (road[idx] == 0)
                {
                    if (math.all(xy == target))
                    {
                        find = true;
                        link = xy;
                    }
                    else if (mark[idx] != mv)
                    {
                        Point t = new Point();
                        t.xy = xy;
                        t.link = xy;
                        temp[eIdx++] = t;
                        mark[idx] = mv;
                    }
                }
            }

            if (pp.xy.y > 0)
            {
                int2 xy = new int2(pp.xy.x, pp.xy.y - 1);
                int idx = xy.y * Size.x + xy.x;
                if (road[idx] == 0)
                {
                    if (math.all(xy == target))
                    {
                        find = true;
                        link = xy;
                    }
                    else if (mark[idx] != mv)
                    {
                        Point t = new Point();
                        t.xy = xy;
                        t.link = xy;
                        temp[eIdx++] = t;
                        mark[idx] = mv;
                    }
                }
            }

            if (pp.xy.x < Size.x - 1)
            {
                int2 xy = new int2(pp.xy.x + 1, pp.xy.y);
                int idx = xy.y * Size.x + xy.x;
                if (road[idx] == 0)
                {
                    if (math.all(xy == target))
                    {
                        find = true;
                        link = xy;
                    }
                    else if (mark[idx] != mv)
                    {
                        Point t = new Point();
                        t.xy = xy;
                        t.link = xy;
                        temp[eIdx++] = t;
                        mark[idx] = mv;
                    }
                }
            }

            if (pp.xy.y < Size.y - 1)
            {
                int2 xy = new int2(pp.xy.x, pp.xy.y + 1);
                int idx = xy.y * Size.x + xy.x;
                if (road[idx] == 0)
                {
                    if (math.all(xy == target))
                    {
                        find = true;
                        link = xy;
                    }
                    else if (mark[idx] != mv)
                    {
                        Point t = new Point();
                        t.xy = xy;
                        t.link = xy;
                        temp[eIdx++] = t;
                        mark[idx] = mv;
                    }
                }
            }
        }
        if (!find)
        {
            while (sIdx < eIdx)
            {
                int next = eIdx;
                for (int i = sIdx; i < next; i++)
                {
                    Point pp = temp[i];

                    if (pp.xy.x > 0)
                    {
                        int2 xy = new int2(pp.xy.x - 1, pp.xy.y);
                        int idx = xy.y * Size.x + xy.x;
                        if (road[idx] == 0)
                        {
                            if (math.all(xy == target))
                            {
                                find = true;
                                link = pp.link;
                                break;
                            }
                            else if (mark[idx] != mv)
                            {
                                Point t = new Point();
                                t.xy = xy;
                                t.link = pp.link;
                                temp[eIdx++] = t;
                                mark[idx] = mv;
                            }
                        }
                    }

                    if (pp.xy.y > 0)
                    {
                        int2 xy = new int2(pp.xy.x, pp.xy.y - 1);
                        int idx = xy.y * Size.x + xy.x;
                        if (road[idx] == 0)
                        {
                            if (math.all(xy == target))
                            {
                                find = true;
                                link = pp.link;
                                break;
                            }
                            else if (mark[idx] != mv)
                            {
                                Point t = new Point();
                                t.xy = xy;
                                t.link = pp.link;
                                temp[eIdx++] = t;
                                mark[idx] = mv;
                            }
                        }
                    }

                    if (pp.xy.x < Size.x - 1)
                    {
                        int2 xy = new int2(pp.xy.x + 1, pp.xy.y);
                        int idx = xy.y * Size.x + xy.x;
                        if (road[idx] == 0)
                        {
                            if (math.all(xy == target))
                            {
                                find = true;
                                link = pp.link;
                                break;
                            }
                            else if (mark[idx] != mv)
                            {
                                Point t = new Point();
                                t.xy = xy;
                                t.link = pp.link;
                                temp[eIdx++] = t;
                                mark[idx] = mv;
                            }
                        }
                    }

                    if (pp.xy.y < Size.y - 1)
                    {
                        int2 xy = new int2(pp.xy.x, pp.xy.y + 1);
                        int idx = xy.y * Size.x + xy.x;
                        if (road[idx] == 0)
                        {
                            if (math.all(xy == target))
                            {
                                find = true;
                                link = pp.link;
                                break;
                            }
                            else if (mark[idx] != mv)
                            {
                                Point t = new Point();
                                t.xy = xy;
                                t.link = pp.link;
                                temp[eIdx++] = t;
                                mark[idx] = mv;
                            }
                        }
                    }
                }
                if (find)
                    break;
                sIdx = next;
            }
        }

        int3 r3 = link.xyy;
        r3.z = find ? 1 : 0;
        ps[pIdx] = r3;

        mark.Dispose();
        temp.Dispose();
    }
}
