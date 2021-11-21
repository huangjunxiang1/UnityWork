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

    static bool _debug;
    public static bool Debug
    {
        get => _debug;
        set
        {
            UnityEngine.GameObject reporter = UnityEngine.GameObject.Find("Reporter");
            if (reporter)
            {
#if UNITY_EDITOR
                reporter.SetActive(false);
#else

                reporter.SetActive(value);
#endif
            }
            _debug = value;
        }
    }
    public static CodeRuntime Runtime { get; set; }
    public static ServiceType ServiceType { get; set; }
}
