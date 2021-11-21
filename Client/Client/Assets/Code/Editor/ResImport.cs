using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using UnityEngine.UI;
using System.Text;

public class ResImport 
{

    [MenuItem("Tools/CreateUICode")]
    static void CreateUICode()
    {
        List<GameObject> uis=new List<GameObject>();
        var fis = Directory.GetFiles(Application.dataPath + "/Res/UI/UUI/UIPrefab/");

        foreach (var f in fis)
        {
            FileInfo fi = new FileInfo(f);
            var temp = AssetDatabase.LoadMainAssetAtPath("Assets/Res/UI/UUI/UIPrefab/" + fi.Name);
            if (!(temp is GameObject go))
                continue;
            uis.Add(go);
        }

        StringBuilder str = new StringBuilder();

        str.AppendLine(@"using System;");
        str.AppendLine(@"using System.Collections.Generic;");
        str.AppendLine(@"using System.Linq;");
        str.AppendLine(@"using System.Text;");
        str.AppendLine(@"using System.Threading.Tasks;");
        str.AppendLine(@"using Main;");
        str.AppendLine(@"using UnityEngine;");
        str.AppendLine(@"using UnityEngine.UI;");
        str.AppendLine(@"using Game;");

        foreach (var go in uis)
        {
            if (go.GetComponent<Canvas>() == null)
            {
                Loger.Error(go.name + "不是Canvas对象");
                return;
            }

            StringBuilder str2 = new StringBuilder();
            str.AppendLine(@"");
            str.AppendLine(@$"partial class {go.name} : UUIBase");
            str.AppendLine(@"{");

            var childs = go.GetComponentsInChildren<Transform>(true);
            for (int j = 0; j < childs.Length; j++)
            {
                var child = childs[j];
                if (child.name.StartsWith("_"))
                {
                    var coms = child.GetComponents<Component>().ToList();
                    coms.RemoveAll(t =>
                    {
                        if (t is Mask) return true;
                        if (t is ContentSizeFitter) return true;
                        if (t is Shadow) return true;
                        if (t is Outline) return true;
                        if (t is CanvasRenderer) return true;
                        return false;
                    });
                    if (coms.Find(t => t is Selectable) != null)
                        coms.RemoveAll(t => t is Image);

                    if (coms.Count > 1)
                        coms.Remove(child.GetComponent<Transform>());

                    List<string> paths = new List<string>();
                    var temp = child;
                    paths.Add(temp.name);
                    while (temp.transform.parent != go.transform)
                    {
                        temp = temp.transform.parent;
                        paths.Add(temp.name);
                    }
                    string p = null;
                    for (int k = paths.Count - 1; k >= 0; k--)
                    {
                        p += paths[k];
                        if (k != 0) p += "/";
                    }

                    foreach (var item1 in coms)
                    {
                        str.AppendLine($@"    public {item1.GetType().FullName} {item1.name}{item1.GetType().Name};");
                        str2.AppendLine($@"        this.{item1.name}{item1.GetType().Name} = this.UI.transform.Find(""{p}"").GetComponent(typeof({item1.GetType().FullName})) as {item1.GetType().FullName};");
                    }
                }
            }

            str.AppendLine(@"");
            str.AppendLine(@"    protected override void Binding()");
            str.AppendLine(@"    {");
            str.AppendLine(str2.ToString());
            str.AppendLine(@"    }");
            str.Append(@"}");
        }

        File.WriteAllText(Application.dataPath + $"/Code/HotFix/Game/UI/UGUI/Auto/UUI.cs", str.ToString());
        AssetDatabase.Refresh();
    }
}
