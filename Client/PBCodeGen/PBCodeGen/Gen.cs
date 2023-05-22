using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

internal class Gen
{
    public string resPath;
    public string outDefinePath;
    public string outRWPath;//解析和写入的函数文件路径

    public void gen()
    {
        StringBuilder df = new StringBuilder(10000);
        StringBuilder rw = new StringBuilder(10000);

        if (!Directory.Exists(outDefinePath))
        {
            Directory.CreateDirectory(outDefinePath);
        }
        if (!Directory.Exists(outRWPath))
        {
            Directory.CreateDirectory(outRWPath);
        }
        foreach (var item in Directory.GetFiles(outDefinePath))
        {
            File.Delete(item);
        }

        rw.AppendLine("using System.Collections.Generic;");
        rw.AppendLine();

        HashSet<string> enums = new HashSet<string>();
        foreach (var item in Directory.GetFiles(resPath))
        {
            string[] strs = File.ReadAllLines(item);
            for (int i = 0; i < strs.Length; i++)
            {
                string s = strs[i];
                string s2 = s.Replace(" ", null);
                if (s2.StartsWith("enum"))
                {
                    string enumName = s.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1];
                    enums.Add(enumName);
                }
            }
        }

        foreach (var item in Directory.GetFiles(resPath))
        {
            if (!item.EndsWith(".proto"))
                continue;
            FileInfo fi = new FileInfo(item);
            string fiName = fi.Name.Replace(fi.Extension, null);
            Console.WriteLine("生成->" + fiName);

            df.Clear();
            df.AppendLine("using System.Collections.Generic;");
            df.AppendLine();
            df.AppendLine($"namespace PB.{fiName}");
            df.AppendLine("{");

            rw.AppendLine($"namespace PB.{fiName}");
            rw.AppendLine("{");

            {
                string[] strs = File.ReadAllLines(item);

                for (int i = 0; i < strs.Length; i++)
                {
                    string s = strs[i];
                    string s2 = s.Replace(" ", null);
                    if (s2.StartsWith("//"))//注释行
                    {
                        continue;
                    }
                    if (s2.StartsWith("message"))
                    {
                        string className = s.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1];

                        if (i > 0)
                        {
                            string ss = strs[i - 1];
                            string ss2 = ss.Replace(" ", null);
                            if (ss2.StartsWith("//"))
                            {
                                df.AppendLine($"    /// <summary>");
                                df.AppendLine($"    /// " + ss.Split("//", options: StringSplitOptions.RemoveEmptyEntries).LastOrDefault());
                                df.AppendLine($"    /// </summary>");
                            }
                        }
                        df.AppendLine($"    public partial class {className}");

                        rw.AppendLine($"    public partial class {className} : IPBMessage");
                        df.AppendLine("    {");
                        rw.AppendLine("    {");

                        StringBuilder wStr = new StringBuilder(10000);
                        StringBuilder rStr = new StringBuilder(10000);
                        wStr.AppendLine("        public void Write(PBWriter writer)");
                        wStr.AppendLine("        {");

                        rStr.AppendLine("        public void Read(PBReader reader)");
                        rStr.AppendLine("        {");
                        rStr.AppendLine("            int tag;");
                        rStr.AppendLine("            while ((tag = reader.ReadTag()) != 0)");
                        rStr.AppendLine("            {");
                        rStr.AppendLine("                switch (tag)");
                        rStr.AppendLine("                {");

                        int idx1 = i + 1;
                        int idx2 = i + 2;
                        for (int j = i + 2; j < strs.Length; j++)
                        {
                            if (strs[j].Contains("}"))
                            {
                                idx2 = j;
                                break;
                            }
                        }

                        for (int j = idx1 + 1; j < idx2; j++)
                        {
                            string ss = strs[j];
                            if (ss.Replace(" ", null).StartsWith("//"))//注释行
                            {
                                continue;
                            }
                            if (string.IsNullOrWhiteSpace(ss))
                            {
                                continue;
                            }

                            var arr = ss.Split("//", StringSplitOptions.RemoveEmptyEntries);
                            string des = arr.Length > 1 ? arr[1] : null;

                            var arr2 = arr[0].Split(new char[] { '=', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            if (arr2.Length != 3 && arr2.Length != 4)
                            {
                                Console.WriteLine($"无法识别协议定义 {fiName} {className} {ss}");
                                return;
                            }

                            bool isLst = arr2.Length == 4 && arr2[0] == "repeated";
                            bool isMap = arr2.Length == 3 && arr2[0].Contains("map<");
                            bool isBytes = arr2.Length == 3 && arr2[0] == "bytes";
                            bool isEnum = arr2.Length == 3 && enums.Contains(arr2[0]);

                            string rowType = arr2[arr2.Length - 3];
                            string realType = null;
                            if (!isMap)
                                realType = getType(rowType);

                            string kType = null;
                            string vType = null;
                            string kRowType = null;
                            string vRowType = null;
                            if (isMap)
                                getMapTypes(rowType, out kType, out vType, out kRowType, out vRowType);

                            string fieldName = arr2[arr2.Length - 2];
                            int mark = getMark(isLst, rowType);
                            int tag = (int.Parse(arr2[arr2.Length - 1]) << 3) | mark;
                            fieldType ft = getFieldTypeEnum(rowType);

                            if (des != null)
                            {
                                df.AppendLine("        /// <summary>");
                                df.AppendLine("        /// " + des);
                                df.AppendLine("        /// </summary>");
                            }
                            if (isLst)
                                df.AppendLine($"        public List<{realType}> {fieldName} {{ get; set; }} = new List<{realType}>();");
                            else if (isMap)
                            {
                                df.AppendLine($"        public Dictionary<{kType}, {vType}> {fieldName} {{ get; set; }} = new Dictionary<{kType}, {vType}>();");
                            }
                            else if (isBytes)
                            {
                                df.AppendLine($"        public byte[] {fieldName} {{ get; set; }}");
                            }
                            else if (isEnum)
                            {
                                df.AppendLine($"        public {realType} {fieldName}" + " { get; set; }");
                            }
                            else
                            {
                                if (ft == fieldType.Value)
                                {
                                    df.AppendLine($"        public {realType} {fieldName}" + " { get; set; }");
                                }
                                else
                                {
                                    df.AppendLine($"        public {realType} {fieldName} {{ get; set; }} = new {realType}();");
                                }
                            }

                            if (isLst)
                            {
                                if (ft == fieldType.Value)
                                {
                                    wStr.AppendLine($"            writer.Write{rowType}s({tag}, this.{fieldName});");
                                }
                                else
                                {
                                    wStr.AppendLine($"            if (this.{fieldName} != null)");
                                    wStr.AppendLine("            {");
                                    wStr.AppendLine($"                int len = this.{fieldName}.Count;");
                                    wStr.AppendLine($"                for (int i = 0; i < len; i++)");
                                    wStr.AppendLine($"                    writer.Writemessage({tag}, this.{fieldName}[i]);");
                                    wStr.AppendLine("            }");
                                }
                            }
                            else if (isMap)
                            {
                                int kmark = getMark(false, kRowType);
                                int ktag = (1 << 3) | kmark;
                                int vmark = getMark(false, vRowType);
                                int vtag = (2 << 3) | vmark;
                                wStr.AppendLine($"            if (this.{fieldName} != null)");
                                wStr.AppendLine("            {");
                                wStr.AppendLine($"                PBWriter tmp = PBBuffPool.Get();");
                                wStr.AppendLine($"                foreach (var item in this.{fieldName})");
                                wStr.AppendLine($"                {{");
                                wStr.AppendLine($"                    tmp.Seek(0);");

                                if (kRowType == "string")
                                    wStr.AppendLine($"                    tmp.Write{kRowType}({ktag}, item.Key);");
                                else
                                {
                                    wStr.AppendLine($"                    tmp.WriteTag({ktag});");
                                    wStr.AppendLine($"                    tmp.Write{kRowType}(item.Key);");
                                }

                                if (vRowType == "int32"
                                    || vRowType == "int64"
                                    || vRowType == "sint32"
                                    || vRowType == "sint64"
                                    || vRowType == "fixed32"
                                    || vRowType == "sfixed32"
                                    || vRowType == "fixed64"
                                    || vRowType == "sfixed64"
                                    || vRowType == "double"
                                    || vRowType == "float"
                                    || vRowType == "bool")
                                {
                                    wStr.AppendLine($"                    tmp.WriteTag({vtag});");
                                    wStr.AppendLine($"                    tmp.Write{vRowType}(item.Value);");
                                }
                                else if (vRowType == "string")
                                {
                                    wStr.AppendLine($"                    tmp.Write{vRowType}({vtag}, item.Value);");
                                }
                                else
                                {
                                    wStr.AppendLine($"                    tmp.Writemessage({vtag}, item.Value);");
                                }

                                wStr.AppendLine($"                    writer.WriteBuff({tag}, tmp);");
                                wStr.AppendLine($"                }}");
                                wStr.AppendLine($"                PBBuffPool.Return(tmp);");
                                wStr.AppendLine("            }");
                            }
                            else if (isBytes)
                            {
                                wStr.AppendLine($"            writer.Writebytes({tag}, this.{fieldName});");
                            }
                            else if (isEnum)
                            {
                                wStr.AppendLine($"            if (this.{fieldName} != 0)");
                                wStr.AppendLine("            {");
                                wStr.AppendLine($"                writer.WriteTag({tag});");
                                wStr.AppendLine($"                writer.Writeint32((int)this.{fieldName});");
                                wStr.AppendLine("            }");
                            }
                            else
                            {
                                if (realType == "int" || realType == "long" || realType == "uint" || realType == "ulong")
                                {
                                    wStr.AppendLine($"            if (this.{fieldName} != 0)");
                                    wStr.AppendLine("            {");
                                    wStr.AppendLine($"                writer.WriteTag({tag});");
                                    wStr.AppendLine($"                writer.Write{rowType}(this.{fieldName});");
                                    wStr.AppendLine("            }");
                                }
                                else if (realType == "bool")
                                {
                                    wStr.AppendLine($"            if (this.{fieldName})");
                                    wStr.AppendLine("            {");
                                    wStr.AppendLine($"                writer.WriteTag({tag});");
                                    wStr.AppendLine($"                writer.Write{rowType}(this.{fieldName});");
                                    wStr.AppendLine("            }");
                                }
                                else if (realType == "string")
                                {
                                    wStr.AppendLine($"            writer.Writestring({tag}, this.{fieldName});");
                                }
                                else if (realType == "float")
                                {
                                    wStr.AppendLine($"            if (this.{fieldName} != 0F)");
                                    wStr.AppendLine("            {");
                                    wStr.AppendLine($"                writer.WriteTag({tag});");
                                    wStr.AppendLine($"                writer.Write{rowType}(this.{fieldName});");
                                    wStr.AppendLine("            }");
                                }
                                else if (realType == "double")
                                {
                                    wStr.AppendLine($"            if (this.{fieldName} != 0D)");
                                    wStr.AppendLine("            {");
                                    wStr.AppendLine($"                writer.WriteTag({tag});");
                                    wStr.AppendLine($"                writer.Write{rowType}(this.{fieldName});");
                                    wStr.AppendLine("            }");
                                }
                                else
                                {
                                    wStr.AppendLine($"            writer.Writemessage({tag}, this.{fieldName});");
                                }
                            }

                            if (isLst)
                            {
                                if (rowType == "string")
                                {
                                    rStr.AppendLine($"                    case {tag}:");
                                    rStr.AppendLine($"                        reader.Read{rowType}s(tag, this.{fieldName});");
                                    rStr.AppendLine($"                        break;");
                                }
                                else if (ft == fieldType.Value)
                                {
                                    rStr.AppendLine($"                    case {tag}:");
                                    rStr.AppendLine($"                        reader.Read{rowType}s(this.{fieldName});");
                                    rStr.AppendLine($"                        break;");
                                }
                                else
                                {
                                    rStr.AppendLine($"                    case {tag}:");
                                    rStr.AppendLine("                        {");
                                    rStr.AppendLine("                            int point;");
                                    rStr.AppendLine($"                            int max = reader.max;");
                                    rStr.AppendLine($"                            do");
                                    rStr.AppendLine($"                            {{");
                                    rStr.AppendLine($"                                int size = reader.Readint32();");
                                    rStr.AppendLine($"                                point = reader.Position;");
                                    rStr.AppendLine($"                                reader.SetLimit(point, point + size);");
                                    rStr.AppendLine($"                                {rowType} message = new {rowType}();");
                                    rStr.AppendLine($"                                message.Read(reader);");
                                    rStr.AppendLine($"                                this.{fieldName}.Add(message);");
                                    rStr.AppendLine($"                                reader.SetLimit(point += size, max);");
                                    rStr.AppendLine($"                                reader.Seek(point);");
                                    rStr.AppendLine($"                            }} while (reader.ReadTag() == {tag});");
                                    rStr.AppendLine($"                            reader.Seek(point);");
                                    rStr.AppendLine("                        }");
                                    rStr.AppendLine($"                        break;");
                                }
                            }
                            else if (isMap)
                            {
                                int kmark = getMark(false, kRowType);
                                int ktag = (1 << 3) | kmark;
                                int vmark = getMark(false, kRowType);
                                int vtag = (2 << 3) | vmark;
                                rStr.AppendLine($"                    case {tag}:");
                                rStr.AppendLine($"                        {{");
                                rStr.AppendLine($"                            int point;");
                                rStr.AppendLine($"                            int max = reader.max;");
                                rStr.AppendLine($"                            do");
                                rStr.AppendLine($"                            {{");
                                rStr.AppendLine($"                                int tag2;");

                                if (kType == "string")
                                    rStr.AppendLine($"                                {kType} k = string.Empty;");
                                else
                                    rStr.AppendLine($"                                {kType} k = 0;");

                                if (vRowType == "string")
                                    rStr.AppendLine($"                                {vType} v = string.Empty;");
                                else if (vRowType == "bool")
                                    rStr.AppendLine($"                                {vType} v = false;");
                                else if (vRowType == "int32"
                                    || vRowType == "int64"
                                    || vRowType == "sint32"
                                    || vRowType == "sint64"
                                    || vRowType == "fixed32"
                                    || vRowType == "sfixed32"
                                    || vRowType == "fixed64"
                                    || vRowType == "sfixed64"
                                    || vRowType == "double"
                                    || vRowType == "float")
                                    rStr.AppendLine($"                                {vType} v = 0;");
                                else
                                    rStr.AppendLine($"                                {vType} v = new {vType}();");

                                rStr.AppendLine($"                                int size = reader.Readint32();");
                                rStr.AppendLine($"                                point = reader.Position;");
                                rStr.AppendLine($"                                reader.SetLimit(point, point + size);");
                                rStr.AppendLine($"                                while ((tag2 = reader.ReadTag()) != 0)");
                                rStr.AppendLine($"                                {{");
                                rStr.AppendLine($"                                    if (tag2 == {ktag})");
                                rStr.AppendLine($"                                        k = reader.Read{kRowType}();");
                                rStr.AppendLine($"                                    else if (tag2 == {vtag})");

                                if (vRowType == "int32"
                                    || vRowType == "int64"
                                    || vRowType == "sint32"
                                    || vRowType == "sint64"
                                    || vRowType == "fixed32"
                                    || vRowType == "sfixed32"
                                    || vRowType == "fixed64"
                                    || vRowType == "sfixed64"
                                    || vRowType == "double"
                                    || vRowType == "float"
                                    || vRowType == "string"
                                    || vRowType == "bool")
                                    rStr.AppendLine($"                                        v = reader.Read{vRowType}();");
                                else
                                    rStr.AppendLine($"                                        reader.Readmessage(v);");

                                rStr.AppendLine($"                                    else");
                                rStr.AppendLine($"                                        break;");
                                rStr.AppendLine($"                                }}");
                                rStr.AppendLine($"                                point += size;");
                                rStr.AppendLine($"                                reader.SetLimit(point, max);");
                                rStr.AppendLine($"                                reader.Seek(point);");
                                rStr.AppendLine($"                                this.{fieldName}[k] = v;");
                                rStr.AppendLine($"                            }} while (reader.ReadTag() == {tag});");
                                rStr.AppendLine($"                            reader.Seek(point);");
                                rStr.AppendLine($"                        }}");
                                rStr.AppendLine($"                        break;");
                            }
                            else if (isBytes)
                            {
                                rStr.AppendLine($"                    case {tag}:");
                                rStr.AppendLine($"                        this.{fieldName} = reader.Readbytes();");
                                rStr.AppendLine($"                        break;");
                            }
                            else if (isEnum)
                            {
                                rStr.AppendLine($"                    case {tag}:");
                                rStr.AppendLine($"                        this.{fieldName} = ({rowType})reader.Readint32();");
                                rStr.AppendLine($"                        break;");
                            }
                            else
                            {
                                if (ft == fieldType.Value)
                                {
                                    rStr.AppendLine($"                    case {tag}:");
                                    rStr.AppendLine($"                        this.{fieldName} = reader.Read{rowType}();");
                                    rStr.AppendLine($"                        break;");
                                }
                                else
                                {
                                    rStr.AppendLine($"                    case {tag}:");
                                    rStr.AppendLine($"                        reader.Readmessage(this.{fieldName});");
                                    rStr.AppendLine($"                        break;");
                                }
                            }
                        }

                        wStr.AppendLine("        }");
                        rStr.AppendLine("                    default:");
                        rStr.AppendLine("                        reader.SeekNext(tag);");
                        rStr.AppendLine("                        break;");
                        rStr.AppendLine("                }");
                        rStr.AppendLine("            }");
                        rStr.AppendLine("        }");

                        df.AppendLine("    }");
                        df.AppendLine();
                        rw.Append(wStr.ToString());
                        rw.Append(rStr.ToString());
                        rw.AppendLine("    }");
                    }
                    else if (s2.StartsWith("enum"))
                    {
                        string enumName = s.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1];

                        if (i > 0)
                        {
                            string ss = strs[i - 1];
                            string ss2 = ss.Replace(" ", null);
                            if (ss2.StartsWith("//"))
                            {
                                df.AppendLine($"    /// <summary>");
                                df.AppendLine($"    /// " + ss.Split("//", options: StringSplitOptions.RemoveEmptyEntries).LastOrDefault());
                                df.AppendLine($"    /// </summary>");
                            }
                        }
                        df.AppendLine($"    public enum {enumName}");
                        df.AppendLine($"    {{");

                        int idx1 = i + 1;
                        int idx2 = i + 2;
                        for (int j = i + 2; j < strs.Length; j++)
                        {
                            if (strs[j].Contains("}"))
                            {
                                idx2 = j;
                                break;
                            }
                        }
                        for (int j = idx1 + 1; j < idx2; j++)
                        {
                            string ss = strs[j];
                            if (ss.Replace(" ", null).StartsWith("//"))//注释行
                            {
                                continue;
                            }
                            if (string.IsNullOrWhiteSpace(ss))
                            {
                                continue;
                            }

                            var arr = ss.Split("//", StringSplitOptions.RemoveEmptyEntries);
                            string des = arr.Length > 1 ? arr[1] : null;

                            var arr2 = arr[0].Split(new char[] { '=', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            if (arr2.Length != 2)
                            {
                                Console.WriteLine($"无法识别协议定义 {fiName} {enumName} {ss}");
                                return;
                            }

                            if (des != null)
                            {
                                df.AppendLine($"        /// <summary>");
                                df.AppendLine($"        /// " + des);
                                df.AppendLine($"        /// </summary>");
                            }
                            df.AppendLine($"        {arr2[0]} = {arr2[1]},");
                        }

                        df.AppendLine($"    }}");
                        df.AppendLine();
                    }
                }
            }

            df.AppendLine("}");
            rw.AppendLine("}");

            File.WriteAllText(outDefinePath + $"/{fiName}.cs",df.ToString());
        }
        File.WriteAllText(outRWPath + $"/RW.cs", rw.ToString());
    }

    string getType(string type)
    {
        if (type == "int32" || type == "sint32" || type == "sfixed32") return "int";
        if (type == "int64" || type == "sint64" || type == "sfixed64") return "long";
        if (type == "fixed32") return "uint";
        if (type == "fixed64") return "ulong";
        if (type == "double") return "double";
        if (type == "bytes") return "byte[]";
        return type;
    }
    int getMark(bool isLst,string type)
    {
        if (isLst)
            return 2;
        if (type == "int32"
            || type == "sint32"
            || type == "sint64"
            || type == "int64"
            || type == "bool")
            return 0;
        else if (type == "fixed64"
              || type == "sfixed64"
              || type == "double")
            return 1;
        else if (type == "string"
              || type == "bytes")
            return 2;
        else if (type == "float"
            || type == "fixed32"
            || type == "sfixed32")
            return 5;
        return 2;//嵌套类型
    }
    void getMapTypes(string s, out string kt, out string vt, out string kkt, out string vvt)
    {
        s = s.Replace("map<", null).Replace(">", null);
        string[] ss = s.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        if (ss.Length != 2)
            throw new Exception("无法解析map类型=" + s);

        if (ss[0] != "int32"
            && ss[0] != "int64"
            && ss[0] != "sint32"
            && ss[0] != "sint64"
            && ss[0] != "fixed32"
            && ss[0] != "sfixed32"
            && ss[0] != "fixed64"
            && ss[0] != "sfixed64"
            && ss[0] != "string")
        {
            throw new Exception("暂不支持map的key类型=" + ss[0]);
        }

        kt = getType(kkt = ss[0]);
        vt = getType(vvt = ss[1]);
    }

    fieldType getFieldTypeEnum(string type)
    {
        if (type == "bool"
            || type == "int32"
            || type == "sint32"
            || type == "int64"
            || type == "sint64"
            || type == "fixed32"
            || type == "sfixed32"
            || type == "fixed64"
            || type == "sfixed64"
            || type == "double"
            || type == "float"
            || type == "string")
            return fieldType.Value;
        return fieldType.Msg;
    }

    enum fieldType
    {
        None,
        Value,
        Msg,
    }
}

