using Cinemachine;
using Main;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

static class Handler
{
    [Event]
    static void EC_InScene(EC_InScene e)
    {
        if (e.sceneId > 10000)
        {
            BaseCamera.SetCamera(new FreedomCamera(Camera.main.GetComponent<CinemachineBrain>()));
            GameObject cm = new("CMTarget");
            cm.transform.position = new Vector3(0, 0, 0);
            BaseCamera.Current.Init(cm);
            BaseCamera.Current.EnableCamera();
        }
    }
    [Event]
    static void EC_HotFixInit(EC_HotFixInit e)
    {
        Game.ECSSingle.Init();
        Application.targetFrameRate = -1;
        FairyGUI.UIConfig.defaultFont = "Impact";
    }
    [Event]
    static void EC_QuitGame(EC_QuitGame e)
    {
        Game.ECSSingle.Dispose();
    }

}
