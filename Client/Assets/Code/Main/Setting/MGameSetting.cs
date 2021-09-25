using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class MGameSetting
{
    public const string LoginAddress = "127.0.0.1:10002";

    public static bool Debug { get; internal set; }
    public static NetModel NetModel { get; internal set; }
}
