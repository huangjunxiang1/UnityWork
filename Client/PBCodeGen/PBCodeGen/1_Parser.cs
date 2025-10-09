using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

internal class Parser
{
    public string path;

    public PBParserResult parse()
    {
        PBParserResult ret = new();
        Console.WriteLine("解析class");
        foreach (var item in Directory.GetFiles(path, "*.proto"))
        {
            PBType pb = new();
            ret.pbs.Add(pb);
            pb.name = new FileInfo(item).Name.Split('.').First();
            var lines = File.ReadAllLines(item);

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                if (line.StartsWith("package"))
                {
                    string ss = line.Split(new string[] { " ", ";" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    pb.refrences.Add(ss);
                }

                string s2 = line.Replace(" ", null);
                if (s2.StartsWith("message"))
                {
                    string className = line.Split(new string[] { " ", "{" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    PBClass c = new();
                    c.parent = pb;
                    c.name = className;
                    c.classType = PBClassType.v_messsage;
                    pb.classes.Add(c);
                    ret.classMap.Add(c.name, c);
                    ret.classMap.Add(c.fullName, c);
                    if (i > 0 && lines[i - 1].Replace(" ", null).StartsWith("//"))
                        c.summary = lines[i - 1];
                }
                else if (s2.StartsWith("enum"))
                {
                    string enumName = line.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1];

                    PBClass c = new();
                    c.parent = pb;
                    c.name = enumName;
                    c.classType = PBClassType.v_enum;
                    pb.classes.Add(c);
                    ret.classMap.Add(c.name, c);
                    ret.classMap.Add(c.fullName, c);
                    if (i > 0 && lines[i - 1].Replace(" ", null).StartsWith("//"))
                        c.summary = lines[i - 1];
                }
            }
        }
        Console.WriteLine("解析field");
        foreach (var item in Directory.GetFiles(path, "*.proto"))
        {
            var lines = File.ReadAllLines(item);

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                string s2 = line.Replace(" ", null);
                if (s2.StartsWith("message"))
                {
                    string className = line.Split(new string[] { " ", "{" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    if (!ret.classMap.TryGetValue(className, out var pb))
                    {
                        Console.WriteLine($"不存在class={className}");
                        return null;
                    }
                    int begin = i;
                    int end = i;
                    for (int j = i; j < lines.Length; j++)
                    {
                        if (lines[j].Contains("{"))
                            begin = j;
                        if (lines[j].Contains("}"))
                        {
                            end = j;
                            break;
                        }
                    }
                    i = end;
                    for (int j = begin + 1; j < end; j++)
                    {
                        string ss = lines[j];
                        if (ss.Replace(" ", null).StartsWith("//"))//注释行
                            continue;
                        if (string.IsNullOrWhiteSpace(ss))
                            continue;

                        var arr = ss.Split("//", StringSplitOptions.RemoveEmptyEntries);

                        var arr2 = arr[0].Split(new char[] { '\t', '=', ';', ' ', '<', '>', ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (arr2.Length < 3 || arr2.Length > 5)
                        {
                            Console.WriteLine($"无法识别协议定义 {new FileInfo(item).Name} {className} {ss}");
                            return null;
                        }

                        FieldObject field = new();
                        pb.fields.Add(field);
                        field.parent = pb;
                        field.summary = arr.Length > 1 ? string.Join(" ", arr[1..]) : null;

                        field.ParseSplitStrings(arr2, ret.classMap);
                    }
                }
                else if (s2.StartsWith("enum"))
                {
                    string className = line.Split(new string[] { " ", "{" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    if (!ret.classMap.TryGetValue(className, out var pb))
                    {
                        Console.WriteLine($"不存在enum={className}");
                        return null;
                    }
                    int begin = i;
                    int end = i;
                    for (int j = i; j < lines.Length; j++)
                    {
                        if (lines[j].Contains("{"))
                            begin = j;
                        if (lines[j].Contains("}"))
                        {
                            end = j;
                            break;
                        }
                    }
                    i = end;

                    for (int j = begin + 1; j < end; j++)
                    {
                        string ss = lines[j];
                        if (ss.Replace(" ", null).StartsWith("//"))//注释行
                            continue;
                        if (string.IsNullOrWhiteSpace(ss))
                            continue;

                        var arr = ss.Split("//", StringSplitOptions.RemoveEmptyEntries);

                        var arr2 = arr[0].Split(new char[] { '\t', '=', ';', ' ', '<', '>', ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (arr2.Length != 2)
                        {
                            Console.WriteLine($"无法识别协议定义 {new FileInfo(item).Name} {className} {ss}");
                            return null;
                        }

                        FieldObject field = new();
                        pb.fields.Add(field);
                        field.parent = pb;
                        field.summary = arr.Length > 1 ? string.Join(" ", arr[1..]) : null;
                        field.name = arr2[0];
                        field.tag = int.Parse(arr2[1]);
                    }
                }
            }
        }

        return ret;
    }

}

