using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

internal class CodeGen
{
    public static void Gen(PBParserResult ret, string path)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        foreach (var item in Directory.GetFiles(path, "*.cs"))
            File.Delete(item);

        StringBuilder df = new StringBuilder(10000);
        StringBuilder rw = new StringBuilder(10000);

        rw.AppendLine("using System.Collections.Generic;");
        rw.AppendLine("using Game;");
        rw.AppendLine("using Unity.Mathematics;");
        for (int i = 0; i < ret.pbs.Count; i++)
        {
            var pb = ret.pbs[i];
            for (int j = 0; j < pb.refrences.Count; j++)
                rw.AppendLine($"using {pb.refrences[i]};");
        }
        rw.AppendLine();

        for (int i = 0; i < ret.pbs.Count; i++)
        {
            var pb = ret.pbs[i];

            df.Clear();
            df.AppendLine("using System.Collections.Generic;");
            df.AppendLine("using Game;");
            df.AppendLine("using Unity.Mathematics;");
            for (int j = 0; j < pb.refrences.Count; j++)
                df.AppendLine($"using {pb.refrences[i]};");
            df.AppendLine();
            df.AppendLine($"namespace {pb.name}");
            df.AppendLine("{");

            rw.AppendLine($"namespace {pb.name}");
            rw.AppendLine("{");

            for (int j = 0; j < pb.classes.Count; j++)
            {
                var c = pb.classes[j];
                if (c.classType == PBClassType.v_messsage)
                {
                    if (!string.IsNullOrEmpty(c.summary))
                    {
                        df.AppendLine($"    /// <summary>");
                        df.AppendLine($"    /// " + c.summary);
                        df.AppendLine($"    /// </summary>");
                    }
                    if (c.Response == null)
                        df.AppendLine($"    [Message({c.cmd})]");
                    else
                        df.AppendLine($"    [Message({c.cmd}, typeof({c.Response.fullName}))]");

                    df.AppendLine($"    public partial class {c.name}");
                    df.AppendLine("    {");
                    rw.AppendLine($"    public partial class {c.name} : PB.PBMessage");
                    rw.AppendLine("    {");

                    int optionalCount = c.fields.Count(t => t.rule == fieldRule.optional);
                    if (optionalCount > 0)
                    {
                        int v = (optionalCount - 1) / 32 + 1;
                        for (int k = 0; k < v; k++)
                            rw.AppendLine($"        int mask{k};");
                    }
                    for (int k = 0; k < c.fields.Count; k++)
                    {
                        var field = c.fields[k];
                        if (field.rule == fieldRule.optional)
                        {
                            int index = field.ruleIndex;
                            int v = index / 32;
                            int b = index % 32;
                            if (field.elementType == elementType.f_string)
                                rw.AppendLine($"        {field.GetFieldTypeString()} _{field.name} = string.Empty;");
                            else if (field.elementType == elementType.f_bytes)
                                rw.AppendLine($"        {field.GetFieldTypeString()} _{field.name};");
                            else
                                rw.AppendLine($"        {field.GetFieldTypeString()} _{field.name} = new();");
                            rw.AppendLine($"        public bool Has_{field.name} => (mask{v} & (1 << {b})) != 0;");
                            rw.AppendLine($"        void Set_{field.name}() => mask{v} |= 1 << {b};");
                            rw.AppendLine($"        public void Clear_{field.name}() => mask{v} &= ~(1 << {b});");
                        }
                    }
                    rw.AppendLine($"        public override void Write(PB.PBWriter writer)");
                    rw.AppendLine("        {");
                    for (int k = 0; k < c.fields.Count; k++)
                    {
                        var field = c.fields[k];
                        switch (field.fieldType)
                        {
                            case fieldType.value:
                                {
                                    if (field.rule == fieldRule.optional)
                                    {
                                        rw.AppendLine($"            if (Has_{field.name}) writer.Write{field.GetApiString()}({field.GetFinalTag()}, this._{field.name});");
                                    }
                                    else
                                        rw.AppendLine($"            writer.Write{field.GetApiString()}({field.GetFinalTag()}, this.{field.name});");
                                }
                                break;
                            case fieldType.list:
                                rw.AppendLine($"            writer.Write{field.GetApiString()}s({field.GetFinalTag()}, this.{field.name});");
                                break;
                            case fieldType.map:
                                {
                                    int ktag = field.GetFinalTag(1, field.elementType);
                                    int vtag = field.GetFinalTag(2, field.elementType2);

                                    rw.AppendLine($"            if (this.{field.name} != null)");
                                    rw.AppendLine("            {");
                                    rw.AppendLine($"                PB.PBWriter tmp = PB.PBBuffPool.Get();");
                                    rw.AppendLine($"                foreach (var item in this.{field.name})");
                                    rw.AppendLine($"                {{");
                                    rw.AppendLine($"                    tmp.Seek(0);");
                                    rw.AppendLine($"                    tmp.Write{field.GetApiString()}({ktag}, item.Key);");
                                    rw.AppendLine($"                    tmp.Write{field.GetApiString2()}({vtag}, item.Value);");
                                    rw.AppendLine($"                    writer.WriteBuff({field.GetFinalTag()}, tmp);");
                                    rw.AppendLine($"                }}");
                                    rw.AppendLine($"                PB.PBBuffPool.Return(tmp);");
                                    rw.AppendLine("            }");

                                }
                                break;
                            case fieldType.message:
                                {
                                    if (field.rule == fieldRule.optional)
                                    {
                                        rw.AppendLine($"            if(Has_{field.name})");
                                        rw.AppendLine($"                writer.Writemessage({field.GetFinalTag()}, this._{field.name});");
                                    }
                                    else
                                        rw.AppendLine($"            writer.Writemessage({field.GetFinalTag()}, this.{field.name});");
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    rw.AppendLine("        }");
                    rw.AppendLine($"        public override void Read(PB.PBReader reader)");
                    rw.AppendLine("        {");
                    rw.AppendLine("            int tag;");
                    rw.AppendLine("            while ((tag = reader.ReadTag()) != 0)");
                    rw.AppendLine("            {");
                    rw.AppendLine("                switch (tag)");
                    rw.AppendLine("                {");
                    for (int k = 0; k < c.fields.Count; k++)
                    {
                        var field = c.fields[k];
                        switch (field.fieldType)
                        {
                            case fieldType.value:
                                if (field.elementType == elementType.f_enum)
                                    rw.AppendLine($"                    case {field.GetFinalTag()}: this.{field.name} = ({field.rawType})reader.Readint32(); break;");
                                else
                                    rw.AppendLine($"                    case {field.GetFinalTag()}: this.{field.name} = reader.Read{field.GetApiString()}(); break;");
                                break;
                            case fieldType.list:
                                {
                                    if (field.elementType == elementType.f_string || field.elementType == elementType.f_bytes || (field.elementType > elementType.f_vector_x_begin && field.elementType < elementType.f_vector_x_end))
                                        rw.AppendLine($"                    case {field.GetFinalTag()}: {field.name}.Add(reader.Read{field.GetApiString()}()); break;");
                                    else if (field.elementType == elementType.f_message)
                                        rw.AppendLine($"                    case {field.GetFinalTag()}: {field.name}.Add(reader.Read{field.GetApiString()}(new {field.rawType}())); break;");
                                    else
                                        rw.AppendLine($"                    case {field.GetFinalTag()}: reader.Read{field.GetApiString()}s(this.{field.name}); break;");
                                }
                                break;
                            case fieldType.map:
                                {
                                    int ktag = field.GetFinalTag(1, field.elementType);
                                    int vtag = field.GetFinalTag(2, field.elementType2);

                                    rw.AppendLine($"                    case {field.GetFinalTag()}:");
                                    rw.AppendLine($"                        {{");

                                    rw.AppendLine($"                            int size = reader.Readint32();");
                                    rw.AppendLine($"                            int max = reader.max;");
                                    rw.AppendLine($"                            int tag2;");
                                    if (field.elementCodeType == elementCodeType.v_string)
                                        rw.AppendLine($"                            {field.GetElementTypeString()} k = string.Empty;");
                                    else
                                        rw.AppendLine($"                            {field.GetElementTypeString()} k = new();");

                                    if (field.elementCodeType2 == elementCodeType.v_string)
                                        rw.AppendLine($"                            {field.GetElementType2String()} v = string.Empty;");
                                    else if (field.elementType2 == elementType.f_bytes)
                                        rw.AppendLine($"                            {field.GetElementType2String()} v = default;");
                                    else 
                                        rw.AppendLine($"                            {field.GetElementType2String()} v = new();");

                                    rw.AppendLine($"                            reader.SetMax(reader.Position + size);");
                                    rw.AppendLine($"                            while ((tag2 = reader.ReadTag()) != 0)");
                                    rw.AppendLine($"                            {{");

                                    if (field.elementType == elementType.f_enum)
                                        rw.AppendLine($"                                if (tag2 == {ktag}) k = ({field.rawType})reader.Readint32();");
                                    else
                                        rw.AppendLine($"                                if (tag2 == {ktag}) k = reader.Read{field.GetApiString()}();");

                                    if (field.elementType2 == elementType.f_enum)
                                        rw.AppendLine($"                                else if (tag2 == {vtag}) v = ({field.rawType2})reader.Readint32();");
                                    else if (field.elementCodeType2 == elementCodeType.v_pbType)
                                        rw.AppendLine($"                                else if (tag2 == {vtag}) v = reader.Read{field.GetApiString2()}(v);");
                                    else
                                        rw.AppendLine($"                                else if (tag2 == {vtag}) v = reader.Read{field.GetApiString2()}();");
                                    rw.AppendLine($"                            }}");
                                    rw.AppendLine($"                            this.{field.name}[k] = v;");
                                    rw.AppendLine($"                            reader.SeekLast();");
                                    rw.AppendLine($"                            reader.SetMax(max);");
                                    rw.AppendLine($"                        }}");
                                    rw.AppendLine($"                        break;");
                                }
                                break;
                            case fieldType.message:
                                rw.AppendLine($"                    case {field.GetFinalTag()}: this.{field.name} = reader.Readmessage(this.{field.name}); break;");
                                break;
                            default:
                                break;
                        }
                    }
                    rw.AppendLine("                    default: reader.SeekNext(tag); break;");
                    rw.AppendLine("                }");
                    rw.AppendLine("            }");
                    rw.AppendLine("        }");
                    rw.AppendLine($"        public override void Merge(PB.PBMessage message)");
                    rw.AppendLine("        {");
                    rw.AppendLine($"            var tmp = ({c.name})message;");
                    for (int k = 0; k < c.fields.Count; k++)
                    {
                        var field = c.fields[k];
                        if (field.rule == fieldRule.optional)
                            rw.AppendLine($"            if (tmp.Has_{field.name}) this.{field.name} = tmp.{field.name};");
                        else
                            rw.AppendLine($"            this.{field.name} = tmp.{field.name};");
                    }
                    rw.AppendLine("        }");

                    for (int k = 0; k < c.fields.Count; k++)
                    {
                        var field = c.fields[k];
                        if (!string.IsNullOrEmpty(field.summary))
                        {
                            df.AppendLine($"        /// <summary>");
                            df.AppendLine($"        /// " + field.summary);
                            df.AppendLine($"        /// </summary>");
                        }
                        if (field.rule == fieldRule.optional)
                            df.AppendLine($"        public {field.GetFieldTypeString()} {field.name} {{ get => _{field.name}; set {{ _{field.name} = value; Set_{field.name}(); }} }}");
                        else if (field.fieldType == fieldType.value)
                            df.AppendLine($"        public {field.GetFieldTypeString()} {field.name};");
                        else
                            df.AppendLine($"        public {field.GetFieldTypeString()} {field.name} = new();");
                    }

                    df.AppendLine("    }");
                    rw.AppendLine("    }");
                }
                else if (c.classType == PBClassType.v_enum)
                {
                    if (!string.IsNullOrEmpty(c.summary))
                    {
                        df.AppendLine($"    /// <summary>");
                        df.AppendLine($"    /// " + c.summary);
                        df.AppendLine($"    /// </summary>");
                    }
                    df.AppendLine($"    public enum {c.name}");
                    df.AppendLine("    {");
                    for (int k = 0; k < c.fields.Count; k++)
                    {
                        var field = c.fields[k];
                        if (!string.IsNullOrEmpty(field.summary))
                        {
                            df.AppendLine($"        /// <summary>");
                            df.AppendLine($"        /// " + field.summary);
                            df.AppendLine($"        /// </summary>");
                        }
                        df.AppendLine($"        {field.name} = {field.tag},");
                    }
                    df.AppendLine("    }");
                }
            }

            df.AppendLine("}");
            File.WriteAllText(path + $"/{pb.name}.cs", df.ToString());

            rw.AppendLine("}");
        }
        File.WriteAllText(path + $"/RW.cs", rw.ToString());
    }
}
