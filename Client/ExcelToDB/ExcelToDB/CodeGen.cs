using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using OfficeOpenXml;

class DClass
{
    public string name;

    public List<DField> fs = new List<DField>();
    public List<DField> ecs = new List<DField>();
    public List<DFieldValue[]> fv = new List<DFieldValue[]>();
}
class DField
{
    public DClass dc;
    public int index;//excel表下标
    public int arrayIndex;//DClass的fv的数组下标

    public string name;
    public string des;

    public FType f1;
    public FType2 f2;
    public string typeStr;
    public string typeStrECS;
    public string realType;
}
class DFieldValue
{
    public long v64;
    public float vf;
    public object vo;

    public int xi;
    public int yi;
    public int zi;

    public float xf;
    public float yf;
    public float zf;
}
enum FType
{
    fbool,//fbool
    fint,//int
    fuint,//uint
    flong,//long
    fulong,//ulong

    ffloat,//float
    fstring,//string

    fv2i,//int2
    fv3i,//int3
    fv2f,//float2
    fv3f,//float3
}
enum FType2
{
    Value,
    Array,
    Array2,//2维数组
}
public struct V2int
{
    public int x;
    public int y;
}
public struct V2float
{
    public float x;
    public float y;
}
public struct V3int
{
    public int x;
    public int y;
    public int z;
}
public struct V3float
{
    public float x;
    public float y;
    public float z;
}
class CodeGen
{
    public string name;

    public string excelPath;
    public string codePath;
    public string dataPath;
    public bool genMapping = false;
    public bool genEcs = false;

    public void Gen()
    {
        {
            List<string> mains = Common.getFiles(excelPath);

            StringBuilder rw = new StringBuilder();
            StringBuilder df = new StringBuilder();
            StringBuilder erw = new StringBuilder();
            StringBuilder edf = new StringBuilder();

            List<DClass> cs = new();
            for (int i = 0; i < mains.Count; i++)
            {
                FileInfo fi = new FileInfo(mains[i]);
                Console.WriteLine("开始解析->" + fi.Name);

                DClass c = new DClass();
                cs.Add(c);
                c.name = fi.Name.Split('.')[0];

                ExcelPackage pkg = new ExcelPackage(fi);

                var sheet = pkg.Workbook.Worksheets[0];
                var keyType = sheet.Cells[2, 1].Text;
                var keyName = sheet.Cells[3, 1].Text;
                if (string.IsNullOrEmpty(keyType) || string.IsNullOrEmpty(keyName))
                {
                    Console.WriteLine(pkg.File.Name + "表没有key索引");
                    Console.ReadLine();
                    return;
                }
                var array = (object[,])sheet.Cells.Value;
                int len = array.GetLength(1);
                int arrayIdx = 0;
                for (int j = 1; j <= len; j++)
                {
                    bool nor = j == 1;
                    bool ecs = j == 1;
                    if (j > 1)
                    {
                        string s = sheet.Cells[4, j].Text.ToLower();
                        nor = s.Contains('c');
                        ecs = s.Contains('e');
                        if (!nor && !ecs)
                            continue;
                    }

                    DField f = new DField();

                    f.dc = c;
                    f.index = j;
                    f.arrayIndex = arrayIdx++;

                    f.name = sheet.Cells[3, j].Text;
                    f.des = sheet.Cells[1, j].Text.Replace("\n", "\n    /// ");

                    f.f1 = Common.GetFtype1(sheet.Cells[2, j].Text);
                    f.f2 = Common.GetFtype2(sheet.Cells[2, j].Text);
                    f.typeStr = Common.GetFTypeStr(sheet.Cells[2, j].Text);
                    f.typeStrECS = Common.GetFTypeStrECS(sheet.Cells[2, j].Text);
                    f.realType = Common.GetFRealType(sheet.Cells[2, j].Text);

                    if (nor)
                        c.fs.Add(f);
                    if (ecs)
                        c.ecs.Add(f);
                }
                int dataLen = array.GetLength(0);
                for (int j = 5; j <= dataLen; j++)
                {
                    if (string.IsNullOrEmpty(sheet.Cells[j, 1].Text))
                        continue;
                    DFieldValue[] fvs = new DFieldValue[c.fs.Count];
                    c.fv.Add(fvs);
                    for (int k = 0; k < c.fs.Count; k++)
                    {
                        var f = c.fs[k];
                        var s = sheet.Cells[j, f.index].Text;
                        if (!Common.GetFv(f, s, out var fv))
                        {
                            Console.WriteLine($"解析出错 {f.dc.name}  {j}-{(char)('A' + f.index - 1)} 类型:{f.typeStr} 字符:{s}");
                            return;
                        }
                        fvs[k] = fv;
                    }
                }
            }

            Console.WriteLine("写入数据");

            rw.AppendLine($"using System.Collections.Generic;");
            rw.AppendLine($"using UnityEngine;");
            rw.AppendLine($"using System.Linq;");
            rw.AppendLine($"");
            rw.AppendLine($"public static class {name}");
            rw.AppendLine($"{{");
            rw.AppendLine($"    static DBuffer dbbuff;");
            rw.AppendLine($"    static bool loadAll;");
            rw.AppendLine($"");
            for (int i = 0; i < cs.Count; i++)
            {
                var c = cs[i];
                rw.AppendLine($"    static Dictionary<{c.fs[0].typeStr}, TabMapping> _map{c.name}Idx;");
                rw.AppendLine($"    static Tab{c.name}[] _{c.name}Array;");
                rw.AppendLine($"    static Dictionary<{c.fs[0].typeStr}, Tab{c.name}> _map{c.name};");
                rw.AppendLine($"    public static Tab{c.name}[] {c.name}Array");
                rw.AppendLine($"    {{");
                rw.AppendLine($"        get");
                rw.AppendLine($"        {{");
                rw.AppendLine($"            if (_{c.name}Array == null)");
                rw.AppendLine($"            {{");
                rw.AppendLine($"                bool isLoadAll = loadAll;");
                rw.AppendLine($"                {c.fs[0].typeStr}[] keys = _map{c.name}Idx.Keys.ToArray();");
                rw.AppendLine($"                int len = keys.Length;");
                rw.AppendLine($"                _{c.name}Array = new Tab{c.name}[_map{c.name}Idx.Count];");
                rw.AppendLine($"                for (int i = 0; i < len; i++)");
                rw.AppendLine($"                {{");
                rw.AppendLine($"                    {c.fs[0].typeStr} k = keys[i];");
                rw.AppendLine($"                    TabMapping v = _map{c.name}Idx[k];");
                rw.AppendLine($"                    if (_map{c.name}.TryGetValue(k, out Tab{c.name} value))");
                rw.AppendLine($"                        _{c.name}Array[v.index] = value;");
                rw.AppendLine($"                    else");
                rw.AppendLine($"                    {{");
                rw.AppendLine($"                        dbbuff.Seek(v.point);");
                rw.AppendLine($"                        Tab{c.name} tmp = new Tab{c.name}(dbbuff, isLoadAll);");
                rw.AppendLine($"                        _map{c.name}[k] = tmp;");
                rw.AppendLine($"                        _{c.name}Array[v.index] = tmp;");
                rw.AppendLine($"                    }}");
                rw.AppendLine($"                }}");
                rw.AppendLine($"                _map{c.name}Idx = null;");
                rw.AppendLine($"            }}");
                rw.AppendLine($"            return _{c.name}Array;");
                rw.AppendLine($"        }}");
                rw.AppendLine($"    }}");
                rw.AppendLine($"");
            }
            rw.AppendLine($"    public static void Init(DBuffer buffer, bool isLoadAll)");
            rw.AppendLine($"    {{");
            rw.AppendLine($"        dbbuff = buffer;");
            rw.AppendLine($"        loadAll = isLoadAll;");
            rw.AppendLine($"");
            for (int i = 0; i < cs.Count; i++)
            {
                var c = cs[i];
                rw.AppendLine(@$"        int len{i} = buffer.Readint();
        _map{c.name}Idx = new Dictionary<{c.fs[0].typeStr}, TabMapping>(len{i});
        _map{c.name} = new Dictionary<{c.fs[0].typeStr}, Tab{c.name}>(len{i});
        _{c.name}Array = null;
        for (int i = 0; i < len{i}; i++)
        {{
            int offset = buffer.Readint();
            TabMapping map = new TabMapping();
            map.point = buffer.Position;
            map.index = i;
            _map{c.name}Idx.Add(buffer.Read{c.fs[0].typeStr}(), map);
            buffer.Seek(map.point + offset);
        }}");
            }
            rw.AppendLine($"        if (loadAll)");
            rw.AppendLine($"        {{");
            for (int i = 0; i < cs.Count; i++)
                rw.AppendLine($"            _ = {cs[i].name}Array;");
            rw.AppendLine($"        }}");
            rw.AppendLine($"    }}");
            for (int i = 0; i < cs.Count; i++)
            {
                var c = cs[i];
                rw.AppendLine($"    public static bool Has{c.name}({c.fs[0].typeStr} key) => (_map{c.name}Idx != null && _map{c.name}Idx.ContainsKey(key)) || _map{c.name}.ContainsKey(key);");
                rw.AppendLine($"    public static Tab{c.name} Get{c.name}({c.fs[0].typeStr} key)");
                rw.AppendLine($"    {{");
                rw.AppendLine($"        if (_map{c.name}.TryGetValue(key, out var value))");
                rw.AppendLine($"            return value;");
                rw.AppendLine($"        if (_map{c.name}Idx != null && _map{c.name}Idx.TryGetValue(key, out TabMapping map))");
                rw.AppendLine($"        {{");
                rw.AppendLine($"            dbbuff.Seek(map.point);");
                rw.AppendLine($"            Tab{c.name} tmp = new Tab{c.name}(dbbuff, loadAll);");
                rw.AppendLine($"            _map{c.name}[key] = tmp;");
                rw.AppendLine($"            return tmp;");
                rw.AppendLine($"        }}");
                rw.AppendLine($"        Loger.Error(\"Tab{c.name}表没有key: \" + key);");
                rw.AppendLine($"        return null;");
                rw.AppendLine($"    }}");
            }
            rw.AppendLine($"}}");
            for (int i = 0; i < cs.Count; i++)
            {
                var c = cs[i];
                rw.AppendLine($"public partial class Tab{c.name}");
                rw.AppendLine($"{{");
                rw.AppendLine($"    DBuffer dbuff;");

                for (int j = 0; j < c.fs.Count; j++)
                {
                    var f = c.fs[j];
                    if (f.f2 == FType2.Array || f.f2 == FType2.Array2 || f.f1 == FType.fstring)
                    {
                        rw.AppendLine($"");
                        rw.AppendLine($"    int _{f.name}Idx;");
                        rw.AppendLine($"    {f.typeStr} _{f.name}Tmp;");
                        rw.AppendLine($"    {f.typeStr} get{f.name}()");
                        rw.AppendLine($"    {{");
                        rw.AppendLine($"        if (_{f.name}Tmp == null)");
                        rw.AppendLine($"        {{");
                        rw.AppendLine($"            dbuff.Seek(_{f.name}Idx);");
                        rw.AppendLine($"            _{f.name}Tmp = dbuff.Read{f.typeStr.Replace("[]", "s")}();");
                        rw.AppendLine($"        }}");
                        rw.AppendLine($"        return _{f.name}Tmp;");
                        rw.AppendLine($"    }}");
                    }
                }
                rw.AppendLine($"");
                rw.AppendLine($"    public Tab{c.name}(DBuffer buffer, bool loadAll = false)");
                rw.AppendLine($"    {{");
                rw.AppendLine($"        dbuff = buffer;");
                for (int j = 0; j < c.fs.Count; j++)
                {
                    var f = c.fs[j];
                    if (f.f2 == FType2.Array || f.f2 == FType2.Array2)
                        rw.AppendLine($"        buffer.Seek(buffer.Readint() + (this._{f.name}Idx = buffer.Position));");
                    else if (f.f1 == FType.fstring)
                    {
                        rw.AppendLine($"        this._{f.name}Idx = buffer.Position;");
                        rw.AppendLine($"        buffer.Seek(buffer.Readint() + buffer.Position);");
                    }
                    else
                        rw.AppendLine($"        this.{f.name} = buffer.Read{f.typeStr}();");
                }
                rw.AppendLine($"        if (loadAll)");
                rw.AppendLine($"        {{");
                for (int j = 0; j < c.fs.Count; j++)
                {
                    var f = c.fs[j];
                    if (f.f2 == FType2.Array || f.f2 == FType2.Array2 || f.f1 == FType.fstring)
                        rw.AppendLine($"            _ = this.{f.name};");
                }
                rw.AppendLine($"        }}");
                rw.AppendLine($"    }}");
                rw.AppendLine($"}}");
            }
            if (genMapping)
            {
                rw.AppendLine($@"public class TabMapping
{{
    public int point;
    public int index;
}}");
            }

            df.AppendLine($"using UnityEngine;");
            for (int i = 0; i < cs.Count; i++)
            {
                var c = cs[i];
                df.AppendLine($"");
                df.AppendLine($"public partial class Tab{c.name}");
                df.AppendLine($"{{");
                for (int j = 0; j < c.fs.Count; j++)
                {
                    var f = c.fs[j];
                    df.AppendLine($"    /// <summary>");
                    df.AppendLine($"    /// {f.des}");
                    df.AppendLine($"    /// </summary>");
                    if (f.f2 == FType2.Array || f.f2 == FType2.Array2 || f.f1 == FType.fstring)
                        df.AppendLine($"    public {f.typeStr} {f.name} => get{f.name}();");
                    else
                        df.AppendLine($"    public {f.typeStr} {f.name} {{ get; }}");
                }
                df.AppendLine($"}}");
            }

            DBuffer buffer = new DBuffer(new MemoryStream(10000000));
            buffer.WriteHeaderInfo();
            DBuffer tmpC = new DBuffer(new MemoryStream(new byte[100000], 0, 100000, true, true));
            DBuffer arrTmp = new DBuffer(new MemoryStream(new byte[10000], 0, 10000, true, true));
            for (int i = 0; i < cs.Count; i++)
            {
                var c = cs[i];
                buffer.Write(c.fv.Count);

                for (int j = 0; j < c.fv.Count; j++)
                {
                    tmpC.Seek(0);
                    var vs = c.fv[j];
                    for (int k = 0; k < vs.Length; k++)
                    {
                        var v = vs[k];
                        Common.WriteFv(c.fs[k], v, tmpC, arrTmp);
                    }
                    buffer.Write(tmpC);
                }
            }

            File.WriteAllBytes(dataPath + ".bytes", buffer.ToBytes());
            File.WriteAllText(codePath + ".cs", rw.ToString());
            File.WriteAllText(codePath + "Define.cs", df.ToString());

            //ecs
            if (genEcs)
            {
                erw.AppendLine($"using Unity.Collections;");
                erw.AppendLine($"using Unity.Mathematics;");
                erw.AppendLine($"using Unity.Collections.LowLevel.Unsafe;");
                erw.AppendLine($"using Unity.Burst;");
                erw.AppendLine($"");
                erw.AppendLine($"public unsafe struct {name}_ST");
                erw.AppendLine($"{{");
                erw.AppendLine($"    public readonly static SharedStatic<{name}_ST> Tab = SharedStatic<{name}_ST>.GetOrCreate<SharedStatic<{name}_ST>>();");
                erw.AppendLine();
                for (int i = 0; i < cs.Count; i++)
                {
                    var c = cs[i];
                    if (c.ecs.Count > 1)
                        erw.AppendLine($"    public readonly NativeArray<{c.name}_ST> {c.name}Array;");
                }
                erw.AppendLine($"");
                for (int i = 0; i < cs.Count; i++)
                {
                    var c = cs[i];
                    if (c.ecs.Count > 1)
                        erw.AppendLine($"    readonly NativeHashMap<{c.fs[0].typeStrECS}, {c.name}_ST> {c.name}Map;");
                }
                erw.AppendLine($"    public static void Init(DBuffer buffer)");
                erw.AppendLine($"    {{");
                erw.AppendLine($"        Tab.Data.Dispose();");
                for (int i = 0; i < cs.Count; i++)
                {
                    var c = cs[i];
                    if (c.ecs.Count > 1)
                    {
                        erw.AppendLine($"        int len{i} = buffer.Readint();");
                        erw.AppendLine($"        fixed (NativeArray<{c.name}_ST>* ptr = &Tab.Data.{c.name}Array) *ptr = new NativeArray<{c.name}_ST>(len{i}, Allocator.Persistent);");
                        erw.AppendLine($"        fixed (NativeHashMap<{c.fs[0].typeStrECS}, {c.name}_ST>* ptr = &Tab.Data.{c.name}Map) *ptr = new NativeHashMap<{c.fs[0].typeStrECS}, {c.name}_ST>(len{i}, AllocatorManager.Persistent);");
                        erw.AppendLine($"        for (int i = 0; i < len{i}; i++)");
                        erw.AppendLine($"        {{");
                        erw.AppendLine($"            {c.name}_ST st = new {c.name}_ST(buffer);");
                        erw.AppendLine($"            UnsafeUtility.WriteArrayElement(Tab.Data.{c.name}Array.GetUnsafePtr(), i, st);");
                        erw.AppendLine($"            Tab.Data.{c.name}Map.Add(st.{c.fs[0].name}, st);");
                        erw.AppendLine($"        }}");
                    }
                }
                erw.AppendLine($"    }}");
                erw.AppendLine($"    public void Dispose()");
                erw.AppendLine($"    {{");
                for (int i = 0; i < cs.Count; i++)
                {
                    var c = cs[i];
                    if (c.ecs.Count > 1)
                    {
                        erw.AppendLine($"        for (int i = 0; i < {c.name}Array.Length; i++)");
                        erw.AppendLine($"        {{");
                        for (int j = 0; j < c.ecs.Count; j++)
                        {
                            if (c.ecs[j].f2== FType2.Array)
                            {
                                erw.AppendLine($"            {c.name}Array[i].{c.ecs[j].name}.Dispose();");
                            }
                        }
                        erw.AppendLine($"        }}");
                        erw.AppendLine($"        {c.name}Array.Dispose();");
                        erw.AppendLine($"        {c.name}Map.Dispose();");
                    }
                }
                erw.AppendLine($"    }}");
                erw.AppendLine($"");
                for (int i = 0; i < cs.Count; i++)
                {
                    var c = cs[i];
                    if (c.ecs.Count > 1)
                    {
                        erw.AppendLine($"    [return: ReadOnly] public readonly bool Has{c.name}({c.fs[0].typeStrECS} key) => {c.name}Map.ContainsKey(key);");
                        erw.AppendLine($"    [return: ReadOnly] public readonly {c.name}_ST Get{c.name}({c.fs[0].typeStrECS} key) => {c.name}Map[key];");
                    }
                }
                erw.AppendLine($"}}");
                for (int i = 0; i < cs.Count; i++)
                {
                    var c = cs[i];
                    if (c.ecs.Count > 1)
                    {
                        erw.AppendLine($"public partial struct {c.name}_ST");
                        erw.AppendLine($"{{");
                        erw.AppendLine($"    public {c.name}_ST(DBuffer buffer)");
                        erw.AppendLine($"    {{");

                        for (int j = 0; j < c.ecs.Count; j++)
                        {
                            var f = c.ecs[j];
                            if (f.f2 == FType2.Value)
                            {
                                if (f.f1 == FType.fv2i || f.f1 == FType.fv2f)
                                    erw.AppendLine($"        this.{f.name} = new {f.typeStrECS}(buffer.Read{f.realType}(), buffer.Read{f.realType}());");
                                else if (f.f1 == FType.fv3i || f.f1 == FType.fv3f)
                                    erw.AppendLine($"        this.{f.name} = new {f.typeStrECS}(buffer.Read{f.realType}(), buffer.Read{f.realType}(), buffer.Read{f.realType}());");
                                else
                                    erw.AppendLine($"        this.{f.name} = buffer.Read{f.realType}();");
                            }
                            else if (f.f2 == FType2.Array)
                            {
                                erw.AppendLine($"        int len{j} = buffer.Readint();");
                                erw.AppendLine($"        this.{f.name} = new UnsafeList<{f.typeStrECS}>(len{j}, AllocatorManager.Persistent);");

                                if (f.f1 == FType.fv2i || f.f1 == FType.fv2f)
                                    erw.AppendLine($"        for (int i = 0; i < len{j}; i++) this.{f.name}.Add(new {f.typeStrECS}(buffer.Read{f.realType}(), buffer.Read{f.realType}()));");
                                else if (f.f1 == FType.fv3i || f.f1 == FType.fv3f)
                                    erw.AppendLine($"        for (int i = 0; i < len{j}; i++) this.{f.name}.Add(new {f.typeStrECS}(buffer.Read{f.realType}(), buffer.Read{f.realType}(), buffer.Read{f.realType}()));");
                                else
                                    erw.AppendLine($"        for (int i = 0; i < len{j}; i++) this.{f.name}.Add(buffer.Read{f.realType}());");
                            }
                        }

                        erw.AppendLine($"    }}");
                        erw.AppendLine($"}}");
                    }
                }


                edf.AppendLine($"using Unity.Collections;");
                edf.AppendLine($"using Unity.Mathematics;");
                edf.AppendLine($"using Unity.Collections.LowLevel.Unsafe;");
                edf.AppendLine($"");
                for (int i = 0; i < cs.Count; i++)
                {
                    var c = cs[i];
                    if (c.ecs.Count > 1)
                    {
                        edf.AppendLine($"public partial struct {c.name}_ST");
                        edf.AppendLine($"{{");
                        for (int j = 0; j < c.ecs.Count; j++)
                        {
                            var f = c.ecs[j];
                            edf.AppendLine($@"    /// <summary>
    /// {f.des}
    /// </summary>");
                            edf.AppendLine($"    [ReadOnly]");
                            if (f.f2 == FType2.Array)
                                edf.AppendLine($"    public UnsafeList<{f.typeStrECS}> {f.name};");
                            else
                                edf.AppendLine($"    public {f.typeStrECS} {f.name};");
                        }
                        edf.AppendLine($"}}");
                    }
                }

                DBuffer bufferEcs = new DBuffer(new MemoryStream(10000000));
                bufferEcs.WriteHeaderInfo();
                for (int i = 0; i < cs.Count; i++)
                {
                    var c = cs[i];
                    if (c.ecs.Count > 1)
                    {
                        bufferEcs.Write(c.fv.Count);

                        for (int j = 0; j < c.fv.Count; j++)
                        {
                            var vs = c.fv[j];
                            for (int k = 0; k < c.ecs.Count; k++)
                            {
                                var f = c.ecs[k];
                                var v = vs[f.arrayIndex];
                                Common.WriteFv(f, v, bufferEcs, null);
                            }
                        }
                    }
                }

                File.WriteAllBytes(dataPath + "_ST.bytes", bufferEcs.ToBytes());
                File.WriteAllText(codePath + "_ST.cs", erw.ToString());
                File.WriteAllText(codePath + "_STDefine.cs", edf.ToString());
            }

            Console.WriteLine("写入完成");
            Console.WriteLine("");
        }
    }
}
