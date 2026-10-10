using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 扫描项目中所有 Shader 与 HLSL/CGINC 文件，生成包含缓存 PropertyID 的常量类。
/// - Shader 与 HLSL 统一输出到 ShaderProperties.cs，类名 ShaderProperties
/// 统一规则：
///   - 属性（Property / HLSL 全局变量） 前缀 PT_
///   - 关键字（Keyword）                前缀 KW_
/// 过滤掉带 [HideInInspector] 的属性（Shader Graph 自动生成的内部属性）。
/// </summary>
public static class ShaderPropertyGenerator
{
    private const string OutputPath = "Assets/Code/HotFix/_Gen/ShaderProperties.cs";
    private const string TopClassName = "ShaderProperties";
    private const string MenuPath = "Tools/Shader Property Generator";

    private const string PropPrefix = "PT_";
    private const string KeywordPrefix = "KW_";

    /// <summary>HLSL 是否只处理 Assets/ 下的。</summary>
    private const bool OnlyProjectHlsl = true;

    // HLSL 全局变量声明匹配
    private static readonly Regex HlslGlobalRegex = new Regex(
        @"^\s*(?:uniform\s+)?(?<type>[A-Za-z_][A-Za-z0-9_]*)(?:\s*<[^>]+>)?\s+(?<name>[A-Za-z_][A-Za-z0-9_]*)\s*(?:\[[^\]]*\])?\s*(?:=[^;]*)?;\s*$",
        RegexOptions.Compiled);

    // #pragma multi_compile / shader_feature 解析
    private static readonly Regex PragmaKeywordRegex = new Regex(
        @"#\s*pragma\s+(?:multi_compile|shader_feature)(?:_local)?(?:_fragment|_vertex)?\s+(?<keywords>[^\r\n]+)",
        RegexOptions.Compiled);

    [MenuItem(MenuPath)]
    public static void Generate()
    {
        var sb = CreateHeader(TopClassName);

        var usedClasses = new HashSet<string>();
        int classCount = 0;
        int fieldCount = 0;

        // ================================================================
        //  1. 扫描所有 Shader
        // ================================================================
        var shaderGuids = AssetDatabase.FindAssets("t:Shader");
        var shaders = new List<Shader>();
        foreach (var guid in shaderGuids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var shader = AssetDatabase.LoadAssetAtPath<Shader>(path);
            if (shader != null) shaders.Add(shader);
        }
        shaders.Sort((a, b) => string.CompareOrdinal(a.name, b.name));

        foreach (var shader in shaders)
        {
            if (string.IsNullOrEmpty(shader.name)) continue;
            if (shader.name.StartsWith("Hidden/")) continue;

            string shaderPath = AssetDatabase.GetAssetPath(shader);
            if (string.IsNullOrEmpty(shaderPath)) continue;

            // ---- Properties（过滤 HideInInspector） ----
            var properties = new List<(string type, string rawName)>();
            int propCount = shader.GetPropertyCount();
            for (int i = 0; i < propCount; i++)
            {
                string propName = shader.GetPropertyName(i);
                if (string.IsNullOrEmpty(propName)) continue;
                if (IsHiddenProperty(shader, i)) continue;
                properties.Add((shader.GetPropertyType(i).ToString(), propName));
            }

            // ---- Keywords（keywordSpace 优先，pragma 兜底） ----
            var keywords = GetShaderLocalKeywords(shader, shaderPath);

            if (properties.Count == 0 && keywords.Count == 0) continue;

            string className = SanitizeClassName(shader.name);
            if (string.IsNullOrEmpty(className)) continue;
            if (!usedClasses.Add(className)) continue;

            sb.AppendLine($"    /// <summary>Shader: {shader.name}</summary>");
            sb.AppendLine($"    public static class {className}");
            sb.AppendLine("    {");

            var seenFieldNames = new HashSet<string>();

            // ---- Properties：PT_xxx ----
            foreach (var (type, rawName) in properties)
            {
                string fieldName = MakeFieldName(PropPrefix, rawName);
                if (string.IsNullOrEmpty(fieldName)) continue;
                if (!seenFieldNames.Add(fieldName)) continue;

                sb.AppendLine($"        /// <summary>{type}  ({rawName})</summary>");
                sb.AppendLine($"        public static readonly int {fieldName} = Shader.PropertyToID(\"{rawName}\");");
                fieldCount++;
            }

            // ---- Keywords：KW_xxx ----
            foreach (var kw in keywords)
            {
                string fieldName = MakeFieldName(KeywordPrefix, kw);
                if (string.IsNullOrEmpty(fieldName)) continue;
                if (!seenFieldNames.Add(fieldName)) continue;

                sb.AppendLine($"        /// <summary>Keyword  ({kw})</summary>");
                sb.AppendLine($"        public const string {fieldName} = \"{kw}\";");
                fieldCount++;
            }

            sb.AppendLine("    }");
            sb.AppendLine();

            classCount++;
        }

        // ================================================================
        //  2. 扫描 HLSL/CGINC 文件
        // ================================================================
        var hlslGuids = AssetDatabase.FindAssets("t:ShaderInclude");
        var hlslPaths = new List<string>();
        foreach (var guid in hlslGuids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            if (string.IsNullOrEmpty(path)) continue;
            if (!path.EndsWith(".hlsl") && !path.EndsWith(".cginc")) continue;
            if (OnlyProjectHlsl && !path.StartsWith("Assets/")) continue;
            hlslPaths.Add(path);
        }
        hlslPaths.Sort(string.CompareOrdinal);

        foreach (var hlslPath in hlslPaths)
        {
            string content;
            try { content = File.ReadAllText(Path.GetFullPath(hlslPath)); }
            catch { continue; }

            // ---- HLSL 全局变量 ----
            var globals = ParseHlslGlobals(content);

            // ---- HLSL Keywords ----
            var hlslKeywords = ParseHlslKeywords(content);

            if (globals.Count == 0 && hlslKeywords.Count == 0) continue;

            string fileName = Path.GetFileNameWithoutExtension(hlslPath);
            string className = SanitizeClassName(fileName);
            if (string.IsNullOrEmpty(className)) continue;
            if (!usedClasses.Add(className)) continue;

            sb.AppendLine($"    /// <summary>HLSL: {hlslPath}</summary>");
            sb.AppendLine($"    public static class {className}");
            sb.AppendLine("    {");

            var seenFieldNames = new HashSet<string>();

            // ---- HLSL 全局变量：PT_xxx ----
            foreach (var (type, rawName) in globals)
            {
                string fieldName = MakeFieldName(PropPrefix, rawName);
                if (string.IsNullOrEmpty(fieldName)) continue;
                if (!seenFieldNames.Add(fieldName)) continue;

                sb.AppendLine($"        /// <summary>{type}  ({rawName})</summary>");
                sb.AppendLine($"        public static readonly int {fieldName} = Shader.PropertyToID(\"{rawName}\");");
                fieldCount++;
            }

            // ---- HLSL Keywords：KW_xxx ----
            foreach (var kw in hlslKeywords)
            {
                string fieldName = MakeFieldName(KeywordPrefix, kw);
                if (string.IsNullOrEmpty(fieldName)) continue;
                if (!seenFieldNames.Add(fieldName)) continue;

                sb.AppendLine($"        /// <summary>Keyword  ({kw})</summary>");
                sb.AppendLine($"        public const string {fieldName} = \"{kw}\";");
                fieldCount++;
            }

            sb.AppendLine("    }");
            sb.AppendLine();

            classCount++;
        }

        // ================================================================
        //  收尾并写入
        // ================================================================
        sb.AppendLine("}");

        WriteFile(OutputPath, sb.ToString());

        AssetDatabase.Refresh();
        Debug.Log($"[ShaderPropertyGenerator] 生成完成: {classCount} 个类 / {fieldCount} 个字段 -> {OutputPath}");
    }

    // ================================================================
    //                          辅助方法
    // ================================================================

    /// <summary>
    /// 生成带前缀的字段名。去掉原始名称开头的下划线，避免 PT__Color。
    /// 例：_Color -> PT_Color；_EMISSION -> KW_EMISSION。
    /// </summary>
    private static string MakeFieldName(string prefix, string rawName)
    {
        if (string.IsNullOrEmpty(rawName)) return null;

        string trimmed = rawName.TrimStart('_');
        if (string.IsNullOrEmpty(trimmed)) trimmed = rawName;

        string result = prefix + trimmed;

        if (char.IsDigit(result[0])) result = "_" + result;
        return result;
    }

    /// <summary>
    /// 判断属性是否带有 [HideInInspector]。
    /// Shader Graph 自动生成的内部属性都会带这个特性，用户 Blackboard 上 Exposed 的不会。
    /// </summary>
    private static bool IsHiddenProperty(Shader shader, int index)
    {
        try
        {
            var attrs = shader.GetPropertyAttributes(index);
            if (attrs == null) return false;
            foreach (var a in attrs)
            {
                if (a == "HideInInspector") return true;
            }
        }
        catch { /* 老版本 Unity 或 API 不可用时不过滤 */ }
        return false;
    }

    private static StringBuilder CreateHeader(string topClassName)
    {
        var sb = new StringBuilder();
        sb.AppendLine("// <auto-generated>");
        sb.AppendLine("//     此文件由 ShaderPropertyGenerator 自动生成，请勿手动修改。");
        sb.AppendLine($"//     重新生成: {MenuPath}");
        sb.AppendLine("// </auto-generated>");
        sb.AppendLine();
        sb.AppendLine("using UnityEngine;");
        sb.AppendLine();
        sb.AppendLine($"public static class {topClassName}");
        sb.AppendLine("{");
        return sb;
    }

    private static void WriteFile(string path, string content)
    {
        var dir = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        if (File.Exists(path) && File.ReadAllText(path) == content)
            return;

        File.WriteAllText(path, content, new UTF8Encoding(false));
    }

    /// <summary>获取 Shader 的所有 Local Keyword（keywordSpace 优先，pragma 兜底）。</summary>
    private static List<string> GetShaderLocalKeywords(Shader shader, string shaderPath)
    {
        var result = new HashSet<string>();

        // ---- 优先：keywordSpace API ----
        try
        {
            var space = shader.keywordSpace;
            var kws = space.keywords;
            if (kws != null)
            {
                foreach (var kw in kws)
                {
                    string name = kw.name;
                    if (!string.IsNullOrEmpty(name))
                        result.Add(name);
                }
            }
        }
        catch { /* keywordSpace 不可用时忽略 */ }

        // ---- 回退：解析 #pragma ----
        if (result.Count == 0 && !string.IsNullOrEmpty(shaderPath))
        {
            string fullPath = Path.GetFullPath(shaderPath);
            if (File.Exists(fullPath))
            {
                string source;
                try { source = File.ReadAllText(fullPath); }
                catch { source = null; }

                if (!string.IsNullOrEmpty(source))
                {
                    foreach (Match m in PragmaKeywordRegex.Matches(source))
                    {
                        var tokens = m.Groups["keywords"].Value
                            .Split(new[] { ' ', '\t', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

                        foreach (var token in tokens)
                        {
                            if (token == "_") continue;
                            if (token.Length == 0) continue;
                            result.Add(token);
                        }
                    }
                }
            }
        }

        var list = new List<string>(result);
        list.Sort(System.StringComparer.Ordinal);
        return list;
    }

    /// <summary>从 HLSL 内容中解析 #pragma multi_compile / shader_feature 定义的 keyword。</summary>
    private static List<string> ParseHlslKeywords(string content)
    {
        var result = new HashSet<string>();

        if (string.IsNullOrEmpty(content)) return new List<string>();

        foreach (Match m in PragmaKeywordRegex.Matches(content))
        {
            var tokens = m.Groups["keywords"].Value
                .Split(new[] { ' ', '\t', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

            foreach (var token in tokens)
            {
                if (token == "_") continue;
                if (token.Length == 0) continue;
                result.Add(token);
            }
        }

        var list = new List<string>(result);
        list.Sort(System.StringComparer.Ordinal);
        return list;
    }

    /// <summary>提取 HLSL 文件中处于大括号深度 0 的全局 uniform 变量。</summary>
    private static List<(string type, string name)> ParseHlslGlobals(string content)
    {
        var result = new List<(string, string)>();
        int depth = 0;
        bool inBlockComment = false;

        foreach (var rawLine in content.Split('\n'))
        {
            var line = rawLine;

            if (inBlockComment)
            {
                int end = line.IndexOf("*/", System.StringComparison.Ordinal);
                if (end < 0) continue;
                line = line.Substring(end + 2);
                inBlockComment = false;
            }
            int blockStart = line.IndexOf("/*", System.StringComparison.Ordinal);
            if (blockStart >= 0)
            {
                int blockEnd = line.IndexOf("*/", blockStart + 2, System.StringComparison.Ordinal);
                if (blockEnd >= 0)
                    line = line.Substring(0, blockStart) + line.Substring(blockEnd + 2);
                else
                {
                    line = line.Substring(0, blockStart);
                    inBlockComment = true;
                }
            }

            int c = line.IndexOf("//", System.StringComparison.Ordinal);
            if (c >= 0) line = line.Substring(0, c);

            var trimmed = line.TrimStart();
            if (trimmed.Length == 0) continue;
            if (trimmed[0] == '#') continue;

            if (depth == 0)
            {
                var m = HlslGlobalRegex.Match(line);
                if (m.Success)
                {
                    string type = m.Groups["type"].Value;
                    string name = m.Groups["name"].Value;

                    if (type != "static" && type != "const" && type != "uniform"
                        && type != "struct" && type != "cbuffer" && type != "typedef"
                        && type != "class" && type != "interface")
                    {
                        result.Add((type, name));
                    }
                }
            }

            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '{') depth++;
                else if (line[i] == '}') depth = System.Math.Max(0, depth - 1);
            }
        }

        return result;
    }

    /// <summary>仅用于「类名」的清理，字段名保留原始命名 + 前缀。</summary>
    private static string SanitizeClassName(string raw)
    {
        if (string.IsNullOrEmpty(raw)) return null;

        var sb = new StringBuilder(raw.Length);
        foreach (var c in raw)
            sb.Append(char.IsLetterOrDigit(c) ? c : '_');

        var result = Regex.Replace(sb.ToString(), "_+", "_").Trim('_');
        if (string.IsNullOrEmpty(result)) return null;
        if (char.IsDigit(result[0])) result = "_" + result;
        return result;
    }
}