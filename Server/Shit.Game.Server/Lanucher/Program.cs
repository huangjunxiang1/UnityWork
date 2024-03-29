// See https://aka.ms/new-console-template for more information

using Core;
using Game;
using System.Reflection;
using Unity.Mathematics;

public static class Program
{
    class temp
    {
        public float2 f2 = 1;
    }
    static void Main(string[] args)
    {
        try
        {
            gameStart();
        }
        catch (Exception e)
        {
            Console.WriteLine("error " + e);
            Console.ReadLine();
            throw;
        }
    }
    static void gameStart()
    {
        var tsc = ThreadSynchronizationContext.GetOrCreate(Thread.CurrentThread.ManagedThreadId);
        ThreadSynchronizationContext.SetMainThread(tsc);
        Loger.__get__log += o =>
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"{o}");
        };
        Loger.__get__warning += o =>
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"warning:{o}");
        };
        Loger.__get__error += o =>
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"error:{o}");
        };

        List<Type> types = Types.ReflectionAllTypes();
        MessageParser.Parse(types);
        Server.Load(types);
    }
}