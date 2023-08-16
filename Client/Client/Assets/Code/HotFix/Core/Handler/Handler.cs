using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

static class CoreHandler
{
    [Event]
    static void Quit(EC_QuitGame e)
    {
        GameL.Close();
        GameM.Close();
    }
}
