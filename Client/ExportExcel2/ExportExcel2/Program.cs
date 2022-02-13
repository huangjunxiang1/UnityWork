using System;
using OfficeOpenXml;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ExportExcel
{
    class Program
    {
        static void Main(string[] args)
        {
            {
                string main = Environment.CurrentDirectory + "/../../../../../Excel/main";
                List<string> mains = getFiles(main);
                StringBuilder TabM_Cs = new StringBuilder(10000);
                StringBuilder TabM_Cs2 = new StringBuilder(10000);
                StringBuilder TabM_Cs3 = new StringBuilder(10000);
                StringBuilder TabM_Cs4 = new StringBuilder(10000);
                StringBuilder TabM_Cs5 = new StringBuilder(10000);

                temp2 mainCS = new temp2();
                DBuffer buffer = new DBuffer();

                {
                    TabM_Cs.AppendLine("using System.Collections.Generic;");
                    TabM_Cs.AppendLine("using UnityEngine;");
                    TabM_Cs.AppendLine("");
                    TabM_Cs.AppendLine("public static class TabM");
                    TabM_Cs.AppendLine("{");

                    TabM_Cs4.AppendLine("    public static void Init(byte[] bytes)");
                    TabM_Cs4.AppendLine("    {");
                    TabM_Cs4.AppendLine("        DBuffer buffer = new DBuffer(bytes);");
                    TabM_Cs4.AppendLine("");
                }

                for (int i = 0; i < mains.Count; i++)
                {
                    List<int> mainIdx = new List<int>();
                    GetHead(mainIdx, mains[i], out temp1 t);
                    FileInfo fi = new FileInfo(mains[i]);
                    Console.WriteLine("开始解析->" + fi.Name);
                    string csName = fi.Name.Split('.')[0];
                    TabM_Cs2.AppendLine("    public static " + csName + "[] " + csName + "Array { get; private set; }");
                    TabM_Cs3.AppendLine("    static Dictionary<" + t.keyType + ", " + csName + "> _map" + csName + ";");
                    TabM_Cs4.AppendLine($"        int len{i} = buffer.ReadInt();");
                    TabM_Cs4.AppendLine("        " + csName + "Array = new " + csName + $"[len{i}];");
                    TabM_Cs4.AppendLine($"        _map{csName} = new Dictionary<{t.keyType}, {csName}>(len{i});");
                    TabM_Cs4.AppendLine($"        for (int i = 0; i < len{i}; i++)");
                    TabM_Cs4.AppendLine("        {");
                    TabM_Cs4.AppendLine($"            var t = new " + csName + "(buffer);");
                    TabM_Cs4.AppendLine($"            " + csName + "Array[i] = t;");
                    TabM_Cs4.AppendLine($"            _map" + csName + $".Add(t.{t.keyName}, t);");
                    TabM_Cs4.AppendLine("        }");

                    TabM_Cs5.AppendLine("    public static " + csName + " Get" + csName + "(" + t.keyType + " key)");
                    TabM_Cs5.AppendLine("    {");
                    TabM_Cs5.AppendLine("        if (!_map" + csName + ".TryGetValue(key, out var ret))");
                    TabM_Cs5.AppendLine("            Loger.Error(\"" + csName + "表没有key: \" + key);");
                    TabM_Cs5.AppendLine("        return ret;");
                    TabM_Cs5.AppendLine("    }");

                    StringBuilder csContent = new StringBuilder(10000);
                    csContent.AppendLine("");
                    csContent.AppendLine("public class " + csName);
                    csContent.AppendLine("{");
                    ExcelPackage pkg = new ExcelPackage(fi);
                    for (int j = 0; j < mainIdx.Count; j++)
                    {
                        int idx = mainIdx[j];
                        string sType = pkg.Workbook.Worksheets[1].Cells[2, idx].Text.ToLower();
                        if (sType == "v2i") sType = "Vector2Int";
                        else if (sType == "v2i[]") sType = "Vector2Int[]";
                        string sName = pkg.Workbook.Worksheets[1].Cells[3, idx].Text;

                        csContent.AppendLine("    /// <summary>");
                        csContent.AppendLine("    /// " + pkg.Workbook.Worksheets[1].Cells[1, idx].Text);
                        csContent.AppendLine("    /// </summary>");
                        csContent.AppendLine("    public " + sType + " " + sName + " { get; }");
                    }
                    csContent.AppendLine();
                    csContent.AppendLine("    public " + csName + "(DBuffer buffer)");
                    csContent.AppendLine("    {");
                    for (int j = 0; j < mainIdx.Count; j++)
                    {
                        int idx = mainIdx[j];
                        string sType = pkg.Workbook.Worksheets[1].Cells[2, idx].Text.ToLower();
                        string sName = pkg.Workbook.Worksheets[1].Cells[3, idx].Text;

                        if (sType == "int")
                            csContent.AppendLine($"        this.{sName} = buffer.ReadInt();");
                        else if (sType == "int[]")
                        {
                            csContent.AppendLine($"        int len{j} = buffer.ReadInt();");
                            csContent.AppendLine($"        this.{sName} = new int[len{j}];");
                            csContent.AppendLine($"        for (int i = 0; i < len{j}; i++)");
                            csContent.AppendLine($"            this.{sName}[i] = buffer.ReadInt();");
                        }
                        else if (sType == "string")
                            csContent.AppendLine($"        this.{sName} = buffer.ReadString();");
                        else if (sType == "string[]")
                        {
                            csContent.AppendLine($"        int len{j} = buffer.ReadInt();");
                            csContent.AppendLine($"        this.{sName} = new string[len{j}];");
                            csContent.AppendLine($"        for (int i = 0; i < len{j}; i++)");
                            csContent.AppendLine($"            this.{sName}[i] = buffer.ReadString();");
                        }
                        else if (sType == "v2i")
                            csContent.AppendLine($"        this.{sName} = new Vector2Int(buffer.ReadInt(), buffer.ReadInt());");
                        else if (sType == "v2i[]")
                        {
                            csContent.AppendLine($"        int len{j} = buffer.ReadInt();");
                            csContent.AppendLine($"        this.{sName} = new Vector2Int[len{j}];");
                            csContent.AppendLine($"        for (int i = 0; i < len{j}; i++)");
                            csContent.AppendLine($"            this.{sName}[i] = new Vector2Int(buffer.ReadInt(), buffer.ReadInt());");
                        }
                        else if (sType == "float")
                            csContent.AppendLine($"        this.{sName} = buffer.ReadFloat();");
                        else if (sType == "float[]")
                        {
                            csContent.AppendLine($"        int len{j} = buffer.ReadInt();");
                            csContent.AppendLine($"        this.{sName} = new float[len{j}];");
                            csContent.AppendLine($"        for (int i = 0; i < len{j}; i++)");
                            csContent.AppendLine($"            this.{sName}[i] = buffer.ReadFloat();");
                        }
                        else if (sType == "bool")
                            csContent.AppendLine($"        this.{sName} = buffer.ReadBool();");
                        else if (sType == "bool[]")
                        {
                            csContent.AppendLine($"        int len{j} = buffer.ReadInt();");
                            csContent.AppendLine($"        this.{sName} = new bool[len{j}];");
                            csContent.AppendLine($"        for (int i = 0; i < len{j}; i++)");
                            csContent.AppendLine($"            this.{sName}[i] = buffer.ReadBool();");
                        }
                        else
                        {
                            Console.WriteLine("未识别类型 " + sType);
                            Console.ReadLine();
                            return;
                        }
                    }
                    csContent.AppendLine("    }");

                    csContent.AppendLine("}");

                    mainCS.className.Add(csName);
                    mainCS.classContent.Add(csContent.ToString());

                    int dataCnt = 0;
                    int dIdx = 4;
                    while (!string.IsNullOrEmpty(pkg.Workbook.Worksheets[1].Cells[dIdx++, 1].Text))
                        dataCnt++;

                    buffer.Write(dataCnt);
                    int lineIdx = 4;
                    for (int m = 0; m < dataCnt; m++)
                    {
                        for (int j = 0; j < mainIdx.Count; j++)
                        {
                            int idx = mainIdx[j];
                            string sType = pkg.Workbook.Worksheets[1].Cells[2, idx].Text.ToLower();
                            string text = pkg.Workbook.Worksheets[1].Cells[lineIdx, idx].Text;

                            WriteValue(buffer, sType, text, fi, lineIdx, idx);
                        }
                        lineIdx++;
                    }

                }
                mainCS.buff = buffer.ToBytes();
                TabM_Cs4.AppendLine("    }");

                {
                    TabM_Cs.AppendLine(TabM_Cs2.ToString());
                    TabM_Cs.AppendLine(TabM_Cs3.ToString());
                    TabM_Cs.AppendLine(TabM_Cs4.ToString());
                    TabM_Cs.AppendLine(TabM_Cs5.ToString());
                    TabM_Cs.AppendLine("}");
                }

                mainCS.TabCS = TabM_Cs.ToString();

                File.WriteAllText(Environment.CurrentDirectory + "/../../../../Client/Assets/Code/Main/Tab/TabM.cs", mainCS.TabCS.ToString());
                for (int i = 0; i < mainCS.className.Count; i++)
                    File.AppendAllText(Environment.CurrentDirectory + "/../../../../Client/Assets/Code/Main/Tab/TabM.cs", mainCS.classContent[i]);
                File.WriteAllBytes(Environment.CurrentDirectory + "/../../../../Client/Assets/Res/Config/TabM.bytes", mainCS.buff);

            }

            {
                string hot = Environment.CurrentDirectory + "/../../../../../Excel/hot";
                List<string> hots = getFiles(hot);
                StringBuilder TabM_Cs = new StringBuilder(10000);
                StringBuilder TabM_Cs2 = new StringBuilder(10000);
                StringBuilder TabM_Cs3 = new StringBuilder(10000);
                StringBuilder TabM_Cs4 = new StringBuilder(10000);
                StringBuilder TabM_Cs5 = new StringBuilder(10000);

                temp2 mainCS = new temp2();
                DBuffer buffer = new DBuffer();

                {
                    TabM_Cs.AppendLine("using System.Collections.Generic;");
                    TabM_Cs.AppendLine("using UnityEngine;");
                    TabM_Cs.AppendLine("");
                    TabM_Cs.AppendLine("public static class TabL");
                    TabM_Cs.AppendLine("{");

                    TabM_Cs4.AppendLine("    public static void Init(byte[] bytes)");
                    TabM_Cs4.AppendLine("    {");
                    TabM_Cs4.AppendLine("        DBuffer buffer = new DBuffer(bytes);");
                    TabM_Cs4.AppendLine("");
                }

                for (int i = 0; i < hots.Count; i++)
                {
                    List<int> mainIdx = new List<int>();
                    GetHead(mainIdx, hots[i], out temp1 t);
                    FileInfo fi = new FileInfo(hots[i]);
                    Console.WriteLine("开始解析->" + fi.Name);
                    string csName = fi.Name.Split('.')[0];
                    TabM_Cs2.AppendLine("    public static " + csName + "[] " + csName + "Array { get; private set; }");
                    TabM_Cs3.AppendLine("    static Dictionary<" + t.keyType + ", " + csName + "> _map" + csName + ";");
                    TabM_Cs4.AppendLine($"        int len{i} = buffer.ReadInt();");
                    TabM_Cs4.AppendLine("        " + csName + "Array = new " + csName + $"[len{i}];");
                    TabM_Cs4.AppendLine($"        _map{csName} = new Dictionary<{t.keyType}, {csName}>(len{i});");
                    TabM_Cs4.AppendLine($"        for (int i = 0; i < len{i}; i++)");
                    TabM_Cs4.AppendLine("        {");
                    TabM_Cs4.AppendLine($"            var t = new " + csName + "(buffer);");
                    TabM_Cs4.AppendLine($"            " + csName + "Array[i] = t;");
                    TabM_Cs4.AppendLine($"            _map" + csName + $".Add(t.{t.keyName}, t);");
                    TabM_Cs4.AppendLine("        }");

                    TabM_Cs5.AppendLine("    public static " + csName + " Get" + csName + "(" + t.keyType + " key)");
                    TabM_Cs5.AppendLine("    {");
                    TabM_Cs5.AppendLine("        if (!_map" + csName + ".TryGetValue(key, out var ret))");
                    TabM_Cs5.AppendLine("            Loger.Error(\"" + csName + "表没有key: \" + key);");
                    TabM_Cs5.AppendLine("        return ret;");
                    TabM_Cs5.AppendLine("    }");

                    StringBuilder csContent = new StringBuilder(10000);
                    csContent.AppendLine("");
                    csContent.AppendLine("public class " + csName);
                    csContent.AppendLine("{");
                    ExcelPackage pkg = new ExcelPackage(fi);
                    for (int j = 0; j < mainIdx.Count; j++)
                    {
                        int idx = mainIdx[j];
                        string sType = pkg.Workbook.Worksheets[1].Cells[2, idx].Text.ToLower();
                        if (sType == "v2i") sType = "Vector2Int";
                        else if (sType == "v2i[]") sType = "Vector2Int[]";
                        string sName = pkg.Workbook.Worksheets[1].Cells[3, idx].Text;

                        csContent.AppendLine("    /// <summary>");
                        csContent.AppendLine("    /// " + pkg.Workbook.Worksheets[1].Cells[1, idx].Text);
                        csContent.AppendLine("    /// </summary>");
                        csContent.AppendLine("    public " + sType + " " + sName + " { get; }");
                    }
                    csContent.AppendLine();
                    csContent.AppendLine("    public " + csName + "(DBuffer buffer)");
                    csContent.AppendLine("    {");
                    for (int j = 0; j < mainIdx.Count; j++)
                    {
                        int idx = mainIdx[j];
                        string sType = pkg.Workbook.Worksheets[1].Cells[2, idx].Text.ToLower();
                        string sName = pkg.Workbook.Worksheets[1].Cells[3, idx].Text;

                        if (sType == "int")
                            csContent.AppendLine($"        this.{sName} = buffer.ReadInt();");
                        else if (sType == "int[]")
                        {
                            csContent.AppendLine($"        int len{j} = buffer.ReadInt();");
                            csContent.AppendLine($"        this.{sName} = new int[len{j}];");
                            csContent.AppendLine($"        for (int i = 0; i < len{j}; i++)");
                            csContent.AppendLine($"            this.{sName}[i] = buffer.ReadInt();");
                        }
                        else if (sType == "string")
                            csContent.AppendLine($"        this.{sName} = buffer.ReadString();");
                        else if (sType == "string[]")
                        {
                            csContent.AppendLine($"        int len{j} = buffer.ReadInt();");
                            csContent.AppendLine($"        this.{sName} = new string[len{j}];");
                            csContent.AppendLine($"        for (int i = 0; i < len{j}; i++)");
                            csContent.AppendLine($"            this.{sName}[i] = buffer.ReadString();");
                        }
                        else if (sType == "v2i")
                            csContent.AppendLine($"        this.{sName} = new Vector2Int(buffer.ReadInt(), buffer.ReadInt());");
                        else if (sType == "v2i[]")
                        {
                            csContent.AppendLine($"        int len{j} = buffer.ReadInt();");
                            csContent.AppendLine($"        this.{sName} = new Vector2Int[len{j}];");
                            csContent.AppendLine($"        for (int i = 0; i < len{j}; i++)");
                            csContent.AppendLine($"            this.{sName}[i] = new Vector2Int(buffer.ReadInt(), buffer.ReadInt());");
                        }
                        else if (sType == "float")
                            csContent.AppendLine($"        this.{sName} = buffer.ReadFloat();");
                        else if (sType == "float[]")
                        {
                            csContent.AppendLine($"        int len{j} = buffer.ReadInt();");
                            csContent.AppendLine($"        this.{sName} = new float[len{j}];");
                            csContent.AppendLine($"        for (int i = 0; i < len{j}; i++)");
                            csContent.AppendLine($"            this.{sName}[i] = buffer.ReadFloat();");
                        }
                        else if (sType == "bool")
                            csContent.AppendLine($"        this.{sName} = buffer.ReadBool();");
                        else if (sType == "bool[]")
                        {
                            csContent.AppendLine($"        int len{j} = buffer.ReadInt();");
                            csContent.AppendLine($"        this.{sName} = new bool[len{j}];");
                            csContent.AppendLine($"        for (int i = 0; i < len{j}; i++)");
                            csContent.AppendLine($"            this.{sName}[i] = buffer.ReadBool();");
                        }
                        else
                        {
                            Console.WriteLine("未识别类型 " + sType);
                            Console.ReadLine();
                            return;
                        }
                    }
                    csContent.AppendLine("    }");

                    csContent.AppendLine("}");

                    mainCS.className.Add(csName);
                    mainCS.classContent.Add(csContent.ToString());

                    int dataCnt = 0;
                    int dIdx = 4;
                    while (!string.IsNullOrEmpty(pkg.Workbook.Worksheets[1].Cells[dIdx++, 1].Text))
                        dataCnt++;

                    buffer.Write(dataCnt);
                    int lineIdx = 4;
                    for (int m = 0; m < dataCnt; m++)
                    {
                        for (int j = 0; j < mainIdx.Count; j++)
                        {
                            int idx = mainIdx[j];
                            string sType = pkg.Workbook.Worksheets[1].Cells[2, idx].Text.ToLower();
                            string text = pkg.Workbook.Worksheets[1].Cells[lineIdx, idx].Text;

                            WriteValue(buffer, sType, text, fi, lineIdx, idx);
                        }
                        lineIdx++;
                    }

                }
                mainCS.buff = buffer.ToBytes();
                TabM_Cs4.AppendLine("    }");

                for (int i = 0; i < hots.Count; i++)
                {
                    List<int> hotIdx = new List<int>();
                    GetHead(hotIdx, hots[0], out temp1 t);
                }

                {
                    TabM_Cs.AppendLine(TabM_Cs2.ToString());
                    TabM_Cs.AppendLine(TabM_Cs3.ToString());
                    TabM_Cs.AppendLine(TabM_Cs4.ToString());
                    TabM_Cs.AppendLine(TabM_Cs5.ToString());
                    TabM_Cs.AppendLine("}");
                }

                mainCS.TabCS = TabM_Cs.ToString();

                File.WriteAllText(Environment.CurrentDirectory + "/../../../../Client/Assets/Code/HotFix/Tab/TabL.cs", mainCS.TabCS.ToString()); 
                for (int i = 0; i < mainCS.className.Count; i++)
                    File.AppendAllText(Environment.CurrentDirectory + "/../../../../Client/Assets/Code/HotFix/Tab/TabL.cs", mainCS.classContent[i]);
                File.WriteAllBytes(Environment.CurrentDirectory + "/../../../../Client/Assets/Res/Config/TabL.bytes", mainCS.buff);

            }

            //Language表 
            {
                string main = Environment.CurrentDirectory + "/../../../../../Excel/Language";
                List<string> mains = getFiles(main);
                List<int> mainIdx = new List<int>();
                GetHead(mainIdx, mains[0], out temp1 t);
                FileInfo fi = new FileInfo(mains[0]);
                Console.WriteLine("开始解析->" + fi.Name);
                string csName = "Language";

                StringBuilder csContent = new StringBuilder(10000);
                csContent.AppendLine("using UnityEngine;");
                csContent.AppendLine("");
                csContent.AppendLine("public class " + csName);
                csContent.AppendLine("{");
                ExcelPackage pkg = new ExcelPackage(fi);
                for (int j = 0; j < mainIdx.Count; j++)
                {
                    int idx = mainIdx[j];
                    string sType = pkg.Workbook.Worksheets[1].Cells[2, idx].Text.ToLower();
                    if (sType == "v2i") sType = "Vector2Int";
                    else if (sType == "v2i[]") sType = "Vector2Int[]";
                    string sName = pkg.Workbook.Worksheets[1].Cells[3, idx].Text;

                    csContent.AppendLine("    /// <summary>");
                    csContent.AppendLine("    /// " + pkg.Workbook.Worksheets[1].Cells[1, idx].Text);
                    csContent.AppendLine("    /// </summary>");
                    csContent.AppendLine("    public " + sType + " " + sName + " { get; }");
                }
                csContent.AppendLine();
                csContent.AppendLine("    public " + csName + "(DBuffer buffer)");
                csContent.AppendLine("    {");
                for (int j = 0; j < mainIdx.Count; j++)
                {
                    int idx = mainIdx[j];
                    string sType = pkg.Workbook.Worksheets[1].Cells[2, idx].Text.ToLower();
                    string sName = pkg.Workbook.Worksheets[1].Cells[3, idx].Text;

                    if (sType == "int")
                        csContent.AppendLine($"        this.{sName} = buffer.ReadInt();");
                    else if (sType == "int[]")
                    {
                        csContent.AppendLine($"        int len{j} = buffer.ReadInt();");
                        csContent.AppendLine($"        this.{sName} = new int[len{j}];");
                        csContent.AppendLine($"        for (int i = 0; i < len{j}; i++)");
                        csContent.AppendLine($"            this.{sName}[i] = buffer.ReadInt();");
                    }
                    else if (sType == "string")
                        csContent.AppendLine($"        this.{sName} = buffer.ReadString();");
                    else if (sType == "string[]")
                    {
                        csContent.AppendLine($"        int len{j} = buffer.ReadInt();");
                        csContent.AppendLine($"        this.{sName} = new string[len{j}];");
                        csContent.AppendLine($"        for (int i = 0; i < len{j}; i++)");
                        csContent.AppendLine($"            this.{sName}[i] = buffer.ReadString();");
                    }
                    else if (sType == "v2i")
                        csContent.AppendLine($"        this.{sName} = new Vector2Int(buffer.ReadInt(), buffer.ReadInt());");
                    else if (sType == "v2i[]")
                    {
                        csContent.AppendLine($"        int len{j} = buffer.ReadInt();");
                        csContent.AppendLine($"        this.{sName} = new Vector2Int[len{j}];");
                        csContent.AppendLine($"        for (int i = 0; i < len{j}; i++)");
                        csContent.AppendLine($"            this.{sName}[i] = new Vector2Int(buffer.ReadInt(), buffer.ReadInt());");
                    }
                    else if (sType == "float")
                        csContent.AppendLine($"        this.{sName} = buffer.ReadFloat();");
                    else if (sType == "float[]")
                    {
                        csContent.AppendLine($"        int len{j} = buffer.ReadInt();");
                        csContent.AppendLine($"        this.{sName} = new float[len{j}];");
                        csContent.AppendLine($"        for (int i = 0; i < len{j}; i++)");
                        csContent.AppendLine($"            this.{sName}[i] = buffer.ReadFloat();");
                    }
                    else if (sType == "bool")
                        csContent.AppendLine($"        this.{sName} = buffer.ReadBool();");
                    else if (sType == "bool[]")
                    {
                        csContent.AppendLine($"        int len{j} = buffer.ReadInt();");
                        csContent.AppendLine($"        this.{sName} = new bool[len{j}];");
                        csContent.AppendLine($"        for (int i = 0; i < len{j}; i++)");
                        csContent.AppendLine($"            this.{sName}[i] = buffer.ReadBool();");
                    }
                    else
                    {
                        Console.WriteLine("未识别类型 " + sType);
                        Console.ReadLine();
                        return;
                    }
                }
                csContent.AppendLine("    }");

                csContent.AppendLine("}");

                DBuffer buffer = new DBuffer();
                int dataCnt = 0;
                int dIdx = 4;
                while (!string.IsNullOrEmpty(pkg.Workbook.Worksheets[1].Cells[dIdx++, 1].Text))
                    dataCnt++;

                buffer.Write(dataCnt);
                int lineIdx = 4;
                for (int m = 0; m < dataCnt; m++)
                {
                    for (int j = 0; j < mainIdx.Count; j++)
                    {
                        int idx = mainIdx[j];
                        string sType = pkg.Workbook.Worksheets[1].Cells[2, idx].Text.ToLower();
                        string text = pkg.Workbook.Worksheets[1].Cells[lineIdx, idx].Text;

                        WriteValue(buffer, sType, text, fi, lineIdx, idx);
                    }
                    lineIdx++;
                }

                File.WriteAllText(Environment.CurrentDirectory + $"/../../../../Client/Assets/Code/Main/Tab/Language.cs", csContent.ToString());
                File.WriteAllBytes(Environment.CurrentDirectory + "/../../../../Client/Assets/Res/Config/Language.bytes", buffer.ToBytes());
            }

            Console.WriteLine("生成成功");
            Console.ReadLine();
        }

        static void WriteValue(DBuffer buffer, string sType, string text,FileInfo fi,int lineIdx,int idx)
        {
            if (sType == "int")
            {
                if (!int.TryParse(text, out var v))
                {
                    Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                    Console.ReadLine();
                    return;
                }
                buffer.Write(v);
            }
            else if (sType == "int[]")
            {
                string[] arr = text.Split(new char[] { '[', ']', ',' });

                List<int> vs = new List<int>();
                for (int k = 1; k < arr.Length - 1; k++)
                {
                    if (!int.TryParse(arr[k], out var v))
                    {
                        Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                        Console.ReadLine();
                        return;
                    }
                    vs.Add(v);
                }
                buffer.Write(vs.Count);
                for (int k = 0; k < vs.Count; k++)
                    buffer.Write(vs[k]);
            }
            else if (sType == "string")
            {
                buffer.Write(text);
            }
            else if (sType == "string[]")
            {
                string[] arr = text.Split(new char[] { '[', ']', ',' });

                List<string> vs = new List<string>();
                for (int k = 1; k < arr.Length - 1; k++)
                {
                    vs.Add(arr[k]);
                }
                buffer.Write(vs.Count);
                for (int k = 0; k < vs.Count; k++)
                    buffer.Write(vs[k]);
            }
            else if (sType == "v2i")
            {
                string[] s = text.Split(new char[] { '(', ')', ',' });
                if (s.Length != 4 || !int.TryParse(s[1], out var v1) || !int.TryParse(s[2], out var v2))
                {
                    Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                    Console.ReadLine();
                    return;
                }
                buffer.Write(v1);
                buffer.Write(v2);
            }
            else if (sType == "v2i[]")
            {
                string[] arr = text.Split(new char[] { '[', ']', '(', ')', ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (arr.Length % 2 != 0)
                {
                    Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                    Console.ReadLine();
                    return;
                }

                List<int> vs = new List<int>();
                for (int k = 0; k < arr.Length; k += 2)
                {
                    if (!int.TryParse(arr[k], out var v1) || !int.TryParse(arr[k + 1], out var v2))
                    {
                        Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                        Console.ReadLine();
                        return;
                    }
                    vs.Add(v1);
                    vs.Add(v2);
                }
                buffer.Write(vs.Count / 2);
                for (int k = 0; k < vs.Count; k += 2)
                {
                    buffer.Write(vs[k]);
                    buffer.Write(vs[k + 1]);
                }
            }
            else if (sType == "float")
            {
                if (!float.TryParse(text, out var v))
                {
                    Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                    Console.ReadLine();
                    return;
                }
                buffer.Write(v);
            }
            else if (sType == "float[]")
            {
                string[] arr = text.Split(new char[] { '[', ']', ',' });

                List<float> vs = new List<float>();
                for (int k = 1; k < arr.Length - 1; k++)
                {
                    if (!float.TryParse(arr[k], out var v))
                    {
                        Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                        Console.ReadLine();
                        return;
                    }
                    vs.Add(v);
                }
                buffer.Write(vs.Count);
                for (int k = 0; k < vs.Count; k++)
                    buffer.Write(vs[k]);
            }
            else if (sType == "bool")
            {
                if (!int.TryParse(text, out var v) || (v != 0 && v != 1))
                {
                    Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                    Console.ReadLine();
                    return;
                }
                buffer.Write(v == 1);
            }
            else if (sType == "bool[]")
            {
                string[] arr = text.Split(new char[] { '[', ']', ',' });

                List<int> vs = new List<int>();
                for (int k = 1; k < arr.Length - 1; k++)
                {
                    if (!int.TryParse(arr[k], out var v) || (v != 0 && v != 1))
                    {
                        Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                        Console.ReadLine();
                        return;
                    }
                    vs.Add(v);
                }
                buffer.Write(vs.Count);
                for (int k = 0; k < vs.Count; k++)
                    buffer.Write(vs[k] == 1);
            }
        }

        static List<string> getFiles(string path)
        {
            List<string> files = new List<string>();
            string[] s = Directory.GetFiles(path);
            for (int i = 0; i < s.Length; i++)
            {
                FileInfo fi = new FileInfo(s[i]);
                if (fi.Name.StartsWith("~")) continue;
                if (fi.Extension != ".xls" && fi.Extension != ".xlsx") continue;
                files.Add(s[i]);
            }
            return files;
        }

        //解析头 获得类型和字段名
        static void GetHead(List<int> coIdx, string path,out temp1 t)
        {
            ExcelPackage pkg = new ExcelPackage(new FileInfo(path));
            t = new temp1();
            t.keyType = pkg.Workbook.Worksheets[1].Cells[2, 1].Text;
            t.keyName = pkg.Workbook.Worksheets[1].Cells[3, 1].Text;
            for (int i = 1; i < 1000; i++)
            {
                if (string.IsNullOrEmpty(pkg.Workbook.Worksheets[1].Cells[2, i].Text))
                    continue;
                if (string.IsNullOrEmpty(pkg.Workbook.Worksheets[1].Cells[3, i].Text))
                    continue;
                coIdx.Add(i);
            }
        }

        class temp1
        {
            public string keyType;
            public string keyName;
        }
        class temp2
        {
            public string TabCS;
            public List<string> className = new List<string>();
            public List<string> classContent = new List<string>();
            public byte[] buff;
        }
    }
}
