using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal static class toLanguage
{
    public static void Excute()
    {
        Console.WriteLine("--->Language");
        List<string> mains = Common.getFiles(Program.excelPath);

        Dictionary<string, Dictionary<string, string>> map = new();
        for (int fileIndex = 0; fileIndex < mains.Count; fileIndex++)
        {
            var fi = new FileInfo(mains[fileIndex]);
            var bytes = File.ReadAllBytes(fi.FullName);
            IWorkbook workbook = new XSSFWorkbook(new MemoryStream(bytes));

            for (int sheetIndex = 0; sheetIndex < workbook.NumberOfSheets; sheetIndex++)
            {
                var sheet = workbook.GetSheetAt(sheetIndex);

                for (int rowIndex = 2; rowIndex <= sheet.LastRowNum; rowIndex++)
                {
                    var row = sheet.GetRow(rowIndex);
                    if (row == null) continue;
                    var k = row.GetCell(0)?.ToString();
                    if (string.IsNullOrEmpty(k)) continue;
                    int cell = fi.Name == "#genFromCode.xlsx" ? 2 : 1;
                    for (; cell < row.LastCellNum; cell++)
                    {
                        var lan = sheet.GetRow(1)?.GetCell(cell)?.ToString();
                        if (string.IsNullOrEmpty(lan))
                            continue;
                        if (!map.TryGetValue(lan, out var kv))
                            map[lan] = kv = new();
                        var v = row.GetCell(cell)?.ToString();
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

        DBuffer buffer = new DBuffer(100000);
        foreach (var lan in map)
        {
            buffer.Seek(11);
            buffer.Write(lan.Value.Count);
            foreach (var kv in lan.Value)
            {
                buffer.Write(kv.Key);
                buffer.Write(kv.Value);
            }
            int len = buffer.Length;
            buffer.Seek(0);
            buffer.WriteHeaderInfo();
            buffer.Seek(len);
            File.WriteAllBytes(Program.assetsPath + $"/Language_{lan.Key}.bytes", buffer.ToBytes());
        }
        

        foreach (var item in Directory.GetFiles($"{Program.excelPath}/UIText", "*.txt"))
        {
            File.WriteAllText($"{Program.assetsPath}/{new FileInfo(item).Name}", File.ReadAllText(item));
        }
    }
}
