using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using YooAsset;

[CreateAssetMenu(menuName = "WSO/" + nameof(APPConfig), fileName = nameof(APPConfig), order = 1)]
public class APPConfig : ScriptableObject
{
    static APPConfig _inst;
    public static APPConfig Inst => _inst ??= Resources.Load<APPConfig>(nameof(APPConfig));

    public EPlayMode EPlayMode;
    public string resUrl;
}