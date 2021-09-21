using Main;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class MInit
{
    static long UnitId;
    static Dictionary<long, GameObject> units = new Dictionary<long, GameObject>();
    static Dictionary<long, float> speeds = new Dictionary<long, float>();
    public static async void Init()
    {
        SynchronizationContext.SetSynchronizationContext(ThreadSynchronizationContext.Instance);

        var m = typeof(MInit).GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        TypesCache.AddTypes(typeof(MInit).Assembly.GetTypes());
        SysEvent.AutoRigisterEvent();

        Main.SceneHelper.LoadScene("Main");

        SysNet.Connect(NetType.KCP, MUtil.ToIPEndPoint(MGameSetting.LoginAddress));

        ProtoBuf.Serializer.NonGeneric.PrepareSerializer(typeof(C2R_Login));

        var msg = new C2R_Login() { Account = "t1", Password = "1" };
        R2C_Login r2CLogin = (await SysNet.SendAsync(msg)) as R2C_Login;

        SysNet.DisConnect();
        SysNet.Connect(NetType.KCP, MUtil.ToIPEndPoint(r2CLogin.Address));
        var C2G_LoginGate = new C2G_LoginGate() { Key = r2CLogin.Key, GateId = r2CLogin.GateId };
        G2C_LoginGate G2C_LoginGate = (await SysNet.SendAsync(C2G_LoginGate)) as G2C_LoginGate;

        G2C_EnterMap G2C_EnterMap = (await SysNet.SendAsync(new C2G_EnterMap())) as G2C_EnterMap;
        UnitId = G2C_EnterMap.UnitId;
        createUnits(G2C_EnterMap.Units);
    }

    public void t1()
    {

    }
    void t2()
    {

    }

    [Event(typeof(G2C_LoginGate))]
    static async void login(IResponse response)
    {
        G2C_LoginGate G2C_LoginGate = response as G2C_LoginGate;
        while (true)
        {
            await Task.Delay(1000);
            await SysNet.SendAsync(new C2G_Ping());
        }
    }
    [Event(typeof(G2C_Ping))]
    static void ping(IResponse response)
    {
        G2C_Ping G2C_Ping = response as G2C_Ping;
        Timer.ServerTime = G2C_Ping.Time;
    }

    /* public static void GetRecv(object response)
     {
         Loger.Log("getMessage->" + response);
         if (response is G2C_Ping)
         {
             var req = response as G2C_Ping;
             Timer.ServerTime = req.Time;

         }
         else if (response is R2C_Login)
         {
             var r2CLogin = response as R2C_Login;
             SysNet.DisConnect();
             SysNet.Connect(NetType.KCP, MUtil.ToIPEndPoint(r2CLogin.Address));

             var C2G_LoginGate = new C2G_LoginGate() { Key = r2CLogin.Key, GateId = r2CLogin.GateId };
             SysNet.Send(C2G_LoginGate);
         }
         else if (response is G2C_LoginGate)
         {
             G2C_LoginGate G2C_LoginGate = response as G2C_LoginGate;
             if (G2C_LoginGate.Error != 0)
             {
                 Loger.Error("登陆gate失败 coed:" + G2C_LoginGate.Error + " message:" + G2C_LoginGate.Message);
                 return;
             }
             Loger.Log("登陆gate成功!");

             var e = new C2G_EnterMap();
             SysNet.Send(e);

             float time = 0;
             Timer.Add(0, ()=>
             {
                 time += Time.deltaTime;
                 if (time>=1)
                 {
                     time = 0;
                     C2G_Ping p = new C2G_Ping();
                     SysNet.Send(p);
                 }
             });
         }
         else if (response is G2C_EnterMap)
         {
             G2C_EnterMap G2C_EnterMap = response as G2C_EnterMap;
             UnitId = G2C_EnterMap.UnitId;
             createUnits(G2C_EnterMap.Units);
         }
         else if (response is M2C_CreateUnits)
         {
             M2C_CreateUnits M2C_CreateUnits = response as M2C_CreateUnits;
             createUnits(M2C_CreateUnits.Units);
         }
         else if (response is M2C_PathfindingResult)
         {
             M2C_PathfindingResult ret = response as M2C_PathfindingResult;
             if (!units.TryGetValue(ret.Id, out var go))
             {
                 Loger.Error("null unit");
                 return;
             }

             Timer.Remove(act);
             float speed = speeds[ret.Id];
             float totalTime = 0;
             float curTime = 0;
             List<float> times = new List<float>();
             for (int i = 0; i < ret.Xs.Count - 1; i++)
             {
                 Vector3 pos1 = new Vector3(ret.Xs[i], ret.Ys[i], ret.Zs[i]);
                 Vector3 pos2 = new Vector3(ret.Xs[i + 1], ret.Ys[i + 1], ret.Zs[i + 1]);
                 totalTime += (pos2 - pos1).magnitude / speed * 1000;
                 times.Add(totalTime);
             }

             act = () =>
             {
                 float t = curTime += Time.deltaTime;
                 int idx = times.FindIndex(s => s >= t);
                 if (idx >= times.Count - 1 || idx == -1) idx = times.Count - 2;
                 go.transform.position = Vector3.Lerp(new Vector3(ret.Xs[idx], ret.Ys[idx], ret.Zs[idx])
                     , new Vector3(ret.Xs[idx + 1], ret.Ys[idx + 1], ret.Zs[idx + 1])
                     , t - times[idx]);
             };
             Timer.Add(0, act);
         }
     }
     static Action act = null;*/
    static void createUnits(List<UnitInfo> Units)
    {
        foreach (var item in Units)
        {
            int idx = item.Ks.FindIndex(t => t == 1000);
            speeds[item.UnitId] = item.Vs[idx];
            if (!units.TryGetValue(item.UnitId, out var go))
            {
                go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                units[item.UnitId] = go;
            }
            go.transform.position = new Vector3(item.X, item.Y, item.Z);
        }
    }
}
