using UnityEngine;
using System.Collections.Generic;
using ILRuntime.Other;
using System;
using System.Collections;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.CLR.Method;
using System.Runtime.CompilerServices;

public class IResponseAdapter : CrossBindingAdaptor
{
    public override Type BaseCLRType
    {
        get
        {
            return typeof(Main.IResponse);
        }
    }

    public override Type AdaptorType
    {
        get
        {
            return typeof(Adaptor);
        }
    }

    public override object CreateCLRInstance(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
    {
        return new Adaptor(appdomain, instance);
    }
    //为了完整实现MonoBehaviour的所有特性，这个Adapter还得扩展，这里只抛砖引玉，只实现了最常用的Awake, Start和Update
    public class Adaptor : Main.IResponse, CrossBindingAdaptorType
    {
        ILTypeInstance instance;
        ILRuntime.Runtime.Enviorment.AppDomain appdomain;

        IMethod ErrorGetter;
        IMethod ErrorSetter;
        IMethod MessageGetter;
        IMethod MessageSetter;
        IMethod rpcIdGetter;
        IMethod rpcIdSetter;

        public Adaptor()
        {

        }

        public Adaptor(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
        {
            this.appdomain = appdomain;
            this.instance = instance;

            ErrorGetter = appdomain.ObjectType.GetMethod("get_Error", 0);
            ErrorSetter = appdomain.ObjectType.GetMethod("set_Error", 1);
            MessageGetter = appdomain.ObjectType.GetMethod("get_Message", 0);
            MessageSetter = appdomain.ObjectType.GetMethod("set_Message", 1);
            rpcIdGetter = appdomain.ObjectType.GetMethod("get_RpcId", 0);
            rpcIdSetter = appdomain.ObjectType.GetMethod("set_RpcId", 1);
        }

        public ILTypeInstance ILInstance { get { return instance; } set { instance = value; } }

        public ILRuntime.Runtime.Enviorment.AppDomain AppDomain { get { return appdomain; } set { appdomain = value; } }

        public int Error
        {
            get { return (int)appdomain.Invoke(ErrorGetter, instance, null); }
            set { appdomain.Invoke(ErrorSetter, instance, value); }
        }
        public string Message
        {
            get { return (string)appdomain.Invoke(MessageGetter, instance, null); }
            set { appdomain.Invoke(MessageSetter, instance, value); }
        }
        public int RpcId
        {
            get { return (int)appdomain.Invoke(rpcIdGetter, instance, null); }
            set { appdomain.Invoke(rpcIdSetter, instance, value); }
        }

        public override string ToString()
        {
            IMethod m = appdomain.ObjectType.GetMethod("ToString", 0);
            m = instance.Type.GetVirtualMethod(m);
            if (m == null || m is ILMethod)
            {
                return instance.ToString();
            }
            else
                return instance.Type.FullName;
        }
    }
}
