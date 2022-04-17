using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Main;
using Game;
using FairyGUI;

abstract class FUIBase : UIBase
{
    public abstract GComponent UI { get; }
}
