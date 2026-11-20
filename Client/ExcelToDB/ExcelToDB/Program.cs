using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        string type = Environment.GetEnvironmentVariable(nameof(type));
        
        if (type == "ExcelToLanguage")
        //Language表 
        {
            string excelPath = Environment.GetEnvironmentVariable(nameof(excelPath));
            string assetsPath = Environment.GetEnvironmentVariable(nameof(assetsPath));
#if DEBUG
            excelPath = Environment.CurrentDirectory + "/../../../../../../Excel/l10n";
            assetsPath = Environment.CurrentDirectory + "/../../../../../../Client/Client/Assets/Res(Config/raw/Tabs/Lan";
#endif
            LubanTabToLanguage.Excute(excelPath, assetsPath);
        }
        if (type == "CombineLanguage")
        {
            string excelPath = Environment.GetEnvironmentVariable(nameof(excelPath));
            string assetsPath = Environment.GetEnvironmentVariable(nameof(assetsPath));
#if DEBUG
            excelPath = Environment.CurrentDirectory + "/../../../../../../Excel/l10n";
#endif
            CombineLanguage.Excute(excelPath);
        }

        Console.WriteLine("生成成功");
        Console.WriteLine();
    }
}
