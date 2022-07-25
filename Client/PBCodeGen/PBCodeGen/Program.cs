// See https://aka.ms/new-console-template for more information
class Enter
{
    public static bool debug = true;
    static void Main(string[] args)
    {
        if (args.Length > 1)
            bool.TryParse(args[0], out debug);

        string protoPath;
        string outputPath;
        if (debug)
        {
            protoPath = Environment.CurrentDirectory + "/../../../../../../PB/main";
            outputPath = Environment.CurrentDirectory + "/../../../../../../" + @"Client\Client\Assets\Code\HotFix\Game\_Gen\PB\";
        }
        else
        {
            protoPath = args[1];
            outputPath = args[2];
        }


        {
            Gen gen = new Gen();
            gen.resPath = protoPath;
            gen.outDefinePath = outputPath;
            gen.outRWPath = outputPath;
            gen.gen();
        }

        Console.WriteLine("生成结束");
    }
}
