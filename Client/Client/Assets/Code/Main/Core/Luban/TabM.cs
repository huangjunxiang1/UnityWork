using System;
using System.Collections.Generic;
using System.Text;
using Unity.Burst;

public static class TabM
{
    readonly static SharedStatic<Native.Tables> _data = SharedStatic<Native.Tables>.GetOrCreate<Native.Tables>();
    public static ref Native.Tables Data => ref _data.Data;

    public static void Init(Native.Tables tables)
    {
        Dispose();
        _data.Data = tables;
    }
    public static void Dispose()
    {
        _data.Data.Dispose();
    }
}
