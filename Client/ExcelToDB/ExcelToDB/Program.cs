using System;
using OfficeOpenXml;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

class Program
{
    public static bool debug = true;

    public static bool compress = true;
    public static string codePath = Environment.CurrentDirectory + "/../../../../../Client/Assets/Code/Main/_Gen/";
    public static string assetsPath = Environment.CurrentDirectory + "/../../../../../Client/Assets/Res/Config/raw/Tabs/";
    public static string excelPath = Environment.CurrentDirectory + "/../../../../../../Excel/main";
    public static string TabName = "TabM";
    public static bool genMapping = true;
    public static bool genEcs = true;
    public static string type = "";
    public static bool ClearBytes = true;

    static void Main(string[] args)
    {
        debug = bool.Parse(args[0]);

        if (!debug)
        {
            assetsPath = Environment.GetEnvironmentVariable(nameof(assetsPath));
            excelPath = Environment.GetEnvironmentVariable(nameof(excelPath));
            ClearBytes = bool.Parse(Environment.GetEnvironmentVariable(nameof(ClearBytes)));
            codePath = Environment.GetEnvironmentVariable(nameof(codePath));
            type = Environment.GetEnvironmentVariable(nameof(type));
        }

        if (ClearBytes)
        {
            foreach (var item in Directory.GetFiles(assetsPath, "*.bytes"))
                File.Delete(item);
            foreach (var item in Directory.GetFiles(assetsPath, "*.txt"))
                File.Delete(item);
        }

        ExcelPackage.LicenseContext = LicenseContext.Commercial;
        //main
        if (type == "ExcelToDB")
        {
            if (!debug)
            {
                compress = bool.Parse(Environment.GetEnvironmentVariable(nameof(compress)));
                TabName = Environment.GetEnvironmentVariable(nameof(TabName));
                genMapping = bool.Parse(Environment.GetEnvironmentVariable(nameof(genMapping)));
                genEcs = bool.Parse(Environment.GetEnvironmentVariable(nameof(genEcs)));
            }

            Console.WriteLine("--->" + TabName);
            CodeGen gen = new CodeGen();
            gen.name = TabName;
            gen.excelPath = excelPath;
            gen.codePath = codePath;
            gen.dataPath = assetsPath;
            gen.genMapping = genMapping;
            gen.genEcs = genEcs;
            gen.Gen();
        }
        else if (type == "ExcelToLanguage")
        //Language表 
        {
            toLanguage.Excute();
        }
        else if (type == "CombineLanguage")
        {
            CombineLanguage.Excute();
        }

        Console.WriteLine("生成成功");
        Console.WriteLine();
    }
}
