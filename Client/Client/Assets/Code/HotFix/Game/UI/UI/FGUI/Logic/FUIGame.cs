using Game;
using main;
using Spine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.UniversalDelegates;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

partial class FUIGame
{
    NativeList<Entity> es = new NativeList<Entity>(10000, AllocatorManager.Persistent);
    protected override void OnEnter(params object[] data)
    {
        base.OnEnter(data);
        this.Close.onClick.Add(onClose);
        this._replay.onClick.Add(replay);

        //_gen_cube();
    }
    protected override void OnExit()
    {
        base.OnExit();
        var em = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
        if (es.IsCreated)
        {
            em.DestroyEntity(es.AsArray());
            es.Dispose();
        }
    }
    async void onClose()
    {
        NetComponent.Inst.Send(new C2S_PlayerQuit());
        NetComponent.Inst.Dispose();
        await Client.Scene.InScene<LoginScene>();
    }

    async void replay()
    {
        var move = Client.Scene.Current.GetChild<SGameObject>().GetComponent<PathFindingAStarComponent>();
        await move.Goto(new int2(Util.RandomInt(0, 50), Util.RandomInt(0, 50)));
        return;
        /*var p = move.Entity.GetComponent<TransformComponent>();
        List<float3> ps = new();
        ps.Add(p.position);
        ps.Add(p.position + new float3(1, 0, 0));
        ps.Add(p.position + new float3(1, 0, 1));
        ps.Add(p.position + new float3(2, 0, 1));
        ps.Add(p.position + new float3(2, 0, 2));
        ps.Add(p.position + new float3(3, 0, 2));
        ps.Add(p.position + new float3(3, 0, 3));
        ps.Add(p.position + new float3(4, 0, 3));
        ps.Add(p.position + new float3(4, 0, 4));
        move.MoveTo(ps, quaternion.LookRotation(math.back(), math.up()), style: MoveStyle.CatmullRom);*/
        var em = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
        em.DestroyEntity(es.AsArray());
        es.Clear();
        _gen_cube();
    }
    async void _gen_cube()
    {
        var em = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
        (Entity one, GameObject g) = await ECSHelper.LoadEntityAndPrefab(@"3D_Cube");
        var d = Client.Data.Get<S2C_JoinRoom>();

        for (int h = 0; h < SettingM.RoomWeight; h++)
        {
            //柱
            for (int x = 0; x < SettingM.RoomSize.x + 1; x++)
            {
                for (int y = 0; y < SettingM.RoomSize.y + 1; y++)
                {
                    Entity e = em.Instantiate(one);
                    em.SetComponentData(e, new LocalToWorld() { Value = float4x4.Translate(new float3(x * SettingM.SubRoomSize.x, h, y * SettingM.SubRoomSize.y) + 0.5f) });
                    es.Add(e);
                }
            }
            //左墙 和 下墙
            for (int x = 0; x < SettingM.RoomSize.x; x++)
            {
                for (int xx = 0; xx < SettingM.SubRoomSize.x; xx++)
                {
                    Entity e = em.Instantiate(one);
                    em.SetComponentData(e, new LocalToWorld() { Value = float4x4.Translate(new float3(x * SettingM.SubRoomSize.x + xx + 1, h, 0) + 0.5f) });
                    es.Add(e);
                }
            }
            for (int y = 0; y < SettingM.RoomSize.y; y++)
            {
                for (int yy = 0; yy < SettingM.SubRoomSize.y; yy++)
                {
                    Entity e = em.Instantiate(one);
                    em.SetComponentData(e, new LocalToWorld() { Value = float4x4.Translate(new float3(0, h, y * SettingM.SubRoomSize.y + yy + 1) + 0.5f) });
                    es.Add(e);
                }
            }
            //右墙和上墙
            for (int x = 0; x < SettingM.RoomSize.x; x++)
            {
                for (int y = 0; y < SettingM.RoomSize.y; y++)
                {
                    for (int xx = 0; xx < SettingM.SubRoomSize.x; xx++)
                    {
                        Entity e = em.Instantiate(one);
                        em.SetComponentData(e, new LocalToWorld() { Value = float4x4.Translate(new float3(x * SettingM.SubRoomSize.x + xx + 1, h, (y + 1) * SettingM.SubRoomSize.y) + 0.5f) });
                        es.Add(e);
                    }
                    for (int yy = 0; yy < SettingM.SubRoomSize.y; yy++)
                    {
                        Entity e = em.Instantiate(one);
                        em.SetComponentData(e, new LocalToWorld() { Value = float4x4.Translate(new float3((x + 1) * SettingM.SubRoomSize.x, h, y * SettingM.SubRoomSize.y + yy + 1) + 0.5f) });
                        es.Add(e);
                    }
                }
            }
        }
        foreach (var item in d.info.link.Values)
        {
            {
                Entity e = em.Instantiate(one);
                float3 p = new float3(SettingM.SubRoomSize.x * item.xy.x, 1, SettingM.SubRoomSize.y * item.xy.y);
                if (item.dir == 0) p += new float3(1, 0, SettingM.SubRoomSize.y / 2);
                else if (item.dir == 1) p += new float3(SettingM.SubRoomSize.x / 2, 0, 1);
                else if (item.dir == 2) p += new float3(SettingM.SubRoomSize.x - 1, 0, SettingM.SubRoomSize.y / 2);
                else if (item.dir == 3) p += new float3(SettingM.SubRoomSize.x / 2, 0, SettingM.SubRoomSize.y - 1);
                em.SetComponentData(e, new LocalToWorld() { Value = float4x4.Translate(p + 0.5f) });
                float v = item.colorIndex % 7 / 7f;
                float3 v3 = hsv2rgb(new float3(v, 1, 1));
                em.AddComponentData(e, new HDRPMaterialPropertyEmissiveColor2() { Value = new float4(v3, 1) });
                es.Add(e);
            }
            {
                Entity e = em.Instantiate(one);
                float3 p = new float3(SettingM.SubRoomSize.x * item.xy.x, 0, SettingM.SubRoomSize.y * item.xy.y);
                if (item.dir == 0) p += new float3(1, 0, SettingM.SubRoomSize.y / 2);
                else if (item.dir == 1) p += new float3(SettingM.SubRoomSize.x / 2, 0, 1);
                else if (item.dir == 2) p += new float3(SettingM.SubRoomSize.x - 1, 0, SettingM.SubRoomSize.y / 2);
                else if (item.dir == 3) p += new float3(SettingM.SubRoomSize.x / 2, 0, SettingM.SubRoomSize.y - 1);
                em.SetComponentData(e, new LocalToWorld() { Value = float4x4.Translate(p + 0.5f) });
                float v = item.colorIndex / 7 / 7f;
                float3 v3 = hsv2rgb(new float3(v, 1, 1));
                em.AddComponentData(e, new HDRPMaterialPropertyEmissiveColor2() { Value = new float4(v3, 1) });
                es.Add(e);
            }
        }

        g.GetComponent<Renderer>().sharedMaterial.SetFloat("_ecsStartTime", Time.time);
        em.DestroyEntity(one);
    }
    static float3 hsv2rgb(float3 c)
    {
        var rgb = math.clamp(math.abs((c.x * 6.0f + new float3(0.0f, 4.0f, 2.0f)) % 6.0f - 3.0f) - 1.0f, 0.0f, 1.0f);
        //rgb = rgb * rgb * (3.0f - 2.0f * rgb); // cubic smoothing 
        return c.z * math.lerp(1, rgb, c.y);
    }
}
[MaterialProperty("_EmissiveColor", -1)]
public struct HDRPMaterialPropertyEmissiveColor2 : IComponentData, IQueryTypeParameter
{
    public float4 Value;
}
