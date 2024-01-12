using System.Threading.Tasks;
using UnityEngine;

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

            await SAsset.LoadScene($"Scene/{TabL.GetScene(SceneID).name}.unity");
            await Task.Delay(100);//场景加载时 会有一帧延迟才能find场景的GameObject

            if (showLoading) GameL.UI.Get<FUILoading>().max = 1;

            GameM.Event.RunEvent(new EC_InScene { sceneId = SceneID, sceneType = TabL.GetScene(SceneID).type });
        }
        public async STask InMainScene(bool showLoading = true)
        {
            if (SceneID == 2) return;

            if (showLoading) await GameL.UI.OpenAsync<FUILoading>();
            GameL.UI.CloseAll();
            GameM.World.DisposeAllChildren();

            GameM.Event.RunEvent(new EC_OutScene { sceneId = SceneID, sceneType = TabL.GetScene(SceneID).type });
            SceneID = 2;

            await SAsset.LoadScene($"Scene/{TabL.GetScene(SceneID).name}.unity");
            await Task.Delay(100);//场景加载时 会有一帧延迟才能find场景的GameObject

            await GameM.Event.RunEventAsync(new EC_InScene { sceneId = SceneID, sceneType = TabL.GetScene(SceneID).type });
            if (showLoading) GameL.UI.Get<FUILoading>().max = 1;
        }
        public async STask InScene(int sceneId, bool showLoading = true)
        {
            if (sceneId <= 1 || SceneID == sceneId) return;

            if (showLoading) await GameL.UI.OpenAsync<FUILoading>();
            GameL.UI.CloseAll();
            GameM.World.DisposeAllChildren();

            GameM.Event.RunEvent(new EC_OutScene { sceneId = SceneID, sceneType = TabL.GetScene(SceneID).type });
            SceneID = sceneId;

            await SAsset.LoadScene($"Scene/{TabL.GetScene(SceneID).name}.unity");
            await Task.Delay(100);

            await GameM.Event.RunEventAsync(new EC_InScene { sceneId = SceneID, sceneType = TabL.GetScene(SceneID).type });
            if (showLoading) GameL.UI.Get<FUILoading>().max = 1;
        }
    }
}