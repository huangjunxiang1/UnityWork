
static class EcsSystem
{
    public static void test()
    {
        Change.test();
        AnyChange.test();
        UpdateCheck.test();
        In.test();
        Out.test();
        EventWatcher.test();
        KVWatcherTest.test();
        //TimerCheck.test();
    }
}
