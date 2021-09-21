using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MAppSetting : MonoBehaviour
{
    public bool Debug;
    public NetModel NetModel;


    private void OnEnable()
    {
        Main.MGameSetting.Debug = Debug;
        Main.MGameSetting.NetModel = NetModel;
    }
}
