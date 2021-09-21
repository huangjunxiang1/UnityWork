using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Loger
{
    public static void Log(object o)
    {
        UnityEngine.Debug.Log(o);
    }
    public static void Error(object o)
    {
        UnityEngine.Debug.LogError(o);
    }
}