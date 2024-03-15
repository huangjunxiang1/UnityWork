using Cinemachine;
using Core;
using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

partial class FUIFighting5
{
    protected override void OnEnter(params object[] data)
    {
        base.OnEnter(data);

        this._demo.onChanged.Add(demo);
        _btnBack.onClick.Add(_clickBack);
        _reversion.onClick.Add(_revesion);
    }
    void _clickBack()
    {
        _ = GameL.Scene.InLoginScene();
    }

    List<PlayerCTRCompnent> list = new();
    SGameObject s2;
    int last = -1;
    async void demo()
    {
        if (this._demo.selectedIndex == 1)
        {
            for (int i = 0; i < 5; i++)
            {
                var s = new SGameObject();
                s.Transform.position = new Vector3(UnityEngine.Random.Range(0f, 5f), 0, UnityEngine.Random.Range(0f, 5f));
                list.Add(s.AddComponent<PlayerCTRCompnent>());
                await s.GameObject.LoadGameObjectAsync("3D/Model/Unit/chan.prefab");
                await STask.Delay(5000);
            }
            for (int i = 0; i < list.Count; i++)
            {
                list[i].Dispose();
                await STask.Delay(5000);
            }
        }
        else if (last == 1)
        {
            for (int i = 0; i < list.Count; i++)
                list[i].Entity.Dispose();
            list.Clear();
        }

        if (this._demo.selectedIndex == 2)
        {
            s2 = new SGameObject();
            await s2.GameObject.LoadGameObjectAsync("3D/Model/Unit/chan.prefab");
            s2.AddComponent<PlayerCTRCompnent>();
            s2.AddComponent<CameraFollowComponent>();
        }
        else if (last == 2)
        {
            s2.Dispose();
            PlayerCTRCompnent.code?.Dispose();
            PlayerCTRCompnent.code = new PlayerCTRCompnent.move1();
        }
        last = this._demo.selectedIndex;
    }

    void _revesion()
    {
        PlayerCTRCompnent.code?.Dispose();

        if (PlayerCTRCompnent.code is PlayerCTRCompnent.move1)
            PlayerCTRCompnent.code = new PlayerCTRCompnent.move2();
        else
            PlayerCTRCompnent.code = new PlayerCTRCompnent.move1();
    }

    class PlayerCTRCompnent : SComponent
    {
        const float speed = 20;
        public static SObject code = new move1();
        public class move1 : SUnityObject
        {
            [Event]
            void update(Update<TransformComponent, PlayerCTRCompnent> a)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    a.t.position += Vector3.forward * 2 * Time.deltaTime;
                    a.t.forward = Vector3.Lerp(a.t.forward, Vector3.forward, Time.deltaTime * speed);
                }
                if (Input.GetKey(KeyCode.S))
                {
                    a.t.position -= Vector3.forward * 2 * Time.deltaTime;
                    a.t.forward = Vector3.Lerp(a.t.forward, -Vector3.forward, Time.deltaTime * speed);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    a.t.position += Vector3.right * 2 * Time.deltaTime;
                    a.t.forward = Vector3.Lerp(a.t.forward, Vector3.right, Time.deltaTime * speed);
                }
                if (Input.GetKey(KeyCode.A))
                {
                    a.t.position -= Vector3.right * 2 * Time.deltaTime;
                    a.t.forward = Vector3.Lerp(a.t.forward, -Vector3.right, Time.deltaTime * speed);
                }
            }
        }
        public class move2 : SUnityObject
        {
            [Event]
            void update(Update<TransformComponent, PlayerCTRCompnent> a)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    a.t.position += -Vector3.forward * 2 * Time.deltaTime;
                    a.t.forward = Vector3.Lerp(a.t.forward, Vector3.forward, Time.deltaTime * speed);
                }
                if (Input.GetKey(KeyCode.S))
                {
                    a.t.position -= -Vector3.forward * 2 * Time.deltaTime;
                    a.t.forward = Vector3.Lerp(a.t.forward, -Vector3.forward, Time.deltaTime * speed);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    a.t.position += -Vector3.right * 2 * Time.deltaTime;
                    a.t.forward = Vector3.Lerp(a.t.forward, Vector3.right, Time.deltaTime * speed);
                }
                if (Input.GetKey(KeyCode.A))
                {
                    a.t.position -= -Vector3.right * 2 * Time.deltaTime;
                    a.t.forward = Vector3.Lerp(a.t.forward, -Vector3.right, Time.deltaTime * speed);
                }
            }
        }
        [Event]
        static void awake(Awake<PlayerCTRCompnent, PlayingComponent> a)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                a.t2.Play("RUN00_F");
                return;
            }
            a.t2.Play("WAIT01");
        }
        [Event]
        static void dispose(Dispose<PlayerCTRCompnent, PlayingComponent> a)
        {
            a.t2.Play("WAIT01");
        }
        [Event]
        static void update(Update<PlayingComponent, PlayerCTRCompnent> a)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
            {
                a.t.Play("RUN00_F");
                return;
            }
            if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
            {
                a.t.Play("WAIT01");
                return;
            }
        }
    }
    class CameraFollowComponent : SComponent
    {
        [Event]
        static void awake(Awake<CameraFollowComponent, GameObjectComponent> t)
        {
            BaseCamera.SetCamera(new LockingCamera(Camera.main.GetComponent<CinemachineBrain>()));
            BaseCamera.Current.Init(t.t2.gameRoot);
            BaseCamera.Current.EnableCamera();
        }
    }
}
