using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "WSO/" + nameof(TestSO), fileName = nameof(TestSO), order = 1)]
public class TestSO : ScriptableObject
{
    public int a;
    public bool b;
    public string c;

    public List<int> d;
}
