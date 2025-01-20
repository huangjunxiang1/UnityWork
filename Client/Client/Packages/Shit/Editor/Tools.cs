using Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

internal class Tools
{
    [MenuItem("Shit/生成SystemHandle")]
    static void genGroupsCode()
    {
        const int count = 10;
        string path = Application.dataPath + "/../Packages/Shit/Runtime/Core/ECS/Handle/";
        foreach (var item in Directory.GetFiles(path, "*.cs"))
        {
            var fi = new FileInfo(item);
            if (fi.Name.EndsWith("__base.cs")) continue;

            StringBuilder code = new(10000);
            var lines = File.ReadAllLines(item).ToList();
            int begin = lines.FindIndex(t => t.StartsWith("//begin"));
            if (begin == -1) continue;
            int cntBegin = int.Parse(lines[begin].Split('#')[1]) - 1;
            int end = lines.FindIndex(t => t.StartsWith("//end"));
            for (int i = 0; i < begin; i++)
                code.AppendLine(lines[i]);

            for (int cnt = cntBegin; cnt < count; cnt++)
            {
                for (int i = begin + 1; i < end; i++)
                {
                    var s = lines[i];

                    int sIdx = 2;
                    int x = 0, y = -1, tag = -1;
                    string v = string.Empty;
                    x = s.IndexOf('?', x);
                    if (x != -1)
                        y = s.IndexOf('?', x + 1);
                    if (x != -1 && y != -1)
                    {
                        v = s[(x + 1)..y];
                        tag = v.IndexOf('+');
                    }
                    if (tag == -1)
                    {
                        code.AppendLine(s[2..]);
                    }
                    else
                    {
                        while (tag != -1)
                        {
                            var code1 = v[..tag];
                            var code2 = v[(tag + 1)..].Split('#');
                            var replace = code2.Length > 0 ? code2[0].Replace("\\r", "\r\n") : string.Empty;
                            int j_begin = code2.Length > 1 ? int.Parse(code2[1]) : 1;
                            code.Append(s[sIdx..x]);
                            sIdx = y + 1;
                            StringBuilder tmp = new(50);
                            for (int j = j_begin - 1; j < cnt + 1; j++)
                            {
                                if (j > j_begin - 1)
                                    tmp.Append(replace);
                                tmp.Append(code1
                                    .Replace("[T]", "T" + (j > 0 ? j + 1 : ""))
                                    .Replace("[t]", "t" + (j > 0 ? j + 1 : ""))
                                    .Replace("[c]", "c" + (j > 0 ? j + 1 : ""))
                                    );
                            }
                            code.Append(tmp);

                            tag = -1;
                            while (x != -1)
                            {
                                x = s.IndexOf('?', y + 1);
                                if (x != -1)
                                    y = s.IndexOf('?', x + 1);
                                else
                                    y = x;
                                if (x != -1 && y != -1)
                                {
                                    v = s[(x + 1)..y];
                                    tag = v.IndexOf('+');
                                    if (tag != -1)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        code.AppendLine(s[sIdx..]);
                    }
                }
            }
            File.WriteAllText(path + "/_gen/gen_" + fi.Name, code.ToString());
        }
        AssetDatabase.Refresh();
    }
    [MenuItem("Shit/文件夹 同步到x", priority = int.MaxValue - 10)]
    static void SyncDirsTo()
    {
        for (int i = 0; i < ShitSettings.Inst.SyncDirs.Count; i++)
        {
            var d = ShitSettings.Inst.SyncDirs[i];
            FileHelper.SyncDirectories((d.rootIsAppDataPath ? Application.dataPath : null) + d.self, (d.rootIsAppDataPath ? Application.dataPath : null) + d.target);
        }
    }
    [MenuItem("Shit/文件夹 从x同步", priority = int.MaxValue - 10)]
    static void SyncDirsFrom()
    {
        for (int i = 0; i < ShitSettings.Inst.SyncDirs.Count; i++)
        {
            var d = ShitSettings.Inst.SyncDirs[i];
            FileHelper.SyncDirectories((d.rootIsAppDataPath ? Application.dataPath : null) + d.target, (d.rootIsAppDataPath ? Application.dataPath : null) + d.self);
        }
    }
}
