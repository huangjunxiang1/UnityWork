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
    static void Main(string[] args)
    {
        if (args != null && args.Length > 0)
            bool.TryParse(args[0], out debug);
        else
            debug = true;

        if (args != null && args.Length > 1)
            bool.TryParse(args[1], out compress);
        else
            compress = true;

        string parentPath;
        if (debug)
            parentPath = Environment.CurrentDirectory + "/../../../../../../Excel/";
        else
            parentPath = Environment.CurrentDirectory + "/";

        foreach (var item in Directory.GetFiles(parentPath + "/../Client/Client/Assets/Res/Config/Tabs/"))
            File.Delete(item);

        ExcelPackage.LicenseContext = LicenseContext.Commercial;
        //main
        {
            CodeGen gen = new CodeGen();
            gen.name = "TabM";
            gen.excelPath = parentPath + "/main";
            gen.codePath = parentPath + "/../Client/Client/Assets/Code/Main/_Gen/TabM";
            gen.dataPath = parentPath + "/../Client/Client/Assets/Res/Config/Tabs/TabM";
            gen.genMapping = true;
            gen.genEcs = true;
            gen.Gen();
        }

        //hot
        {
            CodeGen gen = new CodeGen();
            gen.name = "TabL";
            gen.excelPath = parentPath + "/hot";
            gen.codePath = parentPath + "/../Client/Client/Assets/Code/HotFix/_Gen/TabL";
            gen.dataPath = parentPath + "/../Client/Client/Assets/Res/Config/Tabs/TabL";
            gen.Gen();
        }

        //Language表 
        {
            string main = parentPath + "/Language";
            List<string> mains = Common.getFiles(main);

            FileInfo fih = new FileInfo(mains[0]);
            ExcelPackage pkgh = new ExcelPackage(fih);
            List<int> mainIdx = new List<int>();
            Common.GetHead(mainIdx, pkgh);
            Dictionary<int, temp> lan = new Dictionary<int, temp>();
            for (int i = 0; i < mainIdx.Count; i++)
            {
                FileInfo fi = new FileInfo(mains[0]);
                ExcelPackage pkg = new ExcelPackage(fi);

                var t = lan[mainIdx[i]] = new();
                t.name = pkg.Workbook.Worksheets[0].Cells[3, mainIdx[i]].Text;
            }
            List<temp3> temp3Lst = new List<temp3>();
            foreach (string path in mains)
            {
                FileInfo fi = new FileInfo(path);
                ExcelPackage pkg = new ExcelPackage(fi);

                List<int> lines = new List<int>();
                var array = (object[,])pkg.Workbook.Worksheets[0].Cells.Value;
                int len = array.GetLength(0);
                for (int j = 3; j < len; j++)
                {
                    if (!string.IsNullOrEmpty(pkg.Workbook.Worksheets[0].Cells[j + 1, 1].Text))
                        lines.Add(j + 1);
                }

                temp3 tt = new temp3();
                tt.fi = fi;
                tt.dataLines = lines;

                temp3Lst.Add(tt);
            }
            foreach (var item in temp3Lst)
            {
                ExcelPackage pkg = new ExcelPackage(item.fi);
                Console.WriteLine("开始解析->" + item.fi.Name);

                for (int i = 0; i < item.dataLines.Count; i++)
                {
                    var str = pkg.Workbook.Worksheets[0].Cells[item.dataLines[i], 1].Text;
                    if (int.TryParse(str, out var key))
                    {
                        for (int j = 0; j < mainIdx.Count; j++)
                        {
                            int idx = mainIdx[j];
                            lan[idx].kv.Add(new KV { key = key, v = pkg.Workbook.Worksheets[0].Cells[item.dataLines[i], idx].Text });
                        }
                    }
                    else
                    {
                        for (int j = 0; j < mainIdx.Count; j++)
                        {
                            int idx = mainIdx[j];
                            lan[idx].kv2.Add(new KV2 { key = str, v = pkg.Workbook.Worksheets[0].Cells[item.dataLines[i], idx].Text });
                        }
                    }
                }
            }
            DBuffer buffer = new DBuffer(100000);
            foreach (var item in lan.Values)
            {
                buffer.Seek(0);
                buffer.WriteHeaderInfo();

                buffer.Write(item.kv.Count);
                for (int i = 0; i < item.kv.Count; i++)
                {
                    buffer.Write(item.kv[i].key);
                    buffer.Write(item.kv[i].v);
                }
                buffer.Write(item.kv2.Count);
                for (int i = 0; i < item.kv2.Count; i++)
                {
                    buffer.Write(item.kv2[i].key);
                    buffer.Write(item.kv2[i].v);
                }

                File.WriteAllBytes(parentPath + $"/../Client/Client/Assets/Res/Config/Tabs/Language_{item.name}.bytes", buffer.ToBytes());
            }
        }

        Console.WriteLine("生成成功");
        Console.ReadLine();
    }
}
