﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DisableAutoRegisteredEventAttribute : SAttribute { }
    [AttributeUsage(AttributeTargets.Class)]
    public class DisableAutoRegisteredRPCEventAttribute : SAttribute { }
}