using System;
using OfficeOpenXml;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

class Program
{
    public static bool debug = true;

    public static bool compress = true;
    public static string codePath = Environment.CurrentDirectory + "/../../../../../Client/Assets/Code/Main/_Gen/";
    public static string assetsPath = Environment.CurrentDirectory + "/../../../../../Client/Assets/Res/Config/Tabs/";
    public static string excelPath = Environment.CurrentDirectory + "/../../../../../../Excel/main";
    public static string TabName = "TabM";
    public static bool genMapping = true;
    public static bool genEcs = true;
    public static int type = 0;
    public static bool ClearBytes = true;

    static void Main(string[] args)
    {
        debug = bool.Parse(args[0]);

        if (!debug)
        {
            compress = bool.Parse(Environment.GetEnvironmentVariable(nameof(compress)));
            codePath = Environment.GetEnvironmentVariable(nameof(codePath));
            assetsPath = Environment.GetEnvironmentVariable(nameof(assetsPath));
            excelPath = Environment.GetEnvironmentVariable(nameof(excelPath));
            TabName = Environment.GetEnvironmentVariable(nameof(TabName));
            genMapping = bool.Parse(Environment.GetEnvironmentVariable(nameof(genMapping)));
            genEcs = bool.Parse(Environment.GetEnvironmentVariable(nameof(genEcs)));
            type = int.Parse(Environment.GetEnvironmentVariable(nameof(type)));
            ClearBytes = bool.Parse(Environment.GetEnvironmentVariable(nameof(ClearBytes)));
        }

        if (ClearBytes)
        {
            foreach (var item in Directory.GetFiles(assetsPath, "*.bytes"))
                File.Delete(item);
        }

        ExcelPackage.LicenseContext = LicenseContext.Commercial;
        //main
        if (type == 0)
        {
            Console.WriteLine("--->" + TabName);
            CodeGen gen = new CodeGen();
            gen.name = TabName;
            gen.excelPath = excelPath;
            gen.codePath = codePath;
            gen.dataPath = assetsPath;
            gen.genMapping = genMapping;
            gen.genEcs = genEcs;
            gen.Gen();
        }
        else if (type == 1)
        //Language表 
        {
            Console.WriteLine("--->Language");
            List<string> mains = Common.getFiles(excelPath);

            var _common = mains.Find(t => new FileInfo(t).Name == "_Common.xlsx");
            if (_common == null)
            {
                Console.WriteLine("没有包含定义表 _Common.xlsx");
                Console.ReadLine();
                return;
            }
            FileInfo fih = new FileInfo(_common);
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

                List<int> lines = new List<int>();
                var array = (object[,])pkg.Workbook.Worksheets[0].Cells.Value;
                int len = array.GetLength(0);
                for (int j = 3; j <= len; j++)
                {
                    if (!string.IsNullOrEmpty(pkg.Workbook.Worksheets[0].Cells[j, 1].Text))
                        lines.Add(j);
                }

                temp3 tt = new temp3();
                tt.fi = fi;
                tt.dataLines = lines;
                tt.isCommonTab = fi.Name == "_Common.xlsx";

                temp3Lst.Add(tt);
            }
            foreach (var item in temp3Lst)
            {
                ExcelPackage pkg = new ExcelPackage(item.fi);
                Console.WriteLine("开始解析->" + item.fi.Name);

                for (int j = 0; j < tag.Count; j++)
                {
                    int x = -1;
                    var array = (object[,])pkg.Workbook.Worksheets[0].Cells.Value;
                    int len = array.GetLength(1);
                    for (int k = 1; k <= len; k++)
                    {
                        if (pkg.Workbook.Worksheets[0].Cells[2, k].Text == tag[j])
                        {
                            x = k;
                            break;
                        }
                    }
                    if (x == -1)
                    {
                        Console.WriteLine($"{item.fi.Name} 没有配置{tag[j]}的语言列");
                        Console.ReadLine();
                        return;
                    }

                    var lan_1 = lan[tag[j]];
                    for (int i = 0; i < item.dataLines.Count; i++)
                    {
                        var str = pkg.Workbook.Worksheets[0].Cells[item.dataLines[i], 1].Text;
                        if (item.isCommonTab)
                        {
                            if (pkg.Workbook.Worksheets[0].Cells[item.dataLines[i], 3].Text == "[]")
                            {
                                string s = pkg.Workbook.Worksheets[0].Cells[item.dataLines[i], x].Text;
                                string[] arr = s.Split(Common.arraySplit, StringSplitOptions.RemoveEmptyEntries);
                                lan_1.kv3.Add(new KV3 { key = "gk_" + str, v = arr });
                            }
                            else
                            {
                                lan_1.kv2.Add(new KV2 { key = "gk_" + str, v = pkg.Workbook.Worksheets[0].Cells[item.dataLines[i], x].Text });
                            }
                            continue;
                        }
                        if (int.TryParse(str, out var key))
                        {
                            lan_1.kv.Add(new KV { key = key, v = pkg.Workbook.Worksheets[0].Cells[item.dataLines[i], x].Text });
                        }
                        else
                        {
                            if (str.StartsWith("gk_"))
                            {
                                Console.WriteLine($"以[gk_]开头的id是_Common表的保留id 请勿使用 ->{item.fi.Name}");
                                Console.ReadLine();
                                return;
                            }
                            lan_1.kv2.Add(new KV2 { key = str, v = pkg.Workbook.Worksheets[0].Cells[item.dataLines[i], x].Text });
                        }
                    }
                }
            }
            DBuffer buffer = new DBuffer(100000);
            foreach (var item in lan)
            {
                buffer.Seek(0);
                buffer.WriteHeaderInfo();

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

                File.WriteAllBytes(assetsPath + $"/Language_{item.Key}.bytes", buffer.ToBytes());
            }
            {
                var item = temp3Lst.Find(t => t.fi.Name == "_Common.xlsx");
                StringBuilder str = new(10000);
                str.AppendLine("using System.Collections.Generic;");
                str.AppendLine("using System;");
                str.AppendLine("static class TabCommonLanguage");
                str.AppendLine("{");
                for (int i = 0; i < item.dataLines.Count; i++)
                {
                    string s = pkgh.Workbook.Worksheets[0].Cells[item.dataLines[i], 1].Text;
                    if (pkgh.Workbook.Worksheets[0].Cells[item.dataLines[i], 3].Text == "[]")
                        str.AppendLine($"    public static string[] {s} => \"gk_{s}\".ToLans();");
                    else
                        str.AppendLine($"    public static string {s} => \"gk_{s}\".ToLan();");
                }
                str.AppendLine("}");
                File.WriteAllText(codePath + "TabCommonLanguage.cs", str.ToString());
            }
        }

        Console.WriteLine("生成成功");
        Console.WriteLine();
    }
}
