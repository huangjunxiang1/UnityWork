using System;
using OfficeOpenXml;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

class Program
{
    public const int mark = 20220702;
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
            Common.GetHead(mainIdx, pkgh, out temp1 t);
            Dictionary<int, Lan> lan = new Dictionary<int, Lan>();
            for (int i = 1; i < mainIdx.Count; i++)
            {
                FileInfo fi = new FileInfo(mains[0]);
                ExcelPackage pkg = new ExcelPackage(fi);
                Lan tt = lan[mainIdx[i]] = new Lan();
                tt.buff = new DBuffer(new MemoryStream(100000));
                tt.buff.Compress = false;
                tt.buff.Write(mark);
                tt.buff.Compress = compress;
                tt.buff.Write(tt.buff.Compress);//是否压缩
                string name = pkg.Workbook.Worksheets[0].Cells[3, mainIdx[i]].Text;
                tt.name = name;
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
            for (int i = 1; i < mainIdx.Count; i++)
            {
                int totalCnt = 0;
                foreach (var item in temp3Lst)
                    totalCnt += item.dataLines.Count;
                Lan tt = lan[mainIdx[i]];
                tt.buff.Write(totalCnt);
            }
            foreach (var item in temp3Lst)
            {
                ExcelPackage pkg = new ExcelPackage(item.fi);
                Console.WriteLine("开始解析->" + item.fi.Name);
                for (int i = 1; i < mainIdx.Count; i++)
                {
                    Lan tt = lan[mainIdx[i]];
                    for (int j = 0; j < item.dataLines.Count; j++)
                    {
                        int key = int.Parse(pkg.Workbook.Worksheets[0].Cells[item.dataLines[j], 1].Text);

                        int idx = mainIdx[i];
                        string sType = pkg.Workbook.Worksheets[0].Cells[2, idx].Text.ToLower();
                        string text = pkg.Workbook.Worksheets[0].Cells[item.dataLines[j], idx].Text;

                        tt.buff.Write(key);
                        Common.WriteValue(tt.buff, null, sType, text, item.fi, item.dataLines[j], idx);
                    }
                }
            }
            foreach (var item in lan.Values)
                File.WriteAllBytes(parentPath + $"/../Client/Client/Assets/Res/Config/Tabs/Language_{item.name}.bytes", item.buff.ToBytes());
        }

        Console.WriteLine("生成成功");
        Console.ReadLine();
    }

}
