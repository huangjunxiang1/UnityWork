using Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Rendering.VirtualTexturing;

namespace Game
{
    public class SUnityObject : STree
    {
        public SUnityObject(long rpc = 0) : base(GameM.World, rpc)
        {
        }
    }
}
