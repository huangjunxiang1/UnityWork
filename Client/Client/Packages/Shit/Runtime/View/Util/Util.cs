using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static partial class Util
{
    public static T GetOrAddComponent<T>(this GameObject target) where T : Component
    {
        var c = target.GetComponent<T>();
        if (c) return c;
        return target.AddComponent<T>();
    }
    public static void Clear(this GraphicsBuffer graphicsBuffer)
    {
        int len = graphicsBuffer.count * graphicsBuffer.stride;
        var tmp = ArrayPool<byte>.Shared.Rent(len);
          
        Array.Clear(tmp, 0, tmp.Length);
        graphicsBuffer.SetData(tmp, 0, 0, len);

        ArrayPool<byte>.Shared.Return(tmp);
    }
}
