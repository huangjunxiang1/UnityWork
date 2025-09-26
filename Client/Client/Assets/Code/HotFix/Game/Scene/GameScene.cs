using main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;

namespace Game
{
    [Scene("Game")]
    class GameScene:Scene
    {
        public override async void OnCreate(params object[] os)
        {
            BaseCamera.SetCamera(new FreedomCamera(Camera.main.GetComponent<CinemachineBrain>()));
            GameObject cm = new("CMTarget");
            cm.transform.position = new Vector3(0, 0, 0);
            BaseCamera.Current.Init(cm);
            BaseCamera.Current.EnableCamera();

            var buffer = new DBuffer(Pkg.LoadRaw("raw_Game"));
            Client.Data.Add(new AStarData(buffer));

            var s = Client.Data.Get<S2C_JoinRoom>();
            for (int i = 0; i < s.units.Count; i++)
            {
                var v = s.units[i];
                SGameObject go = new() { ActorId = v.id, Group = 1 };
                this.AddChild(go);
                go.GameObject.SetGameObject("3D_chan");
                go.Transform.position = v.t.p;
                go.Transform.rotation = v.t.r;
                if (go.ActorId == s.myid)
                    go.AddComponent<GameInputComponent>();
                go.KV.Set(v.attribute);
                go.AddComponent<PathFindingAStarComponent>();
                go.AddComponent<PathFindingNodeComponent>();
                go.AddComponent<MoveToComponent>();
            }
            await Client.UI.OpenAsync<FUIGame>();
        }
        public override void Dispose()
        {
            base.Dispose();
            BaseCamera.Current?.Dispose();
        }
        [Event]
        void join(S2C_PlayerJoinRoom t)
        {
            var v = t.info;
            SGameObject go = new() { ActorId = v.id };
            Client.Scene.Current.AddChild(go);
            go.GameObject.SetGameObject("3D_chan");
            go.Transform.position = v.t.p;
            go.Transform.rotation = v.t.r;
            go.KV.Set(v.attribute);
        }
        [Event]
        void S2C_PlayerQuit(S2C_PlayerQuit e)
        {
            Client.Scene.Current.GetChildByActorId(e.id)?.Dispose();
        }
    }
}
