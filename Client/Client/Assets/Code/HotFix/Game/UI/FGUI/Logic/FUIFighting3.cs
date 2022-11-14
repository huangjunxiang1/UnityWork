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
using Unity.Jobs;
using Unity.Burst;

partial class FUIFighting3
{
    public const int size = 100;
    public NativeArray<int> road;
    NativeArray<Entity> block;
    int blockCount = 3000;
    NativeArray<Entity> Player;
    public const int playerCount = 200;
    SystemHandle sys;

    protected override async void OnEnter(params object[] data)
    {
        base.OnEnter(data);

        Entity one = await AssetLoad.LoadEntityAsync(@"3D\Model\ECS\Cube.prefab", TaskCreater);
        var em = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
        em.SetComponentData(one, new LocalToWorld() { Value = float4x4.Translate(float3.zero) });
        em.AddComponentData(one, new HDRPMaterialPropertyEmissiveColor1() { Value = new float4(0, 0, 1, 1) });
        block = new NativeArray<Entity>(blockCount, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
        block[0] = one;
        for (int i = 1; i < blockCount; i++)
        {
            Entity e = em.Instantiate(one);
            block[i] = e;
        }
        _findStyle.selectedIndex = 0;

        _rangeRoad.onClick.Add(_click_rangeRoad);
        _play.onClick.Add(_click_play);
        _btnBack.onClick.Add(_clickBack);
    }
    protected override void OnExit()
    {
        base.OnExit();
        if (road.IsCreated)
            road.Dispose();
        var em = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
        if (block.IsCreated)
        {
            em.DestroyEntity(block);
            block.Dispose();
        }
        if (Player.IsCreated)
        {
            em.DestroyEntity(Player);
            Player.Dispose();
        }
        if (sys != SystemHandle.Null)
            Unity.Entities.World.DefaultGameObjectInjectionWorld.DestroySystem(sys);
    }

    [Event((int)EventIDM.QuitGame)]
    void quit()
    {
        if (road.IsCreated)
            road.Dispose();
        if (block.IsCreated)
            block.Dispose();
    }
    void _clickBack()
    {
        _ = GameL.Scene.InLoginScene();
    }
    void _click_rangeRoad()
    {
        if (!road.IsCreated)
            road = new NativeArray<int>(size * size, Allocator.Persistent, NativeArrayOptions.ClearMemory);
        for (int i = 0; i < size * size; i++)
            road[i] = 0;
        Unity.Mathematics.Random random = new((uint)DateTime.Now.Ticks);
        var em = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
        for (int i = 0; i < block.Length; i++)
        {
            int2 xy = random.NextInt2(size);
            road[xy.y * size + xy.x] = 1;
            em.SetComponentData(block[i], new LocalToWorld() { Value = float4x4.Translate(new float3(xy.x, 0, xy.y) + 0.5f) });
        }
    }
    void _click_play()
    {
        if (!road.IsCreated)
        {
            Box.Tips("还未生成地图");
            return;
        }
        var em = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
        if (!Player.IsCreated)
        {
             Player = new NativeArray<Entity>(playerCount, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            for (int i = 0; i < Player.Length; i++)
            {
                var e = em.Instantiate(block[0]);
                em.AddComponentData(e, new HDRPMaterialPropertyEmissiveColor1() { Value = new float4(1, 0, 0, 1) });
                em.AddComponentData(e, new Demo3Com());
                Player[i] = e;
            }
        }
       
        Unity.Mathematics.Random random = new Unity.Mathematics.Random((uint)DateTime.Now.Ticks);
        for (int i = 0; i < Player.Length; i++)
        {
            int2 r = random.NextInt2(0, size);
            bool find = false;
            for (int j = r.y * size + r.x; j < road.Length; j++)
            {
                if (road[j] > 0)
                    continue;
                int x = j % size;
                int y = j / size;
                r.x = x;
                r.y = y;
                find = true;
                break;
            }
            if (!find)
            {
                for (int j = 0; j < r.y * size + r.x; j++)
                {
                    if (road[j] > 0)
                        continue;
                    int x = j % size;
                    int y = j / size;
                    r.x = x;
                    r.y = y;
                    break;
                }
            }
            road[r.y * size + r.x]++;
            em.SetComponentData(Player[i], new LocalToWorld() { Value = float4x4.Translate(new float3(r.x, 0, r.y) + 0.5f) });
        }

        if (sys == SystemHandle.Null)
        {
            sys = Unity.Entities.World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<Demo3Sys>();
            Unity.Entities.World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<SimulationSystemGroup>().AddSystemToUpdateList(sys);
        }
    }
}

[DisableAutoCreation]
partial struct Demo3Sys : ISystem
{
    EntityQuery query;
    ComponentTypeHandle<Demo3Com> dTypeHandle;
    ComponentTypeHandle<LocalToWorld> LTypeHandle;
    static int tmpMv;
    public void OnCreate(ref SystemState state)
    {
        query = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(typeof(Demo3Com), typeof(LocalToWorld));
        dTypeHandle = state.GetComponentTypeHandle<Demo3Com>();
        LTypeHandle = state.GetComponentTypeHandle<LocalToWorld>();
    }

    public void OnDestroy(ref SystemState state)
    {
        query.Dispose();
    }

    public void OnUpdate(ref SystemState state)
    {
        var road = GameL.UI.Get<FUIFighting3>().road;

        state.Dependency.Complete();
        NativeArray<JobHandle> jobs = new NativeArray<JobHandle>(FUIFighting3.playerCount, Allocator.Temp);
        NativeArray<int3> ps = new NativeArray<int3>(FUIFighting3.playerCount, Allocator.TempJob);
        var chunks = query.ToArchetypeChunkArray(Allocator.TempJob);
        dTypeHandle.Update(ref state);
        LTypeHandle.Update(ref state);

        int mv = ++tmpMv;
        for (int cc = 0; cc < chunks.Length; cc++)
        {
            var chunk = chunks[cc];
            var ds = chunk.GetNativeArray(dTypeHandle);
            var ls = chunk.GetNativeArray(LTypeHandle);

            for (int c = 0; c < chunk.Count; c++)
            {
                Demo3Com t1 = ds[c];
                LocalToWorld pos = ls[c];

                float4x4 f44 = pos.Value;

                PathFindJob2 job = new PathFindJob2();
                job.Size = new int2(FUIFighting3.size, FUIFighting3.size);
                job.road = road;
                job.now = (int2)f44.c3.xz;
                job.target = t1.target;

                job.ps = ps;
                job.pIdx = cc * chunk.Capacity + c;

                job.mv = mv;

                jobs[job.pIdx] = job.Schedule();
            }
        }
     
        JobHandle.CompleteAll(jobs);
        jobs.Dispose();

        JobforMove move = new JobforMove();
        move.chunks = chunks;
        move.dTypeHandle = dTypeHandle;
        move.LTypeHandle = LTypeHandle;
        move.road = road;
        move.random = SharedStatic<Unity.Mathematics.Random>.GetOrCreate<Unity.Mathematics.Random>();
        move.random.Data.state = (uint)(SystemAPI.Time.ElapsedTime * 1000);
        move.paths = ps;
        move.Schedule(chunks.Length, 64, state.Dependency).Complete();
        ps.Dispose();
        chunks.Dispose();
    }
}
[BurstCompile]
public struct Demo3Com : IComponentData
{
    public int2 target;
}

[BurstCompile]
public partial struct JobforMove : IJobParallelFor
{
    [ReadOnly] 
    public NativeArray<ArchetypeChunk> chunks;

    public ComponentTypeHandle<Demo3Com> dTypeHandle;
    public ComponentTypeHandle<LocalToWorld> LTypeHandle;

    [Unity.Collections.LowLevel.Unsafe.NativeDisableContainerSafetyRestriction]
    public NativeArray<int> road;

    [ReadOnly][Unity.Collections.LowLevel.Unsafe.NativeDisableUnsafePtrRestriction] 
    public SharedStatic<Unity.Mathematics.Random> random;

    [ReadOnly] 
    public NativeArray<int3> paths;

    const float speed = 0.1f;
    const int size = 100;
    [BurstCompile]
    public void Execute(int index)
    {
        var chunk = chunks[index];
        var ds = chunk.GetNativeArray(dTypeHandle);
        var ls = chunk.GetNativeArray(LTypeHandle);
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
                int2 r = random.Data.NextInt2(0, size);
                int2 xy = default;
                bool find = false;
                for (int i = r.y * size + r.x; i < road.Length; i++)
                {
                    if (road[i] > 0 || i == now.y * size + now.x)
                        continue;
                    int x = i % size;
                    int y = i / size;
                    xy.x = x;
                    xy.y = y;
                    find = true;
                    break;
                }
                if (!find)
                {
                    for (int i = 0; i < r.y * size + r.x; i++)
                    {
                        if (road[i] > 0 || i == now.y * size + now.x)
                            continue;
                        int x = i % size;
                        int y = i / size;
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
                int2 next = (int2)f44.c3.xz;
                if (math.any(now != next))
                {
                    --road[now.y * size + now.x];
                    ++road[next.y * size + next.x];
                }
                pos.Value = f44;
            }
            ds[c] = t1;
            ls[c] = pos;
        }
    }
}

/*//深度搜索
[BurstCompile]
public struct PathFindJob : IJob
{
    public NativeArray<bool> road;
    public int2 now;
    public int2 target;
    public NativeList<int2> paths;
    public NativeArray<int> finded;
    public int mark;
    public NativeList<FindData> tmp;
    [BurstCompile]
    public void Execute()
    {
        finded[now.y * FUIFighting3.size + now.x] = mark;

        FindData fd = new FindData { xy = now, index = -1 };
        tmp.Clear();
        tmp.Add(fd);

        while (true)
        {
            int2 offset = target - fd.xy;
            bool addded = false;
            bool find = false;
            int index = tmp.Length - 1;
            if (offset.x > 0 && offset.x >= math.abs(offset.y))
            {
                if (!find && fd.xy.x > 0)
                {
                    int2 xy = new int2(fd.xy.x - 1, fd.xy.y);
                    int idx = xy.y * FUIFighting3.size + xy.x;
                    if (road[idx] && finded[idx] != mark)
                    {
                        tmp.Add(new FindData { xy = xy, index = index });
                        addded = true;
                        find = math.all(xy == target);
                        finded[idx] = mark;
                    }
                }
                if (!find && fd.xy.y > 0)
                {
                    int2 xy = new int2(fd.xy.x, fd.xy.y - 1);
                    int idx = xy.y * FUIFighting3.size + xy.x;
                    if (road[idx] && finded[idx] != mark)
                    {
                        tmp.Add(new FindData { xy = xy, index = index });
                        addded = true;
                        find = math.all(xy == target);
                        finded[idx] = mark;
                    }
                }
                if (!find && fd.xy.y < FUIFighting3.size - 1)
                {
                    int2 xy = new int2(fd.xy.x, fd.xy.y + 1);
                    int idx = xy.y * FUIFighting3.size + xy.x;
                    if (road[idx] && finded[idx] != mark)
                    {
                        tmp.Add(new FindData { xy = xy, index = index });
                        addded = true;
                        find = math.all(xy == target);
                        finded[idx] = mark;
                    }
                }
                if (!find && fd.xy.x < FUIFighting3.size - 1)
                {
                    int2 xy = new int2(fd.xy.x + 1, fd.xy.y);
                    int idx = xy.y * FUIFighting3.size + xy.x;
                    if (road[idx] && finded[idx] != mark)
                    {
                        tmp.Add(new FindData { xy = xy, index = index });
                        addded = true;
                        find = math.all(xy == target);
                        finded[idx] = mark;
                    }
                }
            }
            else if (offset.x < 0 && -offset.x >= math.abs(offset.y))
            {
                if (!find && fd.xy.x < FUIFighting3.size - 1)
                {
                    int2 xy = new int2(fd.xy.x + 1, fd.xy.y);
                    int idx = xy.y * FUIFighting3.size + xy.x;
                    if (road[idx] && finded[idx] != mark)
                    {
                        tmp.Add(new FindData { xy = xy, index = index });
                        addded = true;
                        find = math.all(xy == target);
                        finded[idx] = mark;
                    }
                }
                if (!find && fd.xy.y > 0)
                {
                    int2 xy = new int2(fd.xy.x, fd.xy.y - 1);
                    int idx = xy.y * FUIFighting3.size + xy.x;
                    if (road[idx] && finded[idx] != mark)
                    {
                        tmp.Add(new FindData { xy = xy, index = index });
                        addded = true;
                        find = math.all(xy == target);
                        finded[idx] = mark;
                    }
                }
                if (!find && fd.xy.y < FUIFighting3.size - 1)
                {
                    int2 xy = new int2(fd.xy.x, fd.xy.y + 1);
                    int idx = xy.y * FUIFighting3.size + xy.x;
                    if (road[idx] && finded[idx] != mark)
                    {
                        tmp.Add(new FindData { xy = xy, index = index });
                        addded = true;
                        find = math.all(xy == target);
                        finded[idx] = mark;
                    }
                }
                if (!find && fd.xy.x > 0)
                {
                    int2 xy = new int2(fd.xy.x - 1, fd.xy.y);
                    int idx = xy.y * FUIFighting3.size + xy.x;
                    if (road[idx] && finded[idx] != mark)
                    {
                        tmp.Add(new FindData { xy = xy, index = index });
                        addded = true;
                        find = math.all(xy == target);
                        finded[idx] = mark;
                    }
                }
            }
            else if (offset.y > 0 && offset.y > math.abs(offset.x))
            {
                if (!find && fd.xy.y > 0)
                {
                    int2 xy = new int2(fd.xy.x, fd.xy.y - 1);
                    int idx = xy.y * FUIFighting3.size + xy.x;
                    if (road[idx] && finded[idx] != mark)
                    {
                        tmp.Add(new FindData { xy = xy, index = index });
                        addded = true;
                        find = math.all(xy == target);
                        finded[idx] = mark;
                    }
                }
                if (!find && fd.xy.x > 0)
                {
                    int2 xy = new int2(fd.xy.x - 1, fd.xy.y);
                    int idx = xy.y * FUIFighting3.size + xy.x;
                    if (road[idx] && finded[idx] != mark)
                    {
                        tmp.Add(new FindData { xy = xy, index = index });
                        addded = true;
                        find = math.all(xy == target);
                        finded[idx] = mark;
                    }
                }
                if (!find && fd.xy.x < FUIFighting3.size - 1)
                {
                    int2 xy = new int2(fd.xy.x + 1, fd.xy.y);
                    int idx = xy.y * FUIFighting3.size + xy.x;
                    if (road[idx] && finded[idx] != mark)
                    {
                        tmp.Add(new FindData { xy = xy, index = index });
                        addded = true;
                        find = math.all(xy == target);
                        finded[idx] = mark;
                    }
                }
                if (!find && fd.xy.y < FUIFighting3.size - 1)
                {
                    int2 xy = new int2(fd.xy.x, fd.xy.y + 1);
                    int idx = xy.y * FUIFighting3.size + xy.x;
                    if (road[idx] && finded[idx] != mark)
                    {
                        tmp.Add(new FindData { xy = xy, index = index });
                        addded = true;
                        find = math.all(xy == target);
                        finded[idx] = mark;
                    }
                }
            }
            else
            {
                if (!find && fd.xy.y < FUIFighting3.size - 1)
                {
                    int2 xy = new int2(fd.xy.x, fd.xy.y + 1);
                    int idx = xy.y * FUIFighting3.size + xy.x;
                    if (road[idx] && finded[idx] != mark)
                    {
                        tmp.Add(new FindData { xy = xy, index = index });
                        addded = true;
                        find = math.all(xy == target);
                        finded[idx] = mark;
                    }
                }
                if (!find && fd.xy.x > 0)
                {
                    int2 xy = new int2(fd.xy.x - 1, fd.xy.y);
                    int idx = xy.y * FUIFighting3.size + xy.x;
                    if (road[idx] && finded[idx] != mark)
                    {
                        tmp.Add(new FindData { xy = xy, index = index });
                        addded = true;
                        find = math.all(xy == target);
                        finded[idx] = mark;
                    }
                }
                if (!find && fd.xy.x < FUIFighting3.size - 1)
                {
                    int2 xy = new int2(fd.xy.x + 1, fd.xy.y);
                    int idx = xy.y * FUIFighting3.size + xy.x;
                    if (road[idx] && finded[idx] != mark)
                    {
                        tmp.Add(new FindData { xy = xy, index = index });
                        addded = true;
                        find = math.all(xy == target);
                        finded[idx] = mark;
                    }
                }
                if (!find && fd.xy.y > 0)
                {
                    int2 xy = new int2(fd.xy.x, fd.xy.y - 1);
                    int idx = xy.y * FUIFighting3.size + xy.x;
                    if (road[idx] && finded[idx] != mark)
                    {
                        tmp.Add(new FindData { xy = xy, index = index });
                        addded = true;
                        find = math.all(xy == target);
                        finded[idx] = mark;
                    }
                }
            }
            if (find)
                break;
            if (!addded)
                tmp.RemoveAt(tmp.Length - 1);

            if (tmp.Length > 0)
                fd = tmp[tmp.Length - 1];
            else
                break;
        }

        paths.Clear();
        if (tmp.Length > 0)
        {
            FindData t = tmp[tmp.Length - 1];
            while (t.index > -1)
            {
                paths.Add(t.xy);
                t = tmp[t.index];
            }
            paths.Add(t.xy);
        }
    }
}*/

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

    const int size = 100;

    [BurstCompile]
    public void Execute()
    {
        NativeArray<int> mark = new NativeArray<int>(size * size, Allocator.Temp, NativeArrayOptions.ClearMemory);
        NativeArray<Point> temp = new NativeArray<Point>(size * size, Allocator.Temp, NativeArrayOptions.UninitializedMemory);

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
