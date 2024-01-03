using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    class Scene : SObject
    {
        public int SceneID { get; private set; }

        [Event]
        async STask EC_GameStart(EC_GameStart e)
        {
            await InLoginScene();
        }
        [Event]
        void QuitGame(EC_QuitGame e)
        {
            if (Application.isEditor)
                GameM.Event.RunEvent(new EC_OutScene { sceneId = SceneID, sceneType = TabL.GetScene(SceneID).type });
        }

        public async STask InLoginScene(bool showLoading = true)
        {
            if (SceneID == 1) return;

            if (showLoading) await GameL.UI.OpenAsync<FUILoading>();
            GameL.UI.CloseAll();
            GameM.World.DisposeAllChildren();

            GameM.Event.RunEvent(new EC_OutScene { sceneId = SceneID, sceneType = TabL.GetScene(SceneID).type });
            SceneID = 1;

            await SceneManager.LoadSceneAsync(TabL.GetScene(SceneID).name).AsTask();
            await Task.Delay(100);//场景加载时 会有一帧延迟才能find场景的GameObject

            await GameL.UI.OpenAsync<FUILogin>();
            if (showLoading) GameL.UI.Get<FUILoading>().max = 1;

            GameM.Event.RunEvent(new EC_InScene { sceneId = SceneID, sceneType = TabL.GetScene(SceneID).type });
        }
        public async STask InScene(int sceneId, bool showLoading = true)
        {
            if (sceneId <= 1 || SceneID == sceneId) return;

            if (showLoading) await GameL.UI.OpenAsync<FUILoading>();
            GameL.UI.CloseAll();
            GameM.World.DisposeAllChildren();

            GameM.Event.RunEvent(new EC_OutScene { sceneId = SceneID, sceneType = TabL.GetScene(SceneID).type });
            SceneID = sceneId;

            await SceneManager.LoadSceneAsync(TabL.GetScene(sceneId).name).AsTask();
            await Task.Delay(100);

            if (showLoading) GameL.UI.Get<FUILoading>().max = 1;

            GameM.Event.RunEvent(new EC_InScene { sceneId = SceneID, sceneType = TabL.GetScene(SceneID).type });
        }
    }
}