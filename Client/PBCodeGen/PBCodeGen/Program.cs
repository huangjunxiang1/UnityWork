// See https://aka.ms/new-console-template for more information
class Enter
{
    public static bool debug = true;
    static void Main(string[] args)
    {
        debug = bool.Parse(args[0]);

        string protoPath;
        string outputPath;
        if (debug)
        {
            protoPath = Environment.CurrentDirectory + "/../../../../../../PB/main";
            outputPath = Environment.CurrentDirectory + "/../../../../../../" + @"Client\Client\Assets\Code\Main\_Gen\PB\";
        }
        else
        {
            protoPath = Environment.GetEnvironmentVariable(nameof(protoPath));
            outputPath = Environment.GetEnvironmentVariable(nameof(outputPath));
        }


        {
            Parser gen = new Parser();
            gen.path = protoPath;
            var ret = gen.parse();

            CmdGenAndResponse.Gen(ret);
            CodeGen.Gen(ret, outputPath);
        }

        Console.WriteLine($"生成结束");
    }
}
