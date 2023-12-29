using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    class Scene : SObject
    {
        public int SceneID { get; private set; }

        [Event(3, Queue = true)]
        async STask EC_HotFixInit(EC_GameStart e)
        {
            await InLoginScene();
        }
        [Event]
        void QuitGame(EC_QuitGame e)
        {
            if (Application.isEditor)
                SGameM.Event.RunEvent(new EC_OutScene { sceneId = SceneID, sceneType = STabL.GetScene(SceneID).type });
        }

        public async STask InLoginScene(bool showLoading = true)
        {
            if (SceneID == 1) return;

            if (showLoading) await SGameL.UI.OpenAsync<FUILoading>();
            SGameL.UI.CloseAll();
            SGameM.World.DisposeAllChildren();

            SGameM.Event.RunEvent(new EC_OutScene { sceneId = SceneID, sceneType = STabL.GetScene(SceneID).type });
            SceneID = 1;

            await SceneManager.LoadSceneAsync(STabL.GetScene(SceneID).name).AsTask();
            await Task.Delay(100);//场景加载时 会有一帧延迟才能find场景的GameObject

            await SGameL.UI.OpenAsync<FUILogin>();
            if (showLoading) SGameL.UI.Get<FUILoading>().max = 1;

            SGameM.Event.RunEvent(new EC_InScene { sceneId = SceneID, sceneType = STabL.GetScene(SceneID).type });
        }
        public async STask InScene(int sceneId, bool showLoading = true)
        {
            if (sceneId <= 1 || SceneID == sceneId) return;

            if (showLoading) await SGameL.UI.OpenAsync<FUILoading>();
            SGameL.UI.CloseAll();
            SGameM.World.DisposeAllChildren();

            SGameM.Event.RunEvent(new EC_OutScene { sceneId = SceneID, sceneType = STabL.GetScene(SceneID).type });
            SceneID = sceneId;

            await SceneManager.LoadSceneAsync(STabL.GetScene(sceneId).name).AsTask();
            await Task.Delay(100);

            if (showLoading) SGameL.UI.Get<FUILoading>().max = 1;

            SGameM.Event.RunEvent(new EC_InScene { sceneId = SceneID, sceneType = STabL.GetScene(SceneID).type });
        }
    }
}