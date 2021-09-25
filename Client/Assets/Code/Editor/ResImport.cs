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
    [MenuItem("xx/xx")]
    static void test()
    {
        Texture2D tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, Color.white);
        tex.Apply();
        string path = Application.dataPath + "/Res/UI/UISprite/mask.png";
        File.WriteAllBytes(path, tex.EncodeToPNG());
    }
    [MenuItem("WTool/CreateUICode")]
    static void CreateUICode()
    {
        if (Selection.gameObjects == null || Selection.gameObjects.Length == 0)
        {
            Loger.Error("未选中UI对象");
            return;
        }
        foreach (var item in Selection.gameObjects)
        {
            if (item.GetComponent<Canvas>() == null)
            {
                Loger.Error("选中的不是Canvas对象");
                return;
            }
        }

        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            StringBuilder str = new StringBuilder();
            StringBuilder str2 = new StringBuilder();
            GameObject go = Selection.gameObjects[i];

            str.AppendLine(@"using System;");
            str.AppendLine(@"using System.Collections.Generic;");
            str.AppendLine(@"using System.Linq;");
            str.AppendLine(@"using System.Text;");
            str.AppendLine(@"using System.Threading.Tasks;");
            str.AppendLine(@"using Main;");
            str.AppendLine(@"using UnityEngine;");
            str.AppendLine(@"using UnityEngine.UI;");
            str.AppendLine(@"using Game;");
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
                    coms.RemoveAll(t=>
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

                    foreach (var item in coms)
                    {
                        str.AppendLine($@"    public {item.GetType().FullName} {item.name}{item.GetType().Name};");
                        str2.AppendLine($@"        this.{item.name}{item.GetType().Name} = this.UI.transform.Find(""{p}"").GetComponent(typeof({item.GetType().FullName})) as {item.GetType().FullName};");
                    }
                }
            }

            str.AppendLine(@"");
            str.AppendLine(@"    public override void Binding()");
            str.AppendLine(@"    {");
            str.AppendLine(str2.ToString());
            str.AppendLine(@"    }");
            str.Append(@"}");

            File.WriteAllText(Application.dataPath + $"/Code/HotFix/Game/UI/Auto/{go.name}.cs", str.ToString());
        }

        AssetDatabase.Refresh();
    }
}
