using System;
using System.Collections.Generic;
using System.Text;

public static class MethodExtension
{
    public static void AddRange<T>(this HashSet<T> hs, IEnumerable<T> itor)
    {
        if (itor == null) return;
        var it = itor.GetEnumerator();
        while (it.MoveNext())
            hs.Add(it.Current);
    }
    public static void AddRangeFormIndex<T>(this List<T> list, int index, IEnumerable<T> itor)
    {
        if (itor == null) return;
        var it = itor.GetEnumerator();
        int i = 0;
        while (it.MoveNext())
        {
            int j = index + i++;
            if (j < list.Count)
                list[j] = it.Current;
            else
                list.Add(it.Current);
        }
    }
    public static ref T ElementAt<T>(this T[] array, int index) 
    {
        return ref array[index];
    }
}
