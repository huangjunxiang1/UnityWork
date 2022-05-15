using Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class AppSetting
{
    public const string LoginAddressInner = "127.0.0.1:10002";
    public const string LoginAddressOuter = "139.155.0.67:10002";

    static Reporter _reporter;
    static bool _debug;
    static bool _showReporter;

    public static bool Debug
    {
        get => _debug;
        set
        {
            if (!_reporter)
                _reporter = UnityEngine.GameObject.Find("Reporter")?.GetComponent<Reporter>();
            if (_reporter)
            {
#if UNITY_EDITOR
                _reporter.gameObject.SetActive(false);
#else

                _reporter.gameObject.SetActive(value);
#endif
            }
            _debug = value;
        }
    }
    public static bool ShowReporter
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
            _reporter.doShow(value);
#endif
        }
    }

    public static CodeRuntime Runtime { get; set; }
    public static ServiceType ServiceType { get; set; }
}
