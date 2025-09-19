using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal static class toLanguage
{
    public static void Excute()
    {
        Console.WriteLine("--->Language");
        List<string> mains = Common.getFiles(Program.excelPath);

        FileInfo fih = new FileInfo(mains.FirstOrDefault());
        ExcelPackage pkgh = new ExcelPackage(fih);
        List<string> tag = new();
        Common.GetHead(tag, pkgh);
        Dictionary<string, temp> lan = new();
        for (int i = 0; i < tag.Count; i++)
        {
            var t = lan[tag[i]] = new();
        }
        List<temp3> temp3Lst = new List<temp3>();
        foreach (string path in mains)
        {
            FileInfo fi = new FileInfo(path);
            ExcelPackage pkg = new ExcelPackage(fi);

            temp3 tt = new temp3();
            tt.fi = fi;
            for (int i = 0; i < pkg.Workbook.Worksheets.Count; i++)
            {
                List<int> lines = new List<int>();
                var array = (object[,])pkg.Workbook.Worksheets[i].Cells.Value;
                int len = array.GetLength(0);
                for (int j = 3; j <= len; j++)
                {
                    if (!string.IsNullOrEmpty(pkg.Workbook.Worksheets[i].Cells[j, 1].Text))
                        lines.Add(j);
                }

                tt.dataLines.Add((i, lines));
            }

            tt.genCS = fi.Name.StartsWith("_");

            temp3Lst.Add(tt);
        }
        foreach (var item in temp3Lst)
        {
            ExcelPackage pkg = new ExcelPackage(item.fi);
            Console.WriteLine("开始解析->" + item.fi.Name);

            for (int j = 0; j < tag.Count; j++)
            {
                var lan_1 = lan[tag[j]];
                for (int s = 0; s < item.dataLines.Count; s++)
                {
                    int sheet = item.dataLines[s].Item1;
                    var dataLines = item.dataLines[s].Item2;
                    int x = -1;
                    var array = (object[,])pkg.Workbook.Worksheets[sheet].Cells.Value;
                    int len = array.GetLength(1);
                    for (int k = 1; k <= len; k++)
                    {
                        if (pkg.Workbook.Worksheets[sheet].Cells[2, k].Text == tag[j])
                        {
                            x = k;
                            break;
                        }
                    }
                    if (x == -1)
                    {
                        Console.WriteLine($"{item.fi.Name} sheet:{pkg.Workbook.Worksheets[sheet].Name} 没有配置{tag[j]}的语言列");
                        Console.ReadLine();
                        return;
                    }

                    for (int i = 0; i < dataLines.Count; i++)
                    {
                        var str = pkg.Workbook.Worksheets[sheet].Cells[dataLines[i], 1].Text;
                        if (item.genCS)
                        {
                            if (pkg.Workbook.Worksheets[sheet].Cells[dataLines[i], 2].Text == "[]")
                            {
                                string text = pkg.Workbook.Worksheets[sheet].Cells[dataLines[i], x].Text;
                                string[] arr = text.Split(Common.arraySplit, StringSplitOptions.RemoveEmptyEntries);
                                lan_1.kv3.Add(new KV3 { key = "gk_" + str, v = arr });
                            }
                            else
                            {
                                lan_1.kv2.Add(new KV2 { key = "gk_" + str, v = pkg.Workbook.Worksheets[sheet].Cells[dataLines[i], x].Text });
                            }
                            continue;
                        }
                        if (int.TryParse(str, out var key))
                        {
                            lan_1.kv.Add(new KV { key = key, v = pkg.Workbook.Worksheets[sheet].Cells[dataLines[i], x].Text });
                        }
                        else
                        {
                            if (str.StartsWith("gk_"))
                            {
                                Console.WriteLine($"以[gk_]开头的id是代码导出表的保留id 请勿使用 ->{item.fi.Name}");
                                Console.ReadLine();
                                return;
                            }
                            lan_1.kv2.Add(new KV2 { key = str, v = pkg.Workbook.Worksheets[sheet].Cells[dataLines[i], x].Text });
                        }
                    }
                }
            }
        }
        DBuffer buffer = new DBuffer(100000);
        foreach (var item in lan)
        {
            buffer.Seek(11);

            buffer.Write(item.Value.kv.Count);
            for (int i = 0; i < item.Value.kv.Count; i++)
            {
                buffer.Write(item.Value.kv[i].key);
                buffer.Write(item.Value.kv[i].v);
            }
            buffer.Write(item.Value.kv2.Count);
            for (int i = 0; i < item.Value.kv2.Count; i++)
            {
                buffer.Write(item.Value.kv2[i].key);
                buffer.Write(item.Value.kv2[i].v);
            }
            buffer.Write(item.Value.kv3.Count);
            for (int i = 0; i < item.Value.kv3.Count; i++)
            {
                buffer.Write(item.Value.kv3[i].key);
                buffer.Write(item.Value.kv3[i].v);
            }
            int len = buffer.Length;
            buffer.Seek(0);
            buffer.WriteHeaderInfo();
            buffer.Seek(len);

            File.WriteAllBytes(Program.assetsPath + $"/Language_{item.Key}.bytes", buffer.ToBytes());
        }
        {
            StringBuilder str = new(10000);
            str.AppendLine("using System.Collections.Generic;");
            str.AppendLine("using System;");
            str.AppendLine("static class TabText");
            str.AppendLine("{");
            for (int index = 0; index < temp3Lst.Count; index++)
            {
                var item = temp3Lst[index];
                if (!item.genCS) continue;
                ExcelPackage pkg = new ExcelPackage(item.fi);
                for (int i = 0; i < item.dataLines.Count; i++)
                {
                    int sheet = item.dataLines[i].Item1;
                    var dataLines = item.dataLines[i].Item2;
                    for (int j = 0; j < dataLines.Count; j++)
                    {
                        string s = pkg.Workbook.Worksheets[sheet].Cells[dataLines[j], 1].Text;
                        str.AppendLine("    /// <summary>");
                        str.AppendLine($"    /// {pkg.Workbook.Worksheets[sheet].Cells[dataLines[j], 3].Text}");
                        str.AppendLine("    /// </summary>");
                        if (pkg.Workbook.Worksheets[sheet].Cells[dataLines[j], 2].Text == "[]")
                            str.AppendLine($"    public static string[] {s} => \"gk_{s}\".ToLans();");
                        else
                            str.AppendLine($"    public static string {s} => \"gk_{s}\".ToLan();");
                    }
                }
            }
            str.AppendLine("}");
            File.WriteAllText(Program.codePath + "TabText.cs", str.ToString());
        }

        foreach (var item in Directory.GetFiles($"{Program.excelPath}/UIText", "*.txt"))
        {
            File.WriteAllText($"{Program.assetsPath}/{new FileInfo(item).Name}", File.ReadAllText(item));
        }
    }
}
