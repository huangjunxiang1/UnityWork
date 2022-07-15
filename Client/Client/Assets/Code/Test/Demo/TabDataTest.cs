using Main;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using System.IO;
using Unity.Mathematics;

class TabDataTest
{
    [Test]
    public void testCustom()
    {
        DBytesBuffer buff = new DBytesBuffer();
        buff.Write(1588994477);
        buff.Write(-1588994477);
        buff.Seek(0);
        Loger.Log(buff.Readint());
        Loger.Log(buff.Readint());
    }

    [Test]
    public void testDBytesBuffer()
    {
        DBytesBuffer buffM = new(File.ReadAllBytes(Application.dataPath + "/Res/Config/Tabs/TabM.bytes"));
        DBytesBuffer buffL = new(File.ReadAllBytes(Application.dataPath + "/Res/Config/Tabs/TabL.bytes"));
        DBytesBuffer buff_cn = new(File.ReadAllBytes(Application.dataPath + "/Res/Config/Tabs/Language_cn.bytes"));
        DBytesBuffer buff_en = new(File.ReadAllBytes(Application.dataPath + "/Res/Config/Tabs/Language_en.bytes"));
        test(buffM, buffL, buff_cn, buff_en);
    }

    [Test]
    public void testDStreamBuffer()
    {
        using DStreamBuffer buffM = new(File.Open(Application.dataPath + "/Res/Config/Tabs/TabM.bytes", FileMode.Open));
        using DStreamBuffer buffL = new(File.Open(Application.dataPath + "/Res/Config/Tabs/TabL.bytes", FileMode.Open));
        using DStreamBuffer buff_cn = new(File.Open(Application.dataPath + "/Res/Config/Tabs/Language_cn.bytes", FileMode.Open));
        using DStreamBuffer buff_en = new(File.Open(Application.dataPath + "/Res/Config/Tabs/Language_en.bytes", FileMode.Open));
        test(buffM, buffL, buff_cn, buff_en);
    }

    void test(DBuffer buffM, DBuffer buffL, DBuffer buff_cn, DBuffer buff_en)
    {
        buffM.Compress = false;
        if (buffM.Readint() != 20220702)
            throw new System.Exception("不是TabM数据");
        else
        {
            buffM.Compress = buffM.Readbool();
            TabM.Init(buffM);
            int key = 1;

            if (TabM.Get_test2(key).value2[0] != 5)
                throw new System.Exception("数据出错");
            if (TabM.Get_test2(key).value2[1] != 1221)
                throw new System.Exception("数据出错");

            if (TabM.Get_test2(2).value2[0] != -5)
                throw new System.Exception("数据出错");
            if (TabM.Get_test2(2).value2[1] != -1588994477)
                throw new System.Exception("数据出错");

            if (TabM.Get_test2(key).longValue != 888888888888)
                throw new System.Exception("数据出错");
            if (TabM.Get_test2(key).longValue2[0] != 123456456123)
                throw new System.Exception("数据出错");
            if (TabM.Get_test2(key).longValue2[1] != 789456456789)
                throw new System.Exception("数据出错");

            if (TabM.Get_test2(2).longValue != -888888888888)
                throw new System.Exception("数据出错");
            if (TabM.Get_test2(2).longValue2[0] != -1)
                throw new System.Exception("数据出错");
            if (TabM.Get_test2(2).longValue2[1] != -445566778899)
                throw new System.Exception("数据出错");

            if (TabM.Get_test2(key).des != "的肌肤 地方")
                throw new System.Exception("数据出错");
            if (TabM.Get_test2(key).des2[0] != "jkhs")
                throw new System.Exception("数据出错");
            if (TabM.Get_test2(key).des2[1] != "款到发货")
                throw new System.Exception("数据出错");
            if (TabM.Get_test2(key).v2t != new Vector2Int(5, 5))
                throw new System.Exception("数据出错");
            if (TabM.Get_test2(key).v2t2[0] != new Vector2Int(9, 82))
                throw new System.Exception("数据出错");
            if (TabM.Get_test2(key).v2t2[1] != new Vector2Int(45, 65))
                throw new System.Exception("数据出错");
            if (math.abs(TabM.Get_test2(key).f1 - 5.122) >= 0.0001f)
                throw new System.Exception("数据出错");
            if (math.abs(TabM.Get_test2(key).f2[0] - 0.945) >= 0.0001)
                throw new System.Exception("数据出错");
            if (math.abs(TabM.Get_test2(2).f2[1] - 0.878655) >= 0.0001)
                throw new System.Exception("数据出错");
            if (TabM.Get_test2(key).b1 != true)
                throw new System.Exception("数据出错");
            if (TabM.Get_test2(key).b2[0] != true)
                throw new System.Exception("数据出错");
            if (TabM.Get_test2(key).b2[1] != false)
                throw new System.Exception("数据出错");

            if (TabM._test2Array[1].b2[0] != true)
                throw new System.Exception("数据出错");
            if (TabM._test2Array[1].b2[1] != false)
                throw new System.Exception("数据出错");
            if (TabM._test2Array[1].b2[2] != false)
                throw new System.Exception("数据出错");
        }

        buffL.Compress = false;
        if (buffL.Readint() != 20220702)
            throw new System.Exception("不是TabL数据");
        else
        {
            buffL.Compress = buffL.Readbool();
            TabL.Init(buffL);
        }

        buff_cn.Compress = false;
        if (buff_cn.Readint() != 20220702)
            throw new System.Exception("不是Language_cn数据");
        else
        {
            buff_cn.Compress = buff_cn.Readbool();
            LanguageS.Load((int)SystemLanguage.Chinese, buff_cn);
        }

        buff_en.Compress = false;
        if (buff_en.Readint() != 20220702)
            throw new System.Exception("不是Language_en数据");
        else
        {
            buff_en.Compress = buff_en.Readbool();
            LanguageS.Load((int)SystemLanguage.English, buff_en);
        }
    }
}
