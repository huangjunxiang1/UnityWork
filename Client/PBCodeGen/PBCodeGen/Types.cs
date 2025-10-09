using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

public class PBParserResult
{
    public List<PBType> pbs = new();
    public Dictionary<string, PBClass> classMap = new();
}
public class PBType
{
    public string name;

    public List<string> refrences = new();
    public List<PBClass> classes = new();
}
public class PBClass
{
    public PBType parent;
    public string name;
    public string fullName => $"{parent.name}.{name}";
    public PBClassType classType;
    public string summary;
    public string cmd;
    public PBClass Response;//

    public List<FieldObject> fields = new();
}
public class FieldObject
{
    public PBClass parent;
    public string name;
    public string summary;
    public int tag;//if is enum   it is enum value

    public fieldRule rule;
    public int ruleIndex;//if it is optional  it is index in mask
    public fieldType fieldType;

    //if it is map    this is key type 
    public elementType elementType;
    public elementCodeType elementCodeType;
    public string rawType;

    //if it is map    this is value type
    public elementType elementType2;
    public string rawType2;
    public elementCodeType elementCodeType2;

    public string GetApiString()
    {
        if (elementType == elementType.f_enum) return "enum";
        if (elementType == elementType.f_message) return "message";
        return elementType.ToString()[2..];
    }
    public string GetApiString2()
    {
        if (elementType2 == elementType.f_enum) return "enum";
        if (elementType2 == elementType.f_message) return "message";
        return elementType2.ToString()[2..];
    }
    public int GetFinalTag()
    {
        switch (fieldType)
        {
            case fieldType.value:
                return GetFinalTag(tag, elementType);
            case fieldType.list:
                return (tag << 3) | 2;
            case fieldType.map:
                return (tag << 3) | 2;
            case fieldType.message:
                return (tag << 3) | 2;
            default:
                throw new Exception("not support ");
        }
    }
    public int GetFinalTag(int tag, elementType e)
    {
        switch (e)
        {
            case elementType.f_bool:
            case elementType.f_byte:
            case elementType.f_int32:
            case elementType.f_uint32:
            case elementType.f_sint32:
            case elementType.f_int64:
            case elementType.f_uint64:
            case elementType.f_sint64:
            case elementType.f_enum:
                return (tag << 3) | 0;

            case elementType.f_float:
            case elementType.f_fixed32:
                return (tag << 3) | 5;

            case elementType.f_fixed64:
            case elementType.f_double:
                return (tag << 3) | 1;

            default:
                return (tag << 3) | 2;
        }
    }
    public string GetElementTypeString()
    {
        if (elementType == elementType.f_message || elementType == elementType.f_enum)
            return rawType;
        if (elementType == elementType.f_bytes)
            return "byte[]";
        return elementCodeType.ToString()[2..];
    }
    public string GetElementType2String()
    {
        if (elementType2 == elementType.f_message || elementType2 == elementType.f_enum)
            return rawType2;
        if (elementType2 == elementType.f_bytes)
            return "byte[]";
        return elementCodeType2.ToString()[2..];
    }
    public string GetFieldTypeString()
    {
        string s = GetElementTypeString();
        string s2 = GetElementType2String();

        return fieldType switch
        {
            fieldType.value => s,
            fieldType.list => $"List<{s}>",
            fieldType.map => $"Dictionary<{s}, {s2}>",
            fieldType.message => $"{s}",
            _ => throw new Exception("not support"),
        };
    }
    public static elementType GetElementType(string type, Dictionary<string, PBClass> classMap)
    {
        if (classMap.TryGetValue(type, out var pb))
        {
            switch (pb.classType)
            {
                case PBClassType.v_messsage:
                    return elementType.f_message;
                case PBClassType.v_enum:
                    return elementType.f_enum;
                default:
                    break;
            }
        }
        return type switch
        {
            "bool" => elementType.f_bool,
            "byte" => elementType.f_byte,

            "int32" => elementType.f_int32,
            "sint32" => elementType.f_sint32,
            "fixed32" => elementType.f_fixed32,

            "uint32" => elementType.f_uint32,

            "int64" => elementType.f_int64,
            "sint64" => elementType.f_sint64,
            "fixed64" => elementType.f_fixed64,

            "uint64" => elementType.f_uint64,

            "float" => elementType.f_float,

            "double" => elementType.f_double,

            "string" => elementType.f_string,
            "bytes" => elementType.f_bytes,

            "int2" => elementType.f_int2,
            "int3" => elementType.f_int3,
            "int4" => elementType.f_int4,
            "float2" => elementType.f_float2,
            "float3" => elementType.f_float3,
            "float4" => elementType.f_float4,

            _ => throw new Exception($"not support type {type}"),
        };
    }
    public elementCodeType GetElementCodeType(elementType type)
    {
        return type switch
        {
            elementType.f_bool => elementCodeType.v_bool,
            elementType.f_byte => elementCodeType.v_byte,
            elementType.f_int32 => elementCodeType.v_int,
            elementType.f_sint32 => elementCodeType.v_int,
            elementType.f_fixed32 => elementCodeType.v_int,
            elementType.f_uint32 => elementCodeType.v_uint,
            elementType.f_int64 => elementCodeType.v_long,
            elementType.f_sint64 => elementCodeType.v_long,
            elementType.f_fixed64 => elementCodeType.v_long,
            elementType.f_uint64 => elementCodeType.v_ulong,
            elementType.f_float => elementCodeType.v_float,
            elementType.f_double => elementCodeType.v_double,
            elementType.f_string => elementCodeType.v_string,
            elementType.f_bytes => elementCodeType.v_bytes,
            elementType.f_int2 => elementCodeType.v_int2,
            elementType.f_int3 => elementCodeType.v_int3,
            elementType.f_int4 => elementCodeType.v_int4,
            elementType.f_float2 => elementCodeType.v_float2,
            elementType.f_float3 => elementCodeType.v_float3,
            elementType.f_float4 => elementCodeType.v_float4,
            elementType.f_message => elementCodeType.v_pbType,
            elementType.f_enum => elementCodeType.v_pbType,
            _ => throw new Exception($"not support type {type}"),
        };
    }
    public void ParseSplitStrings(string[] splits, Dictionary<string, PBClass> classMap)
    {
        name = splits[^2];
        tag = int.Parse(splits[^1]);

        if (splits[0] == "optional")
        {
            rule = fieldRule.optional;
            ruleIndex = parent.fields.Count(t => t.rule == fieldRule.optional);
        }

        if (splits[0] == "repeated")
            fieldType = fieldType.list;
        else if (splits[0] == "map")
            fieldType = fieldType.map;
        else
        {
            if (rule == fieldRule.optional)
            {
                if (classMap.TryGetValue(splits[1], out var pb))
                    fieldType = pb.classType == PBClassType.v_messsage ? fieldType.message : fieldType.value;
                else
                    fieldType = fieldType.value;
            }
            else
            {
                if (classMap.TryGetValue(splits[0], out var pb))
                    fieldType = pb.classType == PBClassType.v_messsage ? fieldType.message : fieldType.value;
                else
                    fieldType = fieldType.value;
            }
        }

        if (rule == fieldRule.optional && fieldType != fieldType.value && fieldType != fieldType.message)
        {
            throw new Exception($"error optional only support value or message {this.parent.name} {string.Join(" ", splits)}");
        }

        switch (fieldType)
        {
            case fieldType.value:
                rawType = rule == fieldRule.optional ? splits[1] : splits[0];
                elementType = GetElementType(rawType, classMap);
                elementCodeType = GetElementCodeType(elementType);
                break;
            case fieldType.list:
                rawType = splits[1];
                elementType = GetElementType(rawType, classMap);
                elementCodeType = GetElementCodeType(elementType);
                break;
            case fieldType.map:
                rawType = splits[1];
                elementType = GetElementType(rawType, classMap);
                elementCodeType = GetElementCodeType(elementType);
                rawType2 = splits[2];
                elementType2 = GetElementType(rawType2, classMap);
                elementCodeType2 = GetElementCodeType(elementType2);
                break;
            case fieldType.message:
                rawType = rule == fieldRule.optional ? splits[1] : splits[0];
                elementType = GetElementType(rawType, classMap);
                elementCodeType = GetElementCodeType(elementType);
                break;
            default:
                break;
        }

        if (fieldType == fieldType.map)
        {
            if (elementType > elementType.f_key_end)
            {
                throw new Exception($"error map key cannot support {elementType} {this.parent.name} {string.Join(" ", splits)}");
            }
        }
    }
}

public enum PBClassType
{
    v_messsage,
    v_enum,
}
public enum elementType
{
    f_bool,
    f_byte,
    f_int32,
    f_uint32,
    f_int64,
    f_uint64,

    f_fixed32,
    f_fixed64,
    f_sint32,
    f_sint64,

    f_float,
    f_double,
    f_string,
    f_enum,

    f_vector_x_begin,
    f_int2,
    f_int3,
    f_int4,
    f_float2,
    f_float3,
    f_float4,
    f_vector_x_end,

    f_key_end,

    f_bytes,
    f_message,
}
public enum elementCodeType
{
    v_bool,
    v_byte,
    v_int,
    v_long,
    v_uint,
    v_ulong,

    v_float,
    v_double,
    v_string,
    v_int2,
    v_int3,
    v_int4,
    v_float2,
    v_float3,
    v_float4,
    v_bytes,
    v_pbType,
}
public enum fieldRule
{
    none,
    optional,
}
public enum fieldType
{
    value,
    list,
    map,
    message,
}
