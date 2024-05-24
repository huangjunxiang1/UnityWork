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
    public bool isSingle;

    public List<DField> fs = new List<DField>();
    public List<DField> ecs = new List<DField>();
    public List<DField> groups = new List<DField>();
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
    public string dataPath2;
    public bool genMapping = false;
    public bool genEcs = false;

    public void Gen()
    {
        {
            List<string> mains = Common.getFiles(excelPath);

            StringBuilder rw = new StringBuilder();
            StringBuilder erw = new StringBuilder();
            DBuffer buffer = new DBuffer(new MemoryStream(10000000));
            DBuffer bufferEcs = new DBuffer(new MemoryStream(10000000));

            List<DClass> cs = new();

            for (int i = 0; i < mains.Count; i++)
            {
                FileInfo fi = new FileInfo(mains[i]);
                Console.WriteLine("开始解析->" + fi.Name);

                ExcelPackage pkg = new ExcelPackage(fi);
                var sheet = pkg.Workbook.Worksheets[0];

                DClass c = new DClass();
                cs.Add(c);

                if (fi.Name.Contains("_Public"))
                {
                    c.name = "Public";
                    c.isSingle = true;

                    int arrayIdx2 = 0;
                    var array1 = (object[,])sheet.Cells.Value;
                    int len1 = array1 == null ? 0 : array1.GetLength(0);
                    DFieldValue[] fvs = new DFieldValue[len1];
                    c.fv.Add(fvs);
                    for (int x = 1; x <= len1; x++)
                    {
                        DField f = new DField();
                        var tag = sheet.Cells[x, 3].Text.ToLower();

                        f.dc = c;
                        f.index = x;
                        f.arrayIndex = arrayIdx2++;

                        f.name = sheet.Cells[x, 1].Text;
                        f.des = sheet.Cells[x, 2].Text.Replace("\n", "\n    /// ");

                        string v = sheet.Cells[x, 4].Text;
                        f.f1 = Common.GetFtype1(v);
                        f.f2 = Common.GetFtype2(v);
                        f.typeStr = Common.GetFTypeStr(v);
                        f.typeStrECS = Common.GetFTypeStrECS(v);
                        f.realType = Common.GetFRealType(v);

                        if (tag.Contains('c'))
                            c.fs.Add(f);
                        if (tag.Contains('e'))
                            c.ecs.Add(f);

                        if (string.IsNullOrEmpty(sheet.Cells[x, 5].Text))
                            continue;
                        var s = sheet.Cells[x, 5].Text;
                        if (!Common.GetFv(f, s, out var fv))
                        {
                            Console.WriteLine($"解析出错 {f.dc.name}  {4}-{(char)('A' + f.index - 1)} 类型:{f.typeStr} 字符:{s}");
                            return;
                        }
                        fvs[x - 1] = fv;
                    }
                    continue;
                }

                c.name = fi.Name.Split('.')[0];

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
                    bool group = false;
                    if (j > 1)
                    {
                        string s = sheet.Cells[4, j].Text.ToLower();
                        nor = s.Contains('c');
                        ecs = s.Contains('e');
                        group = s.Contains('g');
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
                    if(group)
                    {
                        if (f.f2 != FType2.Value || f.f1 == FType.ffloat || f.f1 == FType.fv2f || f.f1 == FType.fv3f)
                        {
                            Console.WriteLine($"{fi.Name} 的组标记字段 {f.name}  不能为数组或者 float 类型");
                            continue;
                        }
                        c.groups.Add(f);
                    }
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
            rw.AppendLine($"using System.Linq;");
            rw.AppendLine($"using System;");
            rw.AppendLine($"using Unity.Mathematics;");
            if (genEcs)
            {
                erw.AppendLine($"using System.Collections.Generic;");
                erw.AppendLine($"using System.Linq;");
                erw.AppendLine($"using System;");
                erw.AppendLine($"using Unity.Mathematics;");
                erw.AppendLine($"using Unity.Collections;");
                erw.AppendLine($"using Unity.Collections.LowLevel.Unsafe;");
                erw.AppendLine($"using Unity.Burst;");
            }
            rw.AppendLine($"");
            if (genMapping)
            {
                rw.AppendLine($@"public struct TabMapping
{{
    public TabMapping(int point, int index) {{ this.point = point; this.index = index; }}
    public int point;
    public int index;
}}");
            }
            rw.AppendLine($"public static class {name}");
            rw.AppendLine($"{{");
            rw.AppendLine(@$"    static DBuffer dbbuff; static bool loadAll; static string[] stringCache; static int[] stringIndex;
    internal static string __getstring(int index) {{ if (stringCache[index] == null) {{ dbbuff.Seek(stringIndex[index]); stringCache[index] = dbbuff.Readstring(); }} return stringCache[index]; }}
    internal static string[] __getstrings(int point) {{ dbbuff.Seek(point); int len = dbbuff.Readint(); if (len == 0) return Array.Empty<string>(); else {{ var arr = new string[len]; for (int i = 0; i < len; i++) {{ int index = dbbuff.Readint(); int pos = dbbuff.Position; arr[i] = __getstring(index); dbbuff.Seek(pos); }} return arr; }} }}
    internal static string[][] __getstringss(int point) {{ dbbuff.Seek(point); int len = dbbuff.Readint(); if (len == 0) return Array.Empty<string[]>(); else {{ var arr = new string[len][]; for (int i = 0; i < len; i++) {{ int len2 = dbbuff.Readint(); if (len2 == 0) arr[i] = Array.Empty<string>(); else {{ arr[i] = new string[len2]; for (int j = 0; j < len2; j++) {{ int index = dbbuff.Readint(); int pos = dbbuff.Position; arr[i][j] = __getstring(index); dbbuff.Seek(pos); }} }} }} return arr; }} }}");
            rw.AppendLine($"");
            for (int i = 0; i < cs.Count; i++)
            {
                var c = cs[i];
                if (c.isSingle) continue;
                rw.Append($"    static bool _init{c.name}Array; static {name}{c.name}[] _{c.name}Array; static Dictionary<{c.fs[0].typeStr}, TabMapping> _map{c.name};");
                for (int j = 0; j < c.groups.Count; j++)
                    rw.Append($" static Dictionary<{c.groups[j].typeStr}, List<{name}{c.name}>> {c.name}_{c.groups[j].name}GroupMap;");
                rw.AppendLine();
            }
            rw.AppendLine();
            rw.AppendLine($"    public static void Init(DBuffer buffer, bool isLoadAll)");
            rw.AppendLine($"    {{");
            rw.AppendLine($"        dbbuff = buffer; loadAll = isLoadAll;");
            rw.AppendLine($"        int len = buffer.Readint(); buffer.Readint(); stringCache = new string[len]; stringIndex = new int[len]; for (int i = 0; i < len; i++) {{ stringIndex[i] = buffer.Position; buffer.Seek(buffer.Readint() + buffer.Position); }}");
            rw.AppendLine($"        buffer.Readint();//data buff总长");
            for (int i = 0; i < cs.Count; i++)
            {
                if (cs[i].isSingle)
                    rw.AppendLine($"        {cs[i].name} = new(buffer, isLoadAll);");
            }
            for (int i = 0; i < cs.Count; i++)
            {
                var c = cs[i];
                if (c.isSingle) continue;
                rw.AppendLine($"        len = buffer.Readint(); _init{c.name}Array = false; _{c.name}Array = new {name}{c.name}[len]; _map{c.name} = new(len); for (int i = 0; i < len; i++) {{ int offset = buffer.Readint(); TabMapping map = new(buffer.Position, i); _map{c.name}.Add(buffer.Read{c.fs[0].typeStr}(), map); buffer.Seek(map.point + offset); }}");
            }
            rw.Append($"        if (loadAll) {{");
            for (int i = 0; i < cs.Count; i++)
            {
                var c = cs[i];
                if (c.isSingle) continue;
                rw.Append($" _ = {c.name}Array;");
            }
            rw.AppendLine($" }}");
            rw.AppendLine($"    }}");
            for (int i = 0; i < cs.Count; i++)
            {
                if (cs[i].isSingle)
                    rw.AppendLine($"    public static {name}{cs[i].name} {cs[i].name} {{ get; private set; }}");
            }
            for (int i = 0; i < cs.Count; i++)
            {
                var c = cs[i];
                if (c.isSingle) continue;
                rw.AppendLine();
                rw.AppendLine($"    public static {name}{c.name}[] {c.name}Array {{ get {{ if (!_init{c.name}Array) {{ _init{c.name}Array = true; foreach (var item in _map{c.name}.Keys) Get{c.name}(item); }} return _{c.name}Array; }} }}");
                rw.AppendLine($"    public static bool Has{c.name}({c.fs[0].typeStr} key) => _map{c.name}.ContainsKey(key);");
                rw.AppendLine($"    public static {name}{c.name} Get{c.name}({c.fs[0].typeStr} key) {{ if (_map{c.name}.TryGetValue(key, out var value)) {{ if (_{c.name}Array[value.index] == null) {{ dbbuff.Seek(value.point); _{c.name}Array[value.index] = new(dbbuff, loadAll); }} return _{c.name}Array[value.index]; }} Loger.Error(\"{name}{c.name}表没有key: \" + key); return null; }}");
                for (int j = 0; j < c.groups.Count; j++)
                    rw.AppendLine($"    public static List<{name}{c.name}> Get{c.name}_{c.groups[j].name}Group({c.groups[j].typeStr} group) {{ if ({c.name}_{c.groups[j].name}GroupMap == null) {{ {c.name}_{c.groups[j].name}GroupMap = new(); for (int i = 0; i < {c.name}Array.Length; i++) {{ var t = {c.name}Array[i]; if (!{c.name}_{c.groups[j].name}GroupMap.TryGetValue(t.{c.groups[j].name}, out var vs)) {c.name}_{c.groups[j].name}GroupMap[t.{c.groups[j].name}] = vs = new(); vs.Add(t); }} }} if (!{c.name}_{c.groups[j].name}GroupMap.TryGetValue(group, out var lst)) Loger.Error(\"{name}{c.name}表没有{c.groups[j].name}组: \" + group); return lst; }}");
            }

            rw.AppendLine($"}}");
            for (int i = 0; i < cs.Count; i++)
            {
                var c = cs[i];
                rw.AppendLine($"public class {name}{c.name}");
                rw.AppendLine($"{{");

                for (int j = 0; j < c.fs.Count; j++)
                {
                    var f = c.fs[j];
                    rw.AppendLine($"    /// <summary>");
                    rw.AppendLine($"    /// {f.des}");
                    rw.AppendLine($"    /// </summary>");
                    if (f.f2 == FType2.Array || f.f2 == FType2.Array2)
                        rw.AppendLine($"    public {f.typeStr} {f.name} => get{f.name}();");
                    else
                    {
                        if (f.f1 == FType.fstring)
                            rw.AppendLine($"    public {f.typeStr} {f.name} => {name}.__getstring(_{f.name}Idx);");
                        else
                            rw.AppendLine($"    public {f.typeStr} {f.name} {{ get; }}");
                    }
                }
                rw.AppendLine();

                rw.AppendLine($"    DBuffer dbuff;");

                for (int j = 0; j < c.fs.Count; j++)
                {
                    var f = c.fs[j];
                    if (f.f2 == FType2.Array || f.f2 == FType2.Array2)
                    {
                        if (f.f1 == FType.fstring)
                            rw.AppendLine($"    int _{f.name}Idx; {f.typeStr} _{f.name}Tmp; {f.typeStr} get{f.name}() => _{f.name}Tmp ??= {name}.__get{f.typeStr.Replace("[]", "s")}(_{f.name}Idx);");
                        else
                            rw.AppendLine($"    int _{f.name}Idx; {f.typeStr} _{f.name}Tmp; {f.typeStr} get{f.name}() {{ if (_{f.name}Tmp == null) {{ dbuff.Seek(_{f.name}Idx); _{f.name}Tmp = dbuff.Read{f.typeStr.Replace("[]", "s")}(); }} return _{f.name}Tmp; }}");
                    }
                    else if (f.f1 == FType.fstring)
                    {
                        rw.AppendLine($"    int _{f.name}Idx;");
                    }
                }
                rw.AppendLine($"");
                rw.AppendLine($"    public {name}{c.name}(DBuffer buffer, bool loadAll = false)");
                rw.AppendLine($"    {{");
                rw.AppendLine($"        dbuff = buffer;");
                for (int j = 0; j < c.fs.Count; j++)
                {
                    var f = c.fs[j];
                    if (f.f2 == FType2.Array || f.f2 == FType2.Array2)
                        rw.AppendLine($"        buffer.Seek(buffer.Readint() + (this._{f.name}Idx = buffer.Position));");
                    else if (f.f1 == FType.fstring)
                    {
                        rw.AppendLine($"        this._{f.name}Idx = buffer.Readint();");
                    }
                    else
                        rw.AppendLine($"        this.{f.name} = buffer.Read{f.typeStr}();");
                }
                rw.Append($"        if (loadAll) {{");
                for (int j = 0; j < c.fs.Count; j++)
                {
                    var f = c.fs[j];
                    if (f.f2 == FType2.Array || f.f2 == FType2.Array2 || f.f1 == FType.fstring)
                        rw.Append($" _ = {f.name};");
                }
                rw.AppendLine($" }}");
                rw.AppendLine($"    }}");

                rw.AppendLine($"}}");
            }


            DBuffer data = new DBuffer(10000000);
            DBuffer stringData = new DBuffer(10000000);
            Dictionary<string, int> stringIndex = new(10000);
            DBuffer tmpC = new DBuffer(new MemoryStream(new byte[100000], 0, 100000, true, true));
            DBuffer arrTmp = new DBuffer(new MemoryStream(new byte[10000], 0, 10000, true, true));
            cs.Sort((x, y) =>
            {
                if (y.isSingle) return 1;
                if (x.isSingle) return -1;
                return 0;
            });
            for (int i = 0; i < cs.Count; i++)
            {
                var c = cs[i];

                if (c.isSingle)
                {
                    if (c.fv.Count > 0)
                    {
                        var vs = c.fv[0];
                        for (int k = 0; k < vs.Length; k++)
                        {
                            var v = vs[k];
                            Common.WriteFv(c.fs[k], v, data, arrTmp, stringIndex, stringData);
                        }
                    }
                    continue;
                }
                data.Write(c.fv.Count);

                for (int j = 0; j < c.fv.Count; j++)
                {
                    tmpC.Seek(0);
                    var vs = c.fv[j];
                    for (int k = 0; k < vs.Length; k++)
                    {
                        var v = vs[k];
                        Common.WriteFv(c.fs[k], v, tmpC, arrTmp, stringIndex, stringData);
                    }
                    data.Write(tmpC);
                }
            }
            buffer.WriteHeaderInfo();
            buffer.Write(stringIndex.Count);
            buffer.Write(stringData);
            buffer.Write(data);

            //ecs
            if (genEcs)
            {
                erw.AppendLine($"");
                erw.AppendLine($"public readonly unsafe struct {name}_ST");
                erw.AppendLine($"{{");
                erw.AppendLine($"    public readonly static SharedStatic<{name}_ST> Tab = SharedStatic<{name}_ST>.GetOrCreate<SharedStatic<{name}_ST>>();");
                erw.AppendLine();
                for (int i = 0; i < cs.Count; i++)
                {
                    var c = cs[i];
                    if (c.isSingle) continue;
                    if (c.ecs.Count > 1)
                        erw.AppendLine($"    readonly UnsafeHashMap<{c.fs[0].typeStrECS}, int> {c.name}Map;");
                }
                erw.AppendLine($"    public static void Init(DBuffer buffer)");
                erw.AppendLine($"    {{");
                erw.AppendLine($"        Tab.Data.Dispose();");
                for (int i = 0; i < cs.Count; i++)
                {
                    var c = cs[i];
                    if (c.isSingle && c.ecs.Count > 0)
                        erw.AppendLine($"        fixed ({c.name}_ST* ptr = &Tab.Data.{c.name}) {{ *ptr = new {c.name}_ST(buffer); }}");
                }
                erw.AppendLine($"        int len;");
                for (int i = 0; i < cs.Count; i++)
                {
                    var c = cs[i];
                    if (c.isSingle) continue;
                    if (c.ecs.Count > 1)
                    {
                        erw.AppendLine($"        len = buffer.Readint(); fixed (UnsafeList<{c.name}_ST>* ptr = &Tab.Data.{c.name}Array) {{ *ptr = new UnsafeList<{c.name}_ST>(len, Allocator.Persistent); fixed (UnsafeHashMap<{c.fs[0].typeStrECS}, int>* ptr2 = &Tab.Data.{c.name}Map) {{ *ptr2 = new UnsafeHashMap<{c.fs[0].typeStrECS}, int>(len, AllocatorManager.Persistent); for (int i = 0; i < len; i++) {{ {c.name}_ST st = new(buffer); ptr->Add(st); ptr2->Add(st.{c.fs[0].name}, i); }} }} }}");
                    }
                }
                erw.AppendLine($"    }}");
                erw.AppendLine($"    public void Dispose()");
                erw.AppendLine($"    {{");
                for (int i = 0; i < cs.Count; i++)
                {
                    var c = cs[i];
                    if (c.isSingle)
                    {
                        for (int j = 0; j < c.ecs.Count; j++)
                        {
                            if (c.ecs[j].f2 == FType2.Array)
                                erw.AppendLine($"        {c.name}.{c.ecs[j].name}.Dispose();");
                        }
                        continue;
                    }
                    if (c.ecs.Count > 1)
                    {
                        erw.AppendLine($"        for (int i = 0; i < {c.name}Array.Length; i++)");
                        erw.AppendLine($"        {{");
                        for (int j = 0; j < c.ecs.Count; j++)
                        {
                            if (c.ecs[j].f2 == FType2.Array)
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
                for (int i = 0; i < cs.Count; i++)
                {
                    var c = cs[i];
                    if (c.isSingle && c.ecs.Count > 0)
                        erw.AppendLine($"    public readonly {c.name}_ST {c.name};");
                }
                for (int i = 0; i < cs.Count; i++)
                {
                    var c = cs[i];
                    if (c.isSingle) continue;
                    if (c.ecs.Count > 1)
                    {
                        erw.AppendLine($"");
                        erw.AppendLine($"    public readonly UnsafeList<{c.name}_ST> {c.name}Array;");
                        erw.AppendLine($"    public readonly bool Has{c.name}({c.fs[0].typeStrECS} key) => {c.name}Map.ContainsKey(key);");
                        erw.AppendLine($"    public readonly ref {c.name}_ST Get{c.name}({c.fs[0].typeStrECS} key) => ref {c.name}Array.ElementAt({c.name}Map[key]);");
                    }
                }
                erw.AppendLine($"}}");
                for (int i = 0; i < cs.Count; i++)
                {
                    var c = cs[i];
                    if (c.ecs.Count > 1)
                    {
                        erw.AppendLine($"public readonly struct {c.name}_ST");
                        erw.AppendLine($"{{");

                        for (int j = 0; j < c.ecs.Count; j++)
                        {
                            var f = c.ecs[j];
                            erw.AppendLine($@"    /// <summary>
    /// {f.des}
    /// </summary>");
                            if (f.f2 == FType2.Array)
                                erw.AppendLine($"    public readonly UnsafeList<{f.typeStrECS}> {f.name};");
                            else
                                erw.AppendLine($"    public readonly {f.typeStrECS} {f.name};");
                        }

                        erw.AppendLine($"    public {c.name}_ST(DBuffer buffer)");
                        erw.AppendLine($"    {{");
                        erw.AppendLine($"        int len;");
                        for (int j = 0; j < c.ecs.Count; j++)
                        {
                            var f = c.ecs[j];
                            if (f.f2 == FType2.Value)
                            {
                                erw.AppendLine($"        this.{f.name} = buffer.Read{f.realType}();");
                            }
                            else if (f.f2 == FType2.Array)
                            {
                                erw.Append($"        len = buffer.Readint();");
                                erw.Append($" this.{f.name} = new UnsafeList<{f.typeStrECS}>(len, AllocatorManager.Persistent);");
                                erw.AppendLine($" for (int i = 0; i < len; i++) this.{f.name}.Add(buffer.Read{f.realType}());");
                            }
                        }
                        erw.AppendLine($"    }}");

                        erw.AppendLine($"}}");
                    }
                }

                bufferEcs.WriteHeaderInfo();
                for (int i = 0; i < cs.Count; i++)
                {
                    var c = cs[i];
                    if (c.isSingle)
                    {
                        if (c.fv.Count > 0)
                        {
                            var vs = c.fv[0];
                            for (int k = 0; k < c.ecs.Count; k++)
                            {
                                var f = c.ecs[k];
                                var v = vs[f.arrayIndex];
                                Common.WriteFv(f, v, bufferEcs, null);
                            }
                        }
                        continue;
                    }
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
            }

            if (genEcs)
            {
                File.WriteAllBytes(dataPath + name + ".bytes", buffer.ToBytes());
                File.WriteAllBytes(dataPath2 + name + ".bytes", buffer.ToBytes());
                File.WriteAllBytes(dataPath + name + "_ST.bytes", bufferEcs.ToBytes());

                File.WriteAllText(codePath + name + ".cs", rw.ToString());
                File.WriteAllText(codePath + name + "_ST.cs", erw.ToString());
            }
            else
            {
                File.WriteAllBytes(dataPath + name + ".bytes", buffer.ToBytes());
                File.WriteAllBytes(dataPath2 + name + ".bytes", buffer.ToBytes());
                File.WriteAllText(codePath + name + ".cs", rw.ToString());
            }

            Console.WriteLine("写入完成");
            Console.WriteLine("");
        }
    }
}
