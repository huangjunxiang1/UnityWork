using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

internal static class CombineLanguage
{
    static char[] splitChar = new char[] { '"', '>', '<' };
    public static void Excute()
    {
        if (Program.debug)
        {
            Program.excelPath = Environment.CurrentDirectory + "/../../../../../../Excel/Language/UIText";
        }
        if (!File.Exists($"{Program.excelPath}/Language_UIText_Chinese.txt"))
        {
            Console.WriteLine("Language_UIText_Chinese.txt not found");
            return;
        }

        Dictionary<string, string> src = new(10000);
        {
            var txts = File.ReadAllLines($"{Program.excelPath}/Language_UIText_Chinese.txt");
            for (int i = 0; i < txts.Length; i++)
            {
                var line = txts[i];
                if (line.Contains("<string name="))
                {
                    var array = line.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);
                    src[array[2]] = array[5];
                }
            }
        }

        var files = Directory.GetFiles(Program.excelPath, "Language_UIText_*.txt");
        for (int j = 0; j < files.Length; j++)
        {
            if (files[j].EndsWith("Language_UIText_Chinese.txt"))
                continue;

            Dictionary<string, (string, string)> target = new(10000);
            var txts = File.ReadAllLines(files[j]);
            for (int i = 0; i < txts.Length; i++)
            {
                var line = txts[i];
                if (line.Contains("<string name="))
                {
                    var array = line.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);
                    target[array[2]] = (array[4], array[5]);
                }
            }

            foreach (var item in target.Keys.ToList())
            {
                if (!src.ContainsKey(item))
                    target.Remove(item);
            }

            StringBuilder save = new(100000);
            save.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            save.AppendLine("<resources>");

            foreach (var item in src)
            {
                if (!target.TryGetValue(item.Key, out var v2) || v2.Item1 != item.Value || v2.Item2 == v2.Item1)
                {
                    save.AppendLine($"  <string name=\"{item.Key}\" cn=\"{item.Value}\">{item.Value}</string>");
                }
            }
            save.AppendLine("");
            save.AppendLine("");
            foreach (var item in src)
            {
                if (target.TryGetValue(item.Key, out var v2) && v2.Item1 == item.Value && v2.Item2 != v2.Item1)
                {
                    save.AppendLine($"  <string name=\"{item.Key}\" cn=\"{item.Value}\">{v2.Item2}</string>");
                }
            }

            save.AppendLine("</resources>");

            File.WriteAllText(files[j], save.ToString());
        }
        Console.WriteLine("合并语言包成功");
    }
}
