using Core;
using Event;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Game
{
    public class Scene : STree
    {
        public virtual void OnCreate(params object[] os) { }
    }
}
