using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Game;

public static class ExFunc
{
    public static void SetChildCnt(this Transform parent, int count, GameObject res = null, bool destroyMore = false)
    {
        if (parent == null)
        {
            Loger.Error("error parent = null");
            return;
        }

        if (count < 0)
        {
            Loger.Error("error count =" + count);
            return;
        }

        int childCnt = parent.childCount;
        if (count == 0)
        {
            if (childCnt > 0)
            {
                for (int i = childCnt; i > 1; i--)
                {
                    if (destroyMore)
                        SAsset.Release(parent.GetChild(i - 1).gameObject, false);
                    else
                        parent.GetChild(i - 1).gameObject.SetActive(false);
                }

                //第一个是源物体  不删除
                parent.GetChild(0).gameObject.SetActive(false);
            }
        }
        else
        {
            if (childCnt <= 0)
            {
                Loger.Error("error childCnt=0");
                return;
            }

            if (res == null)
                res = parent.GetChild(0).gameObject;

            for (int i = childCnt; i < count; i++)
                UnityEngine.Object.Instantiate(res, parent);

            for (int i = childCnt; i > count; i--)
            {
                if (destroyMore)
                    SAsset.Release(parent.GetChild(i - 1).gameObject, false);
                else
                    parent.GetChild(i - 1).gameObject.SetActive(false);
            }

            for (int i = 0; i < count; i++)
                parent.GetChild(i).gameObject.SetActive(true);
        }
    }
    public static T FindOnCL<T>(this IEnumerable<T> ie, Func<T, bool> test)
    {
        if (ie == null)
            return default;
        foreach (var item in ie)
        {
            if (test(item))
                return item;
        }
        return default;
    }
    public static int FindIndexOnCL<T>(this IList<T> lst, Func<T, bool> test)
    {
        if (lst == null)
            return -1;
        int len = lst.Count;
        for (int i = 0; i < len; i++)
        {
            if (test(lst[i]))
                return i;
        }
        return -1;
    }
    public static List<T> FindAllOnCL<T>(this IEnumerable<T> ie, Func<T, bool> test)
    {
        List<T> ret = new();
        if (ie != null)
        {
            foreach (var item in ie)
            {
                if (test(item))
                    ret.Add(item);
            }
        }
        return ret;
    }
}