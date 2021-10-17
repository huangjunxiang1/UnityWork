using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class AppSetting
{
    public const string LoginAddress = "127.0.0.1:10002";

    public static bool Debug { get; set; }
    public static bool ILRunTime { get; set; }
}
