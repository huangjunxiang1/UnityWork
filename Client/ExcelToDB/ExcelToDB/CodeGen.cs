using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using OfficeOpenXml;

class CodeGen
{
    public string name;

    public string excelPath;
    public string codePath;
    public string dataPath;

    public bool mappingIsClass = false;

    public void Gen()
    {
        List<string> mains = Common.getFiles(excelPath);
        StringBuilder TabM_Cs = new StringBuilder(10000);
        StringBuilder TabM_Cs2 = new StringBuilder(10000);
        StringBuilder TabM_Cs4 = new StringBuilder(10000);
        StringBuilder TabM_Cs5 = new StringBuilder(10000);
        StringBuilder csDefine = new StringBuilder(10000);

        temp2 mainCS = new temp2();
        DBuffer buffer = new DBuffer(new MemoryStream(1024));
        DBuffer OneData = new DBuffer(new MemoryStream(1024));
        DBuffer arrayTemp = new DBuffer(new MemoryStream(1024));
        buffer.Compress = false;
        buffer.Write(Program.mark);
        buffer.Compress = Program.compress;
        buffer.Write(buffer.Compress);//是否压缩
        OneData.Compress = Program.compress;
        arrayTemp.Compress = Program.compress;

        {
            TabM_Cs.AppendLine("using System.Collections.Generic;");
            TabM_Cs.AppendLine("using UnityEngine;");
            TabM_Cs.AppendLine("using System.Linq;");
            TabM_Cs.AppendLine("");
            TabM_Cs.AppendLine($"public static class {name}");
            TabM_Cs.AppendLine("{");
            TabM_Cs.AppendLine("    static DBuffer dbbuff;");
            TabM_Cs.AppendLine("    static bool debug;");
            TabM_Cs.AppendLine("");

            TabM_Cs4.AppendLine("    public static void Init(DBuffer buffer, bool isDebug)");
            TabM_Cs4.AppendLine("    {");
            TabM_Cs4.AppendLine("        dbbuff = buffer;");
            TabM_Cs4.AppendLine("        debug = isDebug;");
            TabM_Cs4.AppendLine("");

            csDefine.AppendLine("using System.Collections.Generic;");
            csDefine.AppendLine("using UnityEngine;");
            csDefine.AppendLine("using System.Linq;");
        }

        for (int i = 0; i < mains.Count; i++)
        {
            FileInfo fi = new FileInfo(mains[i]);
            ExcelPackage pkg = new ExcelPackage(fi);
            List<int> mainIdx = new List<int>();
            Common.GetHead(mainIdx, pkg, out temp1 t);
            Console.WriteLine("开始解析->" + fi.Name);
            string name = fi.Name.Split('.')[0];
            string csName = "Tab" + name;

            TabM_Cs2.AppendLine($"    static Dictionary<{t.keyType}, Mapping> _map{name}Idx;");
            TabM_Cs2.AppendLine($"    static {csName}[] _{name}Array;");
            TabM_Cs2.AppendLine($"    static Dictionary<{t.keyType}, {csName}> _map{name};");
            TabM_Cs2.AppendLine($"    public static {csName}[] {name}Array");
            TabM_Cs2.AppendLine("    {");
            TabM_Cs2.AppendLine($"        get");
            TabM_Cs2.AppendLine("        {");
            TabM_Cs2.AppendLine($"            if (_{name}Array == null)");
            TabM_Cs2.AppendLine("            {");
            TabM_Cs2.AppendLine("                bool isDebug = debug;");
            TabM_Cs2.AppendLine($"                {t.keyType}[] keys = _map{name}Idx.Keys.ToArray();");
            TabM_Cs2.AppendLine($"                int len = keys.Length;");
            TabM_Cs2.AppendLine($"                _{name}Array = new {csName}[_map{name}Idx.Count];");
            TabM_Cs2.AppendLine($"                for (int i = 0; i < len; i++)");
            TabM_Cs2.AppendLine("                {");
            TabM_Cs2.AppendLine($"                    {t.keyType} k = keys[i];");
            TabM_Cs2.AppendLine($"                    Mapping v = _map{name}Idx[k];");
            TabM_Cs2.AppendLine($"                    if (_map{name}.TryGetValue(k, out {csName} value))");
            TabM_Cs2.AppendLine($"                        _{name}Array[v.index] = value;");
            TabM_Cs2.AppendLine("                    else");
            TabM_Cs2.AppendLine("                    {");
            TabM_Cs2.AppendLine("                        dbbuff.Seek(v.point);");
            TabM_Cs2.AppendLine($"                        {csName} tmp = new {csName}(dbbuff, isDebug);");
            TabM_Cs2.AppendLine($"                        _map{name}[k] = tmp;");
            TabM_Cs2.AppendLine($"                        _{name}Array[v.index] = tmp;");
            TabM_Cs2.AppendLine("                    }");
            TabM_Cs2.AppendLine("                }");
            TabM_Cs2.AppendLine($"                _map{name}Idx = null;");
            TabM_Cs2.AppendLine("            }");
            TabM_Cs2.AppendLine($"            return _{name}Array;");
            TabM_Cs2.AppendLine("        }");
            TabM_Cs2.AppendLine("    }");
            TabM_Cs2.AppendLine();

            TabM_Cs4.AppendLine($"        int len{i} = buffer.Readint();");
            TabM_Cs4.AppendLine($"        _map{name}Idx = new Dictionary<int, Mapping>(len{i});");
            TabM_Cs4.AppendLine($"        _map{name} = new Dictionary<int, {csName}>(len{i});");
            TabM_Cs4.AppendLine($"        _{name}Array = null;");
            TabM_Cs4.AppendLine($"        for (int i = 0; i < len{i}; i++)");
            TabM_Cs4.AppendLine("        {");
            TabM_Cs4.AppendLine($"            int offset = buffer.Readint();");
            TabM_Cs4.AppendLine($"            Mapping map = new Mapping();");
            TabM_Cs4.AppendLine($"            map.point = buffer.Position;");
            TabM_Cs4.AppendLine($"            map.index = i;");
            TabM_Cs4.AppendLine($"            _map{name}Idx.Add(buffer.Readint(), map);");
            TabM_Cs4.AppendLine($"            buffer.Seek(map.point + offset);");
            TabM_Cs4.AppendLine("        }");
            TabM_Cs4.AppendLine($"        if (isDebug) _ = {name}Array;");
            TabM_Cs4.AppendLine();

            TabM_Cs5.AppendLine($"    public static {csName} Get{name}({t.keyType} key)");
            TabM_Cs5.AppendLine("    {");
            TabM_Cs5.AppendLine($"        if (_map{name}.TryGetValue(key, out var value))");
            TabM_Cs5.AppendLine($"            return value;");
            TabM_Cs5.AppendLine($"        if (_map{name}Idx != null && _map{name}Idx.TryGetValue(key, out Mapping map))");
            TabM_Cs5.AppendLine("        {");
            TabM_Cs5.AppendLine($"            dbbuff.Seek(map.point);");
            TabM_Cs5.AppendLine($"            {csName} tmp = new {csName}(dbbuff);");
            TabM_Cs5.AppendLine($"            _map{name}[key] = tmp;");
            TabM_Cs5.AppendLine($"            return tmp;");
            TabM_Cs5.AppendLine("        }");
            TabM_Cs5.AppendLine($"        Loger.Error(\"{csName}表没有key: \" + key);");
            TabM_Cs5.AppendLine($"        return null;");
            TabM_Cs5.AppendLine("    }");
            TabM_Cs5.AppendLine();

            StringBuilder csContent = new StringBuilder(10000);
            csContent.AppendLine("");
            csContent.AppendLine("public partial class " + csName);
            csContent.AppendLine("{");
            csContent.AppendLine("    DBuffer dbuff;");

            csDefine.AppendLine("");
            csDefine.AppendLine("public partial class " + csName);
            csDefine.AppendLine("{");

            for (int j = 0; j < mainIdx.Count; j++)
            {
                int idx = mainIdx[j];
                string sType = pkg.Workbook.Worksheets[0].Cells[2, idx].Text.ToLower();
                sType = Common.getType(sType);
                string sName = pkg.Workbook.Worksheets[0].Cells[3, idx].Text;
                Common.appendDefineCode(pkg.Workbook.Worksheets[0].Cells[1, idx].Text, sType, sName, csContent);
                Common.appendDefineCode2(pkg.Workbook.Worksheets[0].Cells[1, idx].Text, sType, sName, csDefine);
            }
            csContent.AppendLine();
            csContent.AppendLine($"    public {csName}(DBuffer buffer, bool isDebug = false)");
            csContent.AppendLine("    {");
            csContent.AppendLine("        dbuff = buffer;");
            for (int j = 0; j < mainIdx.Count; j++)
            {
                int idx = mainIdx[j];
                string sType = pkg.Workbook.Worksheets[0].Cells[2, idx].Text.ToLower();
                string sName = pkg.Workbook.Worksheets[0].Cells[3, idx].Text;
                Common.appendReadCode(sType, j, sName, csContent);
            }
            csContent.AppendLine($"        if (isDebug)");
            csContent.AppendLine($"        {{");
            for (int j = 0; j < mainIdx.Count; j++)
            {
                int idx = mainIdx[j];
                string sType = pkg.Workbook.Worksheets[0].Cells[2, idx].Text.ToLower();
                string sName = pkg.Workbook.Worksheets[0].Cells[3, idx].Text;
                if (sType == "string" || sType.Contains("[]"))
                    csContent.AppendLine($"            _ = this.{sName};");
            }
            csContent.AppendLine($"        }}");
            csContent.AppendLine("    }");
            csContent.AppendLine("}");

            csDefine.AppendLine("}");

            mainCS.className.Add(csName);
            mainCS.classContent.Add(csContent.ToString());

            List<int> lines = new List<int>();
            var array = (object[,])pkg.Workbook.Worksheets[0].Cells.Value;
            int len = array.GetLength(0);
            for (int j = 3; j < len; j++)
            {
                if (!string.IsNullOrEmpty(pkg.Workbook.Worksheets[0].Cells[j + 1, 1].Text))
                    lines.Add(j + 1);
            }

            buffer.Write(lines.Count);
            for (int m = 0; m < lines.Count; m++)
            {
                OneData.Seek(0);
                for (int j = 0; j < mainIdx.Count; j++)
                {
                    int idx = mainIdx[j];
                    string sType = pkg.Workbook.Worksheets[0].Cells[2, idx].Text.ToLower();
                    string text = pkg.Workbook.Worksheets[0].Cells[lines[m], idx].Text;

                    Common.WriteValue(OneData, arrayTemp, sType, text, fi, lines[m], idx);
                }
                buffer.Write(OneData);
            }

        }
        mainCS.buff = buffer.ToBytes();
        TabM_Cs4.AppendLine("    }");

        {
            TabM_Cs.AppendLine(TabM_Cs2.ToString());
            TabM_Cs.AppendLine(TabM_Cs4.ToString());
            TabM_Cs.AppendLine(TabM_Cs5.ToString());

            if (mappingIsClass)
                TabM_Cs.AppendLine("    class Mapping");
            else
                TabM_Cs.AppendLine("    struct Mapping");
            TabM_Cs.AppendLine("    {");
            TabM_Cs.AppendLine("        public int point;");
            TabM_Cs.AppendLine("        public int index;");
            TabM_Cs.AppendLine("    }");

            TabM_Cs.AppendLine("}");
        }

        mainCS.TabCS = TabM_Cs.ToString();

        File.WriteAllText(codePath + ".cs", mainCS.TabCS.ToString());
        for (int i = 0; i < mainCS.className.Count; i++)
            File.AppendAllText(codePath + ".cs", mainCS.classContent[i]);
        File.WriteAllText(codePath + "Define.cs", csDefine.ToString());
        File.WriteAllBytes(dataPath, mainCS.buff);
    }
}
