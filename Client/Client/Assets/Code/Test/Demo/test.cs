using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

class test 
{
    [Test]
    public void pass()
    {
        Loger.Log("pass");
    }
    [Test]
    public void noPass()
    {
        Loger.Error("noPass");
        throw new System.Exception("noPass");
    }
}
