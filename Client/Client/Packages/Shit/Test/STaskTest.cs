using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

internal class STaskTest
{
    public static void test()
    {
        Array.Clear(array, 0, array.Length);
        wait1(0);
        wait1(1);
    }

    static int[] array = new int[2];
    static async STask wait1(int index)
    {
        using (await STaskLocker.Lock(5))
        {
            await STask.Delay(500 - index * 100);
            array[index]++;
            if (index == 0)
            {
                if (array[1] != 0)
                    throw new Exception();
                else
                    Debug.Log("STaskTest succ " + index);
            }
            else if (index == 1)
            {
                if (array[0] != 1)
                    throw new Exception();
                else
                    Debug.Log("STaskTest succ " + index);
            }
        }
    }
}
