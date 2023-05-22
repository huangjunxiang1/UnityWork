using Cinemachine;
using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    class Scene : ObjectL
    {
        public int SceneID { get; private set; }

        [Event((int)EventIDM.QuitGame)]
        void QuitGame()
        {
            if (Application.isEditor)
                GameM.Event.RunEvent((int)EventIDM.OutScene, SceneID, TabL.GetScene(SceneID).type);
            GameM.Net.DisConnect();
        }

        public async TaskAwaiter InLoginScene()
        {
            if (SceneID == 1) return;

            var ui = await GameL.UI.OpenAsync<FUILoading>();
            BaseCamera.Current?.Dispose();
            GameL.UI.CloseAll();
            GameM.World.DisposeAllChildren();

            GameM.Event.RunEvent((int)EventIDM.OutScene, SceneID, TabL.GetScene(SceneID).type);
            SceneID = 1;

            await SceneManager.LoadSceneAsync(TabL.GetScene(SceneID).name).AsTask();
            await Task.Delay(100);//场景加载时 会有一帧延迟才能find场景的GameObject

            await GameL.UI.OpenAsync<FUILogin>();
            ui.max = 1;

            GameM.Event.RunEvent((int)EventIDM.InScene, SceneID, TabL.GetScene(SceneID).type);
        }
        public async TaskAwaiter InScene(int sceneId)
        {
            if (sceneId <= 1 || SceneID == sceneId) return;

            var ui = await GameL.UI.OpenAsync<FUILoading>();
            GameL.ChangeScene();
            GameL.UI.CloseAll();
            GameM.World.DisposeAllChildren();

            GameM.Event.RunEvent((int)EventIDM.OutScene, SceneID, TabL.GetScene(SceneID).type);
            SceneID = sceneId;

            await SceneManager.LoadSceneAsync(TabL.GetScene(sceneId).name).AsTask();
            await Task.Delay(100);

            ui.max = 1;

            GameM.Event.RunEvent((int)EventIDM.InScene, sceneId, TabL.GetScene(sceneId).type);
            BaseCamera.SetCamera(new FreedomCamera(Camera.main.GetComponent<CinemachineBrain>()));
            GameObject cm = new GameObject("CMTarget");
            cm.transform.position = new Vector3(0, 0, 0);
            BaseCamera.Current.Init(cm);
            BaseCamera.Current.EnableCamera();
        }
    }
}