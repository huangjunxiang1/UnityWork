namespace Game
{
    public static class AppSetting
    {
        static Reporter _reporter;
        static bool _debug;
        static bool _showReporter;

        static public bool Debug
        {
            get => _debug;
            set
            {
                _debug = value;
                if (!_reporter)
                    _reporter = UnityEngine.Object.FindFirstObjectByType<Reporter>();
#if !UNITY_EDITOR
                if (value)
                {
                    if (!_reporter)
                    {
                        var res = Resources.Load<GameObject>("Reporter");
                        if (res)
                            GameObject.DontDestroyOnLoad(_reporter = GameObject.Instantiate(res).GetComponent<Reporter>());
                    }
                }
#endif
                if (_reporter)
                    _reporter.gameObject.SetActive(value);
            }
        }
        static public bool ShowReporter
        {
            get => _showReporter;
            set
            {
                if (!_debug)
                    return;
#if UNITY_EDITOR
                return;
#else
                if (_showReporter == value)
                    return;
                _showReporter = value;
                _reporter?.doShow(value);
#endif
            }
        }

        static public CodeRuntime Runtime { get; set; }
    }
    public enum CodeRuntime
    {
        Native,
        Assembly,
    }
}