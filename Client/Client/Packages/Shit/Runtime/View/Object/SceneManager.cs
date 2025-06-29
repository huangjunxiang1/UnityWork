﻿using Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public class SceneManager : Core.STree
    {
        public int SceneID { get; private set; }
        public int Mask { get; private set; }
        public string SceneName { get; private set; }
        internal object[] objects;

        public STask InLoginScene() => InScene(1, 1, "Login");
        public STask InMainScene() => InScene(2, 1, "Login");
        public async STask InScene(int sceneId, int mask, string name, params object[] os)
        {
            if (SceneID == sceneId) return;

            this.DisposeAllChildren();
            await World.Event.RunEventAsync(new EC_OutScene { sceneId = SceneID, sceneType = this.Mask });

            this.SceneID = sceneId;
            this.Mask = mask;
            this.objects = os;

            SAsset.ReleasePoolsGameObjects();
            await SAsset.ReleaseAllUnuseObjects();
            this.SceneName = name;
            await SAsset.LoadSceneAsync($"scene_{name}");
            await SValueTask.Delay(100);

            await World.Event.RunEventAsync(new EC_InScene { sceneId = SceneID, sceneType = this.Mask });
        }
    }
}
