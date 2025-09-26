using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    [Scene(name: "Login")]
    class LoginScene : Scene
    {
        public override void OnCreate(params object[] os)
        {
            base.OnCreate(os);
            Client.Data.Clear();
        }
    }
}
