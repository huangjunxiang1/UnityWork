// See https://aka.ms/new-console-template for more information

using Core;
using Game;

public static class Program
{
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
        var tsc = ThreadSynchronizationContext.GetOrCreate(Environment.CurrentManagedThreadId);
        ThreadSynchronizationContext.SetMainThread(tsc);

        List<Type> types = Types.ReflectionAllTypes();
        MessageParser.Parse(types);
        Server.Load(types);
    }
}