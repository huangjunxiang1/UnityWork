using Cinemachine;
using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    class Scene : EntityL
    {
        public int SceneID { get; private set; }

        [Event((int)EventIDM.QuitGame)]
        void QuitGame()
        {
            if (Application.isEditor)
                GameM.Event.ExecuteEvent((int)EventIDM.OutScene);
            GameM.Net.DisConnect();
        }

        public async TaskAwaiter InLoginScene()
        {
            if (SceneID == 1) return;

            var ui = await GameL.UI.OpenAsync<FUILoading>();
            BaseCamera.Current?.DisableCamera();
            GameL.UI.CloseAll();
            GameM.World.RemoveAllChildren();

            GameM.Event.ExecuteEvent((int)EventIDM.OutScene, SceneID);
            SceneID = 1;

            await SceneManager.LoadSceneAsync(TabL.GetScene(SceneID).name);
            await Task.Delay(100);//场景重复加载时 会有一帧延迟才能find场景的GameObject

            await GameL.UI.OpenAsync<FUILogin>();
            ui.max = 1;

            GameM.Event.ExecuteEvent((int)EventIDM.InScene, SceneID);
        }
        public async TaskAwaiter InScene(int sceneId)
        {
            if (sceneId <= 1 || SceneID == sceneId) return;

            var ui = await GameL.UI.OpenAsync<FUILoading>();
            GameL.UI.CloseAll();
            GameM.World.RemoveAllChildren();

            GameM.Event.ExecuteEvent((int)EventIDM.OutScene, sceneId);
            SceneID = sceneId;

            await SceneManager.LoadSceneAsync(TabL.GetScene(sceneId).name);
            await Task.Delay(100);

            ui.max = 1;

            GameM.Event.ExecuteEvent((int)EventIDM.InScene, sceneId);
            BaseCamera.SetCamera(new FreedomCamera(Camera.main.GetComponent<CinemachineBrain>()));
            GameObject cm = new GameObject("CMTarget");
            cm.transform.position = new Vector3(0, 0, 0);
            BaseCamera.Current.Init(cm);
            BaseCamera.Current.EnableCamera();
        }
    }
}