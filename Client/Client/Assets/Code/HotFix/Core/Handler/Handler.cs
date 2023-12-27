using Main;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

static class CoreHandler
{
    [Event]
    static void Quit(EC_QuitGame e)
    {
        SGameL.Close();
        SGameM.Close();
        STabM_ST.Tab.Data.Dispose();
    }
}
