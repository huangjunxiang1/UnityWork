using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using System.IO;
using LitJson;

class Build : EditorWindow
{
    [MenuItem("Tools/Build")]
    static void Open()
    {
        Build window = new Build();
        window.Show();
        window.maxSize = new UnityEngine.Vector2(2000, 1500);
        window.minSize = new UnityEngine.Vector2(200, 100);
        window.position = new UnityEngine.Rect(1000, 500, 1000, 750);
    }

    private void OnEnable()
    {
        string path = Application.dataPath + "/../Pack/AB/Version.txt";

        if (File.Exists(path))
            lastVer = JsonMapper.ToObject<Versions>(File.ReadAllText(path));
        else
        {
            lastVer = new Versions();
            lastVer.Ver = new Version(1, 0, 0);
        }
        nextVer = new Versions();
        nextVer.Ver = new Version(lastVer.Ver.Major, lastVer.Ver.Minor, lastVer.Ver.Build + 1);

        ver1 = nextVer.Ver.Major.ToString();
        ver2 = nextVer.Ver.Minor.ToString();
        ver3 = nextVer.Ver.Build.ToString();


        skin = new GUIStyle(new GUISkin().textField);
        skin.normal.textColor = Color.white;
        skin.alignment = TextAnchor.MiddleCenter;
        skin.clipping = TextClipping.Overflow;

        skin.focused.textColor = Color.white;
        skin.hover.textColor = Color.white;
    }

    GUIStyle skin;
    Versions lastVer;
    Versions nextVer;
    string ver1;
    string ver2;
    string ver3;

    public void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 300, 50), "打资源包", skin))
        {
            res();
        }
        GUI.Box(new Rect(330, 20, 200, 30), "上个资源版本:" + lastVer.Ver.ToString(3), skin);
        GUI.Box(new Rect(550, 20, 100, 30), "目标资源版本:", skin);

        ver1 = GUI.TextField(new Rect(670, 20, 30, 30), ver1, skin);
        ver2 = GUI.TextField(new Rect(710, 20, 30, 30), ver2, skin);
        ver3 = GUI.TextField(new Rect(750, 20, 30, 30), ver3, skin);
    }
    void res()
    {
        if (!int.TryParse(ver1, out int v1))
        {
            EditorUtility.DisplayDialog("错误", "版本号错误","确定");
            return;
        }
        if (!int.TryParse(ver2, out int v2))
        {
            EditorUtility.DisplayDialog("错误", "版本号错误", "确定");
            return;
        }
        if (!int.TryParse(ver3, out int v3))
        {
            EditorUtility.DisplayDialog("错误", "版本号错误", "确定");
            return;
        }

        nextVer.Ver = new Version(v1, v2, v3);
    }
}