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
        public override async void OnEnter()
        {
            Client.Data.Clear();
            await Client.UI.OpenAsync<FUILogin>();
        }
    }
}
