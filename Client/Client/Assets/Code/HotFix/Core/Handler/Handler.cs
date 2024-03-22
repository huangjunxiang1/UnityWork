using Core;
using Event;
using Game;

static class CoreHandler
{
    [Event]
    static void Quit(EC_QuitGame e)
    {
        GameWorld.Close();
        TabM_ST.Tab.Data.Dispose();
    }
    [Event]
    static void GameStart(EC_GameStart e)
    {
        GameWorld.World.Data.Clear();
    }
}
