using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


[CreateAssetMenu(menuName = "WSO/" + nameof(ColorSetting), fileName = nameof(ColorSetting), order = 1)]
public class ColorSetting : ScriptableObject
{
    public Color[] Colors;
}
