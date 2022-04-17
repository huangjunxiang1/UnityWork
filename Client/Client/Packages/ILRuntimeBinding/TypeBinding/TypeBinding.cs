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

unsafe static class TypeBinding
{
    public static void Binding(ILRuntime.Runtime.Enviorment.AppDomain appdomain)
    {
        appdomain.RegisterCrossBindingAdaptor(new CoroutineAdapter());
        appdomain.RegisterCrossBindingAdaptor(new IAsyncStateMachineAdapter());
        appdomain.RegisterCrossBindingAdaptor(new IMessageAdapter());
        appdomain.RegisterCrossBindingAdaptor(new IRequestAdapter());
        appdomain.RegisterCrossBindingAdaptor(new IResponseAdapter());
        appdomain.RegisterCrossBindingAdaptor(new ScriptableObjectAdapter());

        appdomain.RegisterValueTypeBinder(typeof(Quaternion), new QuaternionBinder());
        appdomain.RegisterValueTypeBinder(typeof(Vector2), new Vector2Binder());
        appdomain.RegisterValueTypeBinder(typeof(Vector3), new Vector3Binder());
    }
}
