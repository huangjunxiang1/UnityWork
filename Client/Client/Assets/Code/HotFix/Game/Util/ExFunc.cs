using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Game;
using Main;

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
                    {
                        GameObject g = parent.GetChild(i - 1).gameObject;
                        AssetLoad.ReleaseTextureRef(g);
                        AssetLoad.TryReleaseGameObject(g);
                    }
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
                {
                    GameObject g = parent.GetChild(i - 1).gameObject;
                    AssetLoad.ReleaseTextureRef(g);
                    AssetLoad.TryReleaseGameObject(g);
                }
                else
                    parent.GetChild(i - 1).gameObject.SetActive(false);
            }

            for (int i = 0; i < count; i++)
                parent.GetChild(i).gameObject.SetActive(true);
        }
    }
    public static bool ContainsOnCL<T>(this IEnumerable<T> ie, Func<T, bool> test)
    {
        if (ie == null)
            return false;
        foreach (var item in ie)
        {
            if (test(item))
                return true;
        }
        return false;
    }
    public static int FindCount<T>(this IEnumerable<T> ie, Func<T, bool> test)
    {
        if (ie == null)
            return 0;
        int count = 0;
        foreach (var item in ie)
        {
            if (test(item))
                count++;
        }
        return count;
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
        List<T> ret = new List<T>();
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
    public static void RemoveOnCL<T>(this IList<T> lst, Func<T, bool> test)
    {
        if (lst == null)
            return;
        for (int i = lst.Count - 1; i >= 0; i--)
        {
            if (test(lst[i]))
                lst.RemoveAt(i);
        }
    }
    public static void SortOnCL<T>(this IList<T> lst, Func<T, T, bool> test)
    {
        if (lst == null)
            return;
        int cnt = lst.Count;

        //使用有序排列
        for (int j = 1; j < cnt; j++)
        {
            T item = lst[j];
            int i = j - 1;
            while (i >= 0 && test(lst[i], item))
            {
                lst[i + 1] = lst[i];
                i--;
            }
            lst[i + 1] = item;
        }
    }
}