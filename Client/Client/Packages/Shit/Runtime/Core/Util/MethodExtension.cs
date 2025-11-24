using System;
using System.Collections.Generic;
using System.Text;

public static class MethodExtension
{
    public static ref T ElementAt<T>(this T[] array, int index) 
    {
        return ref array[index];
    }
}
