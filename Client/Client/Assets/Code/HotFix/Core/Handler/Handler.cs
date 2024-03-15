using Event;

static class CoreHandler
{
    [Event]
    static void Quit(EC_QuitGame e)
    {
        GameL.Close();
        GameM.Close();
        TabM_ST.Tab.Data.Dispose();
    }
    [Event]
    static void GameStart(EC_GameStart e)
    {
        GameM.Data.Clear();
    }
}
