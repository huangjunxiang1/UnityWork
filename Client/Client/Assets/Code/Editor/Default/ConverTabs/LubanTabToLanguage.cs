using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

class LubanTabToLanguage
{
    public static void Excute(string excelPath, string assetsPath)
    {
        Console.WriteLine("--->Language");
        List<string> mains = ExcelHelper.GetFiles(excelPath, SearchOption.TopDirectoryOnly);

        // Excel 数据: excelMap[lan][key] = val
        Dictionary<string, Dictionary<string, string>> excelMap = new();
        // txt 数据: txtMap[lan][key] = val（写入时排在 excelMap 后面）
        Dictionary<string, Dictionary<string, string>> txtMap = new();
        // 只要 Excel 或 txt 任一处存在该语言，就加入导出列表
        HashSet<string> allLanguages = new();

        // 1. 读取 Excel 数据
        for (int fileIndex = 0; fileIndex < mains.Count; fileIndex++)
        {
            var fi = new FileInfo(mains[fileIndex]);
            using var fs = new FileStream(fi.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            IWorkbook workbook = new XSSFWorkbook(fs);

            for (int sheetIndex = 0; sheetIndex < workbook.NumberOfSheets; sheetIndex++)
            {
                var sheet = workbook.GetSheetAt(sheetIndex);
                if (sheet == null) continue;

                // 第 0 行是语言表头（C 列起为 Chinese、English 等）[cite: 1]
                var headerRow = sheet.GetRow(0);
                if (headerRow == null) continue;

                // 数据从第 3 行（Excel 第 4 行）开始；B 列（index=1）为 key[cite: 1]
                for (int rowIndex = 3; rowIndex <= sheet.LastRowNum; rowIndex++)
                {
                    var row = sheet.GetRow(rowIndex);
                    if (row == null) continue;

                    var firstCell = row.GetCell(0)?.ToString()?.Trim();
                    if (!string.IsNullOrEmpty(firstCell) && firstCell.StartsWith("##"))
                        continue;

                    var k = row.GetCell(1)?.ToString()?.Trim();
                    if (string.IsNullOrEmpty(k)) continue;

                    for (int cell = 2; cell < headerRow.LastCellNum; cell++)
                    {
                        var lan = headerRow.GetCell(cell)?.ToString()?.Trim();
                        if (string.IsNullOrEmpty(lan))
                            continue;

                        allLanguages.Add(lan);

                        if (!excelMap.TryGetValue(lan, out var kv))
                            excelMap[lan] = kv = new Dictionary<string, string>();

                        var v = row.GetCell(cell)?.ToString() ?? string.Empty;
                        if (kv.ContainsKey(k))
                        {
                            Console.WriteLine($"k={k} 已经包含");
                            Console.ReadLine();
                            return;
                        }
                        kv.Add(k, v);
                    }
                }
            }
        }

        // 2. 在 excelPath 下搜索所有 *_{lan}.txt 文件，提取 lan 并解析 XML 中的 <string name="...">...</string>[cite: 2]
        foreach (string txtPath in GetAllLanguageTxtFiles(excelPath))
        {
            string lanName = ExtractLanFromFileName(txtPath);
            if (string.IsNullOrEmpty(lanName))
                continue;

            allLanguages.Add(lanName);

            if (!txtMap.TryGetValue(lanName, out var tKv))
                txtMap[lanName] = tKv = new Dictionary<string, string>();

            XDocument doc = XDocument.Load(txtPath);
            foreach (var elem in doc.Root?.Elements("string") ?? Array.Empty<XElement>())
            {
                string nameAttr = elem.Attribute("name")?.Value?.Trim();
                if (string.IsNullOrEmpty(nameAttr))
                    continue;

                string textVal = elem.Value ?? string.Empty;

                if (tKv.ContainsKey(nameAttr))
                {
                    Console.WriteLine($"k={nameAttr} 已经包含 (lan={lanName}, file={Path.GetFileName(txtPath)})");
                    Console.ReadLine();
                    return;
                }

                tKv.Add(nameAttr, textVal);
            }
        }

        if (!string.IsNullOrEmpty(assetsPath))
        {
            Directory.CreateDirectory(assetsPath);
        }

        // 3. 写入 buffer：先写 eCount + Excel 数据，再写 tCount + txt 数据
        DBuffer buffer = new DBuffer(100000);
        foreach (string lanName in allLanguages)
        {
            excelMap.TryGetValue(lanName, out var eKv);
            txtMap.TryGetValue(lanName, out var tKv);

            int eCount = eKv?.Count ?? 0;
            int tCount = tKv?.Count ?? 0;

            buffer.Seek(11);

            buffer.Write(eCount);
            if (eKv != null)
            {
                foreach (var kv in eKv)
                {
                    buffer.Write(kv.Key);
                    buffer.Write(kv.Value);
                }
            }

            buffer.Write(tCount);
            if (tKv != null)
            {
                foreach (var kv in tKv)
                {
                    buffer.Write(kv.Key);
                    buffer.Write(kv.Value);
                }
            }

            int len = buffer.Length;
            buffer.Seek(0);
            buffer.WriteHeaderInfo();
            buffer.Seek(len);
            File.WriteAllBytes(Path.Combine(assetsPath, $"{lanName}.bytes"), buffer.ToBytes());
        }
    }

    private static IEnumerable<string> GetAllLanguageTxtFiles(string excelPath)
    {
        HashSet<string> result = new(StringComparer.OrdinalIgnoreCase);
        if (string.IsNullOrEmpty(excelPath) || !Directory.Exists(excelPath))
            return result;

        foreach (string file in Directory.GetFiles(excelPath, "*_*.txt", SearchOption.AllDirectories))
        {
            result.Add(file);
        }

        return result;
    }

    private static string ExtractLanFromFileName(string filePath)
    {
        string nameWithoutExt = Path.GetFileNameWithoutExtension(filePath);
        int underscoreIdx = nameWithoutExt.LastIndexOf('_');
        if (underscoreIdx < 0 || underscoreIdx >= nameWithoutExt.Length - 1)
            return null;

        return nameWithoutExt.Substring(underscoreIdx + 1).Trim();
    }
}