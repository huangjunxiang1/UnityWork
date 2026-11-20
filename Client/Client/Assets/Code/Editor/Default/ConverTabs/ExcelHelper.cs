using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
internal class ExcelHelper
{
    public static List<string> GetFiles(string path, SearchOption op)
    {
        List<string> files = new List<string>();
        string[] s = Directory.GetFiles(path, "*.xlsx", op);
        for (int i = 0; i < s.Length; i++)
        {
            FileInfo fi = new FileInfo(s[i]);
            if (fi.Name.StartsWith("~")) continue;
            files.Add(s[i]);
        }
        s = Directory.GetFiles(path, "*.xls", op);
        for (int i = 0; i < s.Length; i++)
        {
            FileInfo fi = new FileInfo(s[i]);
            if (fi.Name.StartsWith("~")) continue;
            files.Add(s[i]);
        }
        return files;
    }
}
