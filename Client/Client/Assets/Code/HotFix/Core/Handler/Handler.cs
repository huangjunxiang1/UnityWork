using Core;
using Event;
using Game;

static class CoreHandler
{
    [Event]
    static void Quit(EC_QuitGame e)
    {
        Client.Close(); 
        Server.Close();
        TabM_ST.Tab.Data.Dispose();
    }
    [Event]
    static void GameStart(EC_GameStart e)
    {
        Client.Data.Clear();
    }
}
