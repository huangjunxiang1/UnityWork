using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static partial class Util
{
    public static T AddOrGetComponent<T>(this GameObject target) where T : Component
    {
        if (!target.GetComponent<T>())
            target.AddComponent<T>();
        return target.GetComponent<T>();
    }
}
