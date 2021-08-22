using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Core
{
    public static class MGameSetting
    {
        public const string IP = "";
        public const int Port = 0;

        public static bool Debug { get; internal set; }
        public static NetModel NetModel { get; internal set; }
    }
}
