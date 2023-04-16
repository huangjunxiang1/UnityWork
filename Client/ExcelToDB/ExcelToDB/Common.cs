using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

static class Common
{

    public static string getType(string sType)
    {
        if (sType == "v2") sType = "Vector2";
        else if (sType == "v2[]") sType = "Vector2[]";

        else if (sType == "v2i") sType = "Vector2Int";
        else if (sType == "v2i[]") sType = "Vector2Int[]";

        else if (sType == "v3") sType = "Vector3";
        else if (sType == "v3[]") sType = "Vector3[]";

        else if (sType == "v3i") sType = "Vector3Int";
        else if (sType == "v3i[]") sType = "Vector3Int[]";
        return sType;
    }

    public static void appendDefineCode(string des, string type, string name, StringBuilder code)
    {
        if (type == "string")
        {
            code.AppendLine("");
            code.AppendLine($"    int _{name}Idx;");
            code.AppendLine($"    {type} _{name}Tmp;");
            code.AppendLine($"    public {type} get{name}()");
            code.AppendLine(@"    {");
            code.AppendLine($"        if (_{name}Tmp == null)");
            code.AppendLine($"        {{");
            code.AppendLine($"            dbuff.Seek(_{name}Idx);");
            code.AppendLine($"            _{name}Tmp = dbuff.Readstring();");
            code.AppendLine($"        }}");
            code.AppendLine($"        return _{name}Tmp;");
            code.AppendLine(@"    }");
        }
        else if (type.Contains("[]"))
        {
            string realType = type.Replace("[]", null);
            code.AppendLine("");
            code.AppendLine($"    int _{name}Idx;");
            code.AppendLine($"    {type} _{name}Tmp;");
            code.AppendLine($"    public {type} get{name}()");
            code.AppendLine(@"    {");
            code.AppendLine($"        if (_{name}Tmp == null)");
            code.AppendLine($"        {{");
            code.AppendLine($"            dbuff.Seek(_{name}Idx);");
            code.AppendLine($"            _{name}Tmp = dbuff.Read{realType}s();");
            code.AppendLine($"        }}");
            code.AppendLine($"        return _{name}Tmp;");
            code.AppendLine(@"    }");
        }
    }
    public static void appendDefineCode2(string des, string type, string name, StringBuilder code)
    {
        if (type == "string" || type.Contains("[]"))
        {
            code.AppendLine("    /// <summary>");
            code.AppendLine("    /// " + des);
            code.AppendLine("    /// </summary>");
            code.AppendLine($"    public {type} {name} => get{name}();");
        }
        else
        {
            code.AppendLine("    /// <summary>");
            code.AppendLine("    /// " + des);
            code.AppendLine("    /// </summary>");
            code.AppendLine("    public " + type + " " + name + " { get; }");
        }
    }

    public static void appendReadCode(string sType, int j, string sName, StringBuilder csContent)
    {
        sType = getType(sType);
        if (sType == "string")
        {
            csContent.AppendLine($"        this._{sName}Idx = buffer.Position;");
            csContent.AppendLine($"        buffer.Seek(buffer.Readint() + buffer.Position);");
        }
        else if (sType.Contains("[]"))
        {
            csContent.AppendLine($"        int next{j} = buffer.Readint() + buffer.Position;");
            csContent.AppendLine($"        this._{sName}Idx = buffer.Position;");
            csContent.AppendLine($"        buffer.Seek(next{j});");
        }
        else
            csContent.AppendLine($"        this.{sName} = buffer.Read{sType}();");
    }

    public static void WriteValue(DBuffer buffer, DBuffer arrayTemp, string sType, string text, FileInfo fi, int lineIdx, int idx)
    {
        ///arrayTemp  如果写入的是数组 arrayTemp不为空时  数据先写入数组的数据总大小 arrayTemp为空则不写入数组的数据总大小
        sType = getType(sType);
        if (sType == "bool")
        {
            if (string.IsNullOrEmpty(text))
            {
                buffer.Write(false);
                return;
            }
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
            string[] arr = text.Split(new char[] { '[', ']', ',' }, StringSplitOptions.RemoveEmptyEntries);

            bool[] temp = new bool[arr.Length];
            if (arrayTemp != null)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    if (!int.TryParse(arr[i], out var v) || (v != 0 && v != 1))
                    {
                        Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                        Console.ReadLine();
                        return;
                    }
                    temp[i] = v == 1;
                }
                arrayTemp.Seek(0);
                arrayTemp.Write(temp);
                buffer.Write(arrayTemp);
            }
            else
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    if (!int.TryParse(arr[i], out var v) || (v != 0 && v != 1))
                    {
                        Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                        Console.ReadLine();
                        return;
                    }
                    temp[i] = v == 1;
                }
                buffer.Write(temp);
            }
        }

        else if (sType == "byte")
        {
            if (string.IsNullOrEmpty(text))
            {
                buffer.Write((byte)0);
                return;
            }
            if (!byte.TryParse(text, out var v))
            {
                Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                Console.ReadLine();
                return;
            }
            buffer.Write(v);
        }
        else if (sType == "byte[]")
        {
            string[] arr = text.Split(new char[] { '[', ']', ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (arrayTemp != null)
            {
                arrayTemp.Seek(0);
                arrayTemp.Write(arr.Length);
                for (int i = 0; i < arr.Length; i++)
                {
                    if (!byte.TryParse(arr[i], out var v))
                    {
                        Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                        Console.ReadLine();
                        return;
                    }
                    arrayTemp.Write(v);
                }
                buffer.Write(arrayTemp);
            }
            else
            {
                buffer.Write(arr.Length);
                for (int i = 0; i < arr.Length; i++)
                {
                    if (!byte.TryParse(arr[i], out var v))
                    {
                        Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                        Console.ReadLine();
                        return;
                    }
                    buffer.Write(v);
                }
            }
        }

        else if (sType == "int")
        {
            if (string.IsNullOrEmpty(text))
            {
                buffer.Write(0);
                return;
            }
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
            string[] arr = text.Split(new char[] { '[', ']', ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (arrayTemp != null)
            {
                arrayTemp.Seek(0);
                arrayTemp.Write(arr.Length);
                for (int i = 0; i < arr.Length; i++)
                {
                    if (!int.TryParse(arr[i], out var v))
                    {
                        Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                        Console.ReadLine();
                        return;
                    }
                    arrayTemp.Write(v);
                }
                buffer.Write(arrayTemp);
            }
            else
            {
                buffer.Write(arr.Length);
                for (int i = 0; i < arr.Length; i++)
                {
                    if (!int.TryParse(arr[i], out var v))
                    {
                        Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                        Console.ReadLine();
                        return;
                    }
                    buffer.Write(v);
                }
            }
        }

        else if (sType == "long")
        {
            if (string.IsNullOrEmpty(text))
            {
                buffer.Write(0);
                return;
            }
            if (!long.TryParse(text, out var v))
            {
                Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                Console.ReadLine();
                return;
            }
            buffer.Write(v);
        }
        else if (sType == "long[]")
        {
            string[] arr = text.Split(new char[] { '[', ']', ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (arrayTemp != null)
            {
                arrayTemp.Seek(0);
                arrayTemp.Write(arr.Length);
                for (int i = 0; i < arr.Length; i++)
                {
                    if (!long.TryParse(arr[i], out var v))
                    {
                        Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                        Console.ReadLine();
                        return;
                    }
                    arrayTemp.Write(v);
                }
                buffer.Write(arrayTemp);
            }
            else
            {
                buffer.Write(arr.Length);
                for (int i = 0; i < arr.Length; i++)
                {
                    if (!long.TryParse(arr[i], out var v))
                    {
                        Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                        Console.ReadLine();
                        return;
                    }
                    buffer.Write(v);
                }
            }
        }

        else if (sType == "string")
        {
            buffer.Write(text);
        }
        else if (sType == "string[]")
        {
            string[] arr = text.Split(new char[] { '[', ']', ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (arrayTemp != null)
            {
                arrayTemp.Seek(0);
                arrayTemp.Write(arr.Length);
                for (int i = 0; i < arr.Length; i++)
                    arrayTemp.Write(arr[i]);
                buffer.Write(arrayTemp);
            }
            else
            {
                buffer.Write(arr.Length);
                for (int i = 0; i < arr.Length; i++)
                    buffer.Write(arr[i]);
            }
        }

        else if (sType == "Vector2")
        {
            if (string.IsNullOrEmpty(text))
            {
                buffer.Write(0);
                buffer.Write(0);
                return;
            }
            string[] s = text.Split(new char[] { '(', ')', ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (s.Length != 2 || !float.TryParse(s[0], out var v1) || !float.TryParse(s[1], out var v2))
            {
                Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                Console.ReadLine();
                return;
            }
            buffer.Write(v1);
            buffer.Write(v2);
        }
        else if (sType == "Vector2[]")
        {
            string[] arr = text.Split(new char[] { '[', ']', '(', ')', ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (arr.Length % 2 != 0)
            {
                Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                Console.ReadLine();
                return;
            }

            if (arrayTemp != null)
            {
                arrayTemp.Seek(0);
                arrayTemp.Write(arr.Length / 2);
                for (int i = 0; i < arr.Length; i += 2)
                {
                    if (!float.TryParse(arr[i], out var v1) || !float.TryParse(arr[i + 1], out var v2))
                    {
                        Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                        Console.ReadLine();
                        return;
                    }
                    arrayTemp.Write(v1);
                    arrayTemp.Write(v2);
                }
                buffer.Write(arrayTemp);
            }
            else
            {
                buffer.Write(arr.Length / 2);
                for (int i = 0; i < arr.Length; i += 2)
                {
                    if (!float.TryParse(arr[i], out var v1) || !float.TryParse(arr[i + 1], out var v2))
                    {
                        Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                        Console.ReadLine();
                        return;
                    }
                    buffer.Write(v1);
                    buffer.Write(v2);
                }
            }
        }

        else if (sType == "Vector2Int")
        {
            if (string.IsNullOrEmpty(text))
            {
                buffer.Write(0);
                buffer.Write(0);
                return;
            }
            string[] s = text.Split(new char[] { '(', ')', ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (s.Length != 2 || !int.TryParse(s[0], out var v1) || !int.TryParse(s[1], out var v2))
            {
                Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                Console.ReadLine();
                return;
            }
            buffer.Write(v1);
            buffer.Write(v2);
        }
        else if (sType == "Vector2Int[]")
        {
            string[] arr = text.Split(new char[] { '[', ']', '(', ')', ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (arr.Length % 2 != 0)
            {
                Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                Console.ReadLine();
                return;
            }

            if (arrayTemp != null)
            {
                arrayTemp.Seek(0);
                arrayTemp.Write(arr.Length / 2);
                for (int i = 0; i < arr.Length; i += 2)
                {
                    if (!int.TryParse(arr[i], out var v1) || !int.TryParse(arr[i + 1], out var v2))
                    {
                        Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                        Console.ReadLine();
                        return;
                    }
                    arrayTemp.Write(v1);
                    arrayTemp.Write(v2);
                }
                buffer.Write(arrayTemp);
            }
            else
            {
                buffer.Write(arr.Length / 2);
                for (int i = 0; i < arr.Length; i += 2)
                {
                    if (!int.TryParse(arr[i], out var v1) || !int.TryParse(arr[i + 1], out var v2))
                    {
                        Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                        Console.ReadLine();
                        return;
                    }
                    buffer.Write(v1);
                    buffer.Write(v2);
                }
            }
        }

        else if (sType == "Vector3")
        {
            if (string.IsNullOrEmpty(text))
            {
                buffer.Write(0);
                buffer.Write(0);
                return;
            }
            string[] s = text.Split(new char[] { '(', ')', ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (s.Length != 3 || !float.TryParse(s[0], out var v1) || !float.TryParse(s[1], out var v2) || !float.TryParse(s[2], out var v3))
            {
                Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                Console.ReadLine();
                return;
            }
            buffer.Write(v1);
            buffer.Write(v2);
            buffer.Write(v3);
        }
        else if (sType == "Vector3[]")
        {
            string[] arr = text.Split(new char[] { '[', ']', '(', ')', ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (arr.Length % 3 != 0)
            {
                Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                Console.ReadLine();
                return;
            }

            if (arrayTemp != null)
            {
                arrayTemp.Seek(0);
                arrayTemp.Write(arr.Length / 3);
                for (int i = 0; i < arr.Length; i += 3)
                {
                    if (!float.TryParse(arr[i], out var v1) || !float.TryParse(arr[i + 1], out var v2) || !float.TryParse(arr[2], out var v3))
                    {
                        Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                        Console.ReadLine();
                        return;
                    }
                    arrayTemp.Write(v1);
                    arrayTemp.Write(v2);
                    arrayTemp.Write(v3);
                }
                buffer.Write(arrayTemp);
            }
            else
            {
                buffer.Write(arr.Length / 3);
                for (int i = 0; i < arr.Length; i += 3)
                {
                    if (!float.TryParse(arr[i], out var v1) || !float.TryParse(arr[i + 1], out var v2) || !float.TryParse(arr[2], out var v3))
                    {
                        Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                        Console.ReadLine();
                        return;
                    }
                    buffer.Write(v1);
                    buffer.Write(v2);
                    buffer.Write(v3);
                }
            }
        }

        else if (sType == "Vector3Int")
        {
            if (string.IsNullOrEmpty(text))
            {
                buffer.Write(0);
                buffer.Write(0);
                return;
            }
            string[] s = text.Split(new char[] { '(', ')', ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (s.Length != 3 || !int.TryParse(s[0], out var v1) || !int.TryParse(s[1], out var v2) || !int.TryParse(s[2], out var v3))
            {
                Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                Console.ReadLine();
                return;
            }
            buffer.Write(v1);
            buffer.Write(v2);
            buffer.Write(v3);
        }
        else if (sType == "Vector3Int[]")
        {
            string[] arr = text.Split(new char[] { '[', ']', '(', ')', ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (arr.Length % 3 != 0)
            {
                Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                Console.ReadLine();
                return;
            }

            if (arrayTemp != null)
            {
                arrayTemp.Seek(0);
                arrayTemp.Write(arr.Length / 3);
                for (int i = 0; i < arr.Length; i += 3)
                {
                    if (!int.TryParse(arr[i], out var v1) || !int.TryParse(arr[i + 1], out var v2) || !int.TryParse(arr[2], out var v3))
                    {
                        Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                        Console.ReadLine();
                        return;
                    }
                    arrayTemp.Write(v1);
                    arrayTemp.Write(v2);
                    arrayTemp.Write(v3);
                }
                buffer.Write(arrayTemp);
            }
            else
            {
                buffer.Write(arr.Length / 3);
                for (int i = 0; i < arr.Length; i += 3)
                {
                    if (!int.TryParse(arr[i], out var v1) || !int.TryParse(arr[i + 1], out var v2) || !int.TryParse(arr[2], out var v3))
                    {
                        Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                        Console.ReadLine();
                        return;
                    }
                    buffer.Write(v1);
                    buffer.Write(v2);
                    buffer.Write(v3);
                }
            }
        }

        else if (sType == "float")
        {
            if (string.IsNullOrEmpty(text))
            {
                buffer.Write(0f);
                return;
            }
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
            string[] arr = text.Split(new char[] { '[', ']', ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (arrayTemp != null)
            {
                arrayTemp.Seek(0);
                arrayTemp.Write(arr.Length);
                for (int i = 0; i < arr.Length; i++)
                {
                    if (!float.TryParse(arr[i], out var v))
                    {
                        Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                        Console.ReadLine();
                        return;
                    }
                    arrayTemp.Write(v);
                }
                buffer.Write(arrayTemp);
            }
            else
            {
                buffer.Write(arr.Length);
                for (int i = 0; i < arr.Length; i++)
                {
                    if (!float.TryParse(arr[i], out var v))
                    {
                        Console.WriteLine("解析出错 " + fi.Name + "  行:" + lineIdx + "  列:" + idx + "  类型:" + sType + "  值:" + text);
                        Console.ReadLine();
                        return;
                    }
                    buffer.Write(v);
                }
            }
        }

        else
        {
            Console.WriteLine("未识别类型->" + sType);
            Console.ReadLine();
        }
    }

    public static List<string> getFiles(string path)
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
    public static void GetHead(List<int> coIdx, ExcelPackage pkg, out temp1 t)
    {
        t = new temp1();
        t.keyType = pkg.Workbook.Worksheets[0].Cells[2, 1].Text;
        t.keyName = pkg.Workbook.Worksheets[0].Cells[3, 1].Text;
        if (string.IsNullOrEmpty(t.keyType) || string.IsNullOrEmpty(t.keyName))
        {
            Console.WriteLine(pkg.File.Name + "表没有key索引");
            Console.ReadLine();
            return;
        }
        var array = (object[,])pkg.Workbook.Worksheets[0].Cells.Value;
        int len = array.GetLength(1);
        for (int i = 1; i <= len; i++)
        {
            if (string.IsNullOrEmpty(pkg.Workbook.Worksheets[0].Cells[2, i].Text))
                continue;
            if (string.IsNullOrEmpty(pkg.Workbook.Worksheets[0].Cells[3, i].Text))
                continue;
            coIdx.Add(i);
        }
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
class temp3
{
    public FileInfo fi;
    public List<int> dataLines;
}
class Lan
{
    public DBuffer buff;
    public string name;
}
