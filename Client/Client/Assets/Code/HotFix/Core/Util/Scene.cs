using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    class Scene : SObject
    {
        public int SceneID { get; private set; }

        [Event] STask EC_GameStart(EC_GameStart e) => InLoginScene();

        [Event]
        void QuitGame(EC_QuitGame e)
        {
            if (Application.isEditor)
                GameM.Event.RunEvent(new EC_OutScene { sceneId = SceneID, sceneType = TabL.GetScene(SceneID).type });
        }

        public STask InLoginScene(bool showLoading = true) => InScene(1, showLoading);
        public STask InMainScene(bool showLoading = true) => InScene(2, showLoading);
        public async STask InScene(int sceneId, bool showLoading = true)
        {
            if (SceneID == sceneId) return;

            if (showLoading) await GameL.UI.OpenAsync<FUILoading>();
            GameL.UI.CloseAll(ui => ui.uiConfig.CloseIfChangeScene);
            GameM.World.DisposeAllChildren();

            GameM.Event.RunEvent(new EC_OutScene { sceneId = SceneID, sceneType = TabL.GetScene(SceneID).type });
            SceneID = sceneId;

            SAsset.ReleasePoolsGameObjects();
            await SAsset.LoadSceneAsync($"Scene/{TabL.GetScene(SceneID).name}.unity");
            await Task.Delay(100);

            await GameM.Event.RunEventAsync(new EC_InScene { sceneId = SceneID, sceneType = TabL.GetScene(SceneID).type });
            if (showLoading) GameL.UI.Get<FUILoading>().max = 1;
        }
    }
}