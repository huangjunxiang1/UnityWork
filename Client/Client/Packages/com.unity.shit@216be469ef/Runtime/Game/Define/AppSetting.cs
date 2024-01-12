using Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;

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
                _reporter.doShow(value);
#endif
            }
        }

        static public CodeRuntime Runtime { get; set; }
    }
}