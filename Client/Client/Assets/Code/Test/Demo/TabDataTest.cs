using Main;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using System.IO;
using Unity.Mathematics;
using System.Diagnostics;

class TabDataTest
{
    [Test]
    public void testDBuffer()
    {
        using DBuffer buffM = new(new MemoryStream(File.ReadAllBytes(Application.dataPath + "/Res/Config/Tabs/TabM.bytes")));
        using DBuffer buffL = new(new MemoryStream(File.ReadAllBytes(Application.dataPath + "/Res/Config/Tabs/TabL.bytes")));
        using DBuffer buff_cn = new(new MemoryStream(File.ReadAllBytes(Application.dataPath + "/Res/Config/Tabs/Language_Chinese.bytes")));
        using DBuffer buff_en = new(new MemoryStream(File.ReadAllBytes(Application.dataPath + "/Res/Config/Tabs/Language_English.bytes")));
        using DBuffer buffM_ST = new(new MemoryStream(File.ReadAllBytes(Application.dataPath + "/Res/Config/Tabs/TabM_ST.bytes")));

        test(buffM, buffL, buff_cn, buff_en, false);
        buffM.Seek(0);
        buffL.Seek(0);
        buff_cn.Seek(0);
        buff_en.Seek(0);
        test(buffM, buffL, buff_cn, buff_en, true);

        test_ST(buffM_ST);
    }

    void test(DBuffer buffM, DBuffer buffL, DBuffer buff_cn, DBuffer buff_en, bool debug)
    {
        buffM.Compress = false;
        if (buffM.Readint() != 20220702)
            throw new System.Exception("����TabM����");
        else
        {
            buffM.Compress = buffM.Readbool();
            TabM.Init(buffM, debug);
            int key = 1;

            if (TabM.Get_test2(key).value2[0] != 5)
                throw new System.Exception("���ݳ���");
            if (TabM.Get_test2(key).value2[1] != 1221)
                throw new System.Exception("���ݳ���");

            if (TabM.Get_test2(2).value2[0] != -5)
                throw new System.Exception("���ݳ���");
            if (TabM.Get_test2(2).value2[1] != -1588994477)
                throw new System.Exception("���ݳ���");

            if (TabM.Get_test2(key).longValue != 888888888888)
                throw new System.Exception("���ݳ���");
            if (TabM.Get_test2(key).longValue2[0] != 123456456123)
                throw new System.Exception("���ݳ���");
            if (TabM.Get_test2(key).longValue2[1] != 789456456789)
                throw new System.Exception("���ݳ���");

            if (TabM.Get_test2(2).longValue != -888888888888)
                throw new System.Exception("���ݳ���");
            if (TabM.Get_test2(2).longValue2[0] != -1)
                throw new System.Exception("���ݳ���");
            if (TabM.Get_test2(2).longValue2[1] != -445566778899)
                throw new System.Exception("���ݳ���");

            if (TabM.Get_test2(key).des != "�ļ��� �ط�")
                throw new System.Exception("���ݳ���");
            if (TabM.Get_test2(key).des2[0] != "jkhs")
                throw new System.Exception("���ݳ���");
            if (TabM.Get_test2(key).des2[1] != "�����")
                throw new System.Exception("���ݳ���");
            if (TabM.Get_test2(key).v2t != new Vector2Int(5, 5))
                throw new System.Exception("���ݳ���");
            if (TabM.Get_test2(key).v2t2[0] != new Vector2Int(9, 82))
                throw new System.Exception("���ݳ���");
            if (TabM.Get_test2(key).v2t2[1] != new Vector2Int(45, 65))
                throw new System.Exception("���ݳ���");
            if (math.abs(TabM.Get_test2(key).f1 - 5.122) >= 0.0001f)
                throw new System.Exception("���ݳ���");
            if (math.abs(TabM.Get_test2(key).f2[0] - 0.945) >= 0.0001)
                throw new System.Exception("���ݳ���");
            if (math.abs(TabM.Get_test2(2).f2[1] - 0.878655) >= 0.0001)
                throw new System.Exception("���ݳ���");
            if (TabM.Get_test2(key).b1 != true)
                throw new System.Exception("���ݳ���");
            if (TabM.Get_test2(key).b2[0] != true)
                throw new System.Exception("���ݳ���");
            if (TabM.Get_test2(key).b2[1] != false)
                throw new System.Exception("���ݳ���");

            if (TabM._test2Array[1].b2[0] != true)
                throw new System.Exception("���ݳ���");
            if (TabM._test2Array[1].b2[1] != false)
                throw new System.Exception("���ݳ���");
            if (TabM._test2Array[1].b2[2] != false)
                throw new System.Exception("���ݳ���");

            if (TabM._test2Array[0].arrs[0][0] != 5)
                throw new System.Exception("���ݳ���");
            if (TabM._test2Array[0].arrs[0][1] != 9)
                throw new System.Exception("���ݳ���");
            if (TabM._test2Array[0].arrs[1][0] != 6)
                throw new System.Exception("���ݳ���");
            if (TabM._test2Array[0].arrs[1][1] != 8)
                throw new System.Exception("���ݳ���");

            if (TabM._test2Array[0].arr2s[0][0] != "ss")
                throw new System.Exception("���ݳ���");
            if (TabM._test2Array[0].arr2s[0][1] != "gg")
                throw new System.Exception("���ݳ���");
            if (TabM._test2Array[0].arr2s[1][0] != "df")
                throw new System.Exception("���ݳ���");
            if (TabM._test2Array[0].arr2s[1][1] != "hh")
                throw new System.Exception("���ݳ���");

            if (TabM._test2Array[1].arrs.Length != 0)
                throw new System.Exception("���ݳ���");
            if (TabM._test2Array[1].arr2s.Length != 0)
                throw new System.Exception("���ݳ���");
        }

        buffL.Compress = false;
        if (buffL.Readint() != 20220702)
            throw new System.Exception("����TabL����");
        else
        {
            buffL.Compress = buffL.Readbool();
            TabL.Init(buffL, debug);
        }

        buff_cn.Compress = false;
        if (buff_cn.Readint() != 20220702)
            throw new System.Exception("����Language_cn����");
        else
        {
            buff_cn.Compress = buff_cn.Readbool();
            LanguageS.Load((int)SystemLanguage.Chinese, buff_cn, true);
        }

        buff_en.Compress = false;
        if (buff_en.Readint() != 20220702)
            throw new System.Exception("����Language_en����");
        else
        {
            buff_en.Compress = buff_en.Readbool();
            LanguageS.Load((int)SystemLanguage.English, buff_en, true);
        }
    }
    void test_ST(DBuffer buffM)
    {
        TabM_ST st = new TabM_ST();
        buffM.Compress = false;
        if (buffM.Readint() != 20220702)
            throw new System.Exception("����TabM����");
        else
        {
            buffM.Compress = buffM.Readbool();
            st.Init(buffM);
            int key = 1;

            if (st.Get_test2(key).value2[0] != 5)
                throw new System.Exception("���ݳ���");
            if (st.Get_test2(key).value2[1] != 1221)
                throw new System.Exception("���ݳ���");

            if (st.Get_test2(2).value2[0] != -5)
                throw new System.Exception("���ݳ���");
            if (st.Get_test2(2).value2[1] != -1588994477)
                throw new System.Exception("���ݳ���");

            if (st.Get_test2(key).longValue != 888888888888)
                throw new System.Exception("���ݳ���");
            if (st.Get_test2(key).longValue2[0] != 123456456123)
                throw new System.Exception("���ݳ���");
            if (st.Get_test2(key).longValue2[1] != 789456456789)
                throw new System.Exception("���ݳ���");

            if (st.Get_test2(2).longValue != -888888888888)
                throw new System.Exception("���ݳ���");
            if (st.Get_test2(2).longValue2[0] != -1)
                throw new System.Exception("���ݳ���");
            if (st.Get_test2(2).longValue2[1] != -445566778899)
                throw new System.Exception("���ݳ���");

            if (math.all(st.Get_test2(key).v2t != new int2(5, 5)))
                throw new System.Exception("���ݳ���");
            if (math.all(st.Get_test2(key).v2t2[0] != new int2(9, 82)))
                throw new System.Exception("���ݳ���");
            if (math.all(st.Get_test2(key).v2t2[1] != new int2(45, 65)))
                throw new System.Exception("���ݳ���");
            if (math.abs(st.Get_test2(key).f1 - 5.122) >= 0.0001f)
                throw new System.Exception("���ݳ���");
            if (math.abs(st.Get_test2(key).f2[0] - 0.945) >= 0.0001)
                throw new System.Exception("���ݳ���");
            if (math.abs(st.Get_test2(2).f2[1] - 0.878655) >= 0.0001)
                throw new System.Exception("���ݳ���");
            if (st.Get_test2(key).b1 != true)
                throw new System.Exception("���ݳ���");
            if (st.Get_test2(key).b2[0] != true)
                throw new System.Exception("���ݳ���");
            if (st.Get_test2(key).b2[1] != false)
                throw new System.Exception("���ݳ���");

            if (st._test2Array[1].b2[0] != true)
                throw new System.Exception("���ݳ���");
            if (st._test2Array[1].b2[1] != false)
                throw new System.Exception("���ݳ���");
            if (st._test2Array[1].b2[2] != false)
                throw new System.Exception("���ݳ���");
        }
    }
}
