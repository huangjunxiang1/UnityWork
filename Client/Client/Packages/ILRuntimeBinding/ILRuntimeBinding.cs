using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using ILRuntime.Runtime.Stack;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.CLR.Utils;
using ILRuntime.CLR.Method;

public unsafe static class ILRuntimeBinding
{
    public static void Binding(ILRuntime.Runtime.Enviorment.AppDomain appdomain)
    {
        EditorAction.Binding(appdomain);
        TypeBinding.Binding(appdomain);
        DelegateBinding.Binding(appdomain);
        ILRuntime.Runtime.CLRBinding.CLRBindingUtils.Initialize(appdomain);
    }
}
