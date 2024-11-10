using Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game
{
    public class Scene : Core.STree
    {
        public int SceneID { get; private set; }
        public int Mask { get; private set; }

        public STask InLoginScene() => InScene(1, 1, "Login");
        public STask InMainScene() => InScene(2, 1, "Login");
        public async STask InScene(int sceneId, int mask, string name)
        {
            if (SceneID == sceneId) return;

            this.DisposeAllChildren();
            Client.UI.CloseUI();
            await World.Event.RunEventAsync(new EC_OutScene { sceneId = SceneID, sceneType = this.Mask });

            this.SceneID = sceneId;
            this.Mask = mask;

            SAsset.ReleasePoolsGameObjects();
            await SAsset.ReleaseAllUnuseObjects();
            await SAsset.LoadSceneAsync($"scene_{name}");
            await STask.Delay(100);

            await World.Event.RunEventAsync(new EC_InScene { sceneId = SceneID, sceneType = this.Mask });
        }
    }
}
