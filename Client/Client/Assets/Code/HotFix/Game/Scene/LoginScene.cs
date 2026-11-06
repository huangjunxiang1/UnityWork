using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Scene(AssetsID.Res.scene.Login)]
class LoginScene : Scene
{
    public override async void OnEnter()
    {
        Game.Data.Clear();
        await Game.UI.OpenAsync<FUILogin>();
    }
}
