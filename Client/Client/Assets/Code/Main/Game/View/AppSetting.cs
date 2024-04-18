using UnityEngine;

namespace Game
{
    public static class AppSetting
    {
        static public bool Debug { get; set; }
        static public CodeRuntime Runtime { get; set; }
    }
    public enum CodeRuntime
    {
        Native,
        Assembly,
    }
}