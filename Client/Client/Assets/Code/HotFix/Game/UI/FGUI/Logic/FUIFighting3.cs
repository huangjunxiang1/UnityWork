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

partial class FUIFighting3
{
    public const int size = 100;
    NativeArray<bool> road;
    NativeArray<Entity> block;
    int blockCount = 5000;
    NativeArray<Entity> Player;
    int playerCount = 1;
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
            for (int i = 0; i < Player.Length; i++)
            {
                var com = em.GetComponentData<Demo3Com>(Player[i]);
                com.paths.Dispose();
                com.finded.Dispose();
                com.tmp.Dispose();
            }
            em.DestroyEntity(Player);
            Player.Dispose();
        }
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
            road = new NativeArray<bool>(size* size, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
        for (int i = 0; i < size * size; i++)
            road[i] = true;
        Unity.Mathematics.Random random = new((uint)DateTime.Now.Ticks);
        var em = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
        for (int i = 0; i < block.Length; i++)
        {
            int2 xy = random.NextInt2(size);
            road[xy.y * size + xy.x] = false;
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
                em.AddComponentData(e, new Demo3Com()
                {
                    paths = new NativeList<int2>(100, AllocatorManager.Persistent),
                    finded = new NativeArray<int>(size * size, Allocator.Persistent),
                    tmp = new NativeList<FindData>(100, AllocatorManager.Persistent)
                });
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
                if (!road[j])
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
                    if (!road[j])
                        continue;
                    int x = j % size;
                    int y = j / size;
                    r.x = x;
                    r.y = y;
                    break;
                }
            }
            em.SetComponentData(Player[i], new LocalToWorld() { Value = float4x4.Translate(new float3(r.x, 0, r.y) + 0.5f) });
        }
    }
    [GenerateTestsForBurstCompatibility]
    partial struct Demo3Sys : ISystem
    {
        public void OnCreate(ref SystemState state)
        {

        }

        public void OnDestroy(ref SystemState state)
        {

        }

        [GenerateTestsForBurstCompatibility]
        public void OnUpdate(ref SystemState state)
        {
            var ui = GameL.UI.Get<FUIFighting3>();
            if (ui == null || !ui.Player.IsCreated)
                return;

            var stime = (float)SystemAPI.Time.ElapsedTime;
            Unity.Mathematics.Random random = new Unity.Mathematics.Random((uint)DateTime.Now.Ticks);
            NativeArray<bool> road = ui.road;
            foreach (var t in SystemAPI.Query<Demo3ComAsp>())
                t.Move(stime, ref random, road);
        }
    }
}
[GenerateTestsForBurstCompatibility]
struct Demo3Com : IComponentData
{
    public float rtime;
    public NativeList<int2> paths;

    public NativeArray<int> finded;
    public int mark;
    public NativeList<FindData> tmp;
}
[GenerateTestsForBurstCompatibility]
readonly partial struct Demo3ComAsp : IAspect
{
    readonly RefRW<Demo3Com> target;
    readonly RefRW<LocalToWorld> pos;
    const float speed = 0.5f;

    [GenerateTestsForBurstCompatibility]
    public void Move(float stime, ref Unity.Mathematics.Random random, NativeArray<bool> road)
    {
        float time = math.max(0, stime - target.ValueRO.rtime);
        float4x4 f44 = pos.ValueRO.Value;
        if (time >= (target.ValueRO.paths.Length - 1) * speed + 2)
        {
            int2 now = (int2)f44.c3.xz;
            int2 r = random.NextInt2(0, FUIFighting3.size);
            int2 xy = default;
            bool find = false;
            for (int i = r.y * FUIFighting3.size + r.x; i < road.Length; i++)
            {
                if (!road[i] || i == now.y * FUIFighting3.size + now.x)
                    continue;
                int x = i % FUIFighting3.size;
                int y = i / FUIFighting3.size;
                xy.x = x;
                xy.y = y;
                find = true;
                break;
            }
            if (!find)
            {
                for (int i = 0; i < r.y * FUIFighting3.size + r.x; i++)
                {
                    if (!road[i] || i == now.y * FUIFighting3.size + now.x)
                        continue;
                    int x = i % FUIFighting3.size;
                    int y = i / FUIFighting3.size;
                    xy.x = x;
                    xy.y = y;
                    break;
                }
            }

            PathFindJob job = new PathFindJob();
            job.road = road;
            job.now = (int2)f44.c3.xz;
            job.target = xy;
            job.paths = target.ValueRO.paths;
            job.finded = target.ValueRO.finded;
            job.mark = ++target.ValueRW.mark;
            job.tmp = target.ValueRO.tmp;

            job.now = 0;
            job.target = FUIFighting3.size - 1;

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            job.Schedule().Complete();
            sw.Stop();
            Loger.Error(sw.Elapsed);
            //job.Execute();

            target.ValueRW.rtime = stime;
            target.ValueRW.paths = job.paths;
            return;
        }
        if (time < (target.ValueRO.paths.Length - 1) * speed)
        {
            int i = target.ValueRO.paths.Length - 1 - (int)(time / speed);
            float2 f2 = math.lerp(target.ValueRO.paths[i], target.ValueRO.paths[i - 1], (time % speed) * 2);
            f44.c3.xyz = new float3(f2.x, 0, f2.y) + 0.5f;
            pos.ValueRW.Value = f44;
        }
        else
        {
            if (target.ValueRO.paths.Length > 0)
            {
                float2 f2 = target.ValueRO.paths[0];
                f44.c3.xyz = new float3(f2.x, 0, f2.y) + 0.5f;
                pos.ValueRW.Value = f44;
            }
        }
    }
    [GenerateTestsForBurstCompatibility]
    struct PathFindJob : IJob
    {
        public NativeArray<bool> road;
        public int2 now;
        public int2 target;
        public NativeList<int2> paths;
        public NativeArray<int> finded;
        public int mark;
        public NativeList<FindData> tmp;
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
    }
}

[GenerateTestsForBurstCompatibility]
struct FindData
{
    public int index;
    public int2 xy;
}
