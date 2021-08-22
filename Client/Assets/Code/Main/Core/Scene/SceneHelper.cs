using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Main.Core
{
    public static class SceneHelper
    {
        public static string Current { get; private set; }

        public static void LoadScene(string name)
        {
            SceneManager.LoadScene(name);
            Current = name;
        }
       
    }
}
