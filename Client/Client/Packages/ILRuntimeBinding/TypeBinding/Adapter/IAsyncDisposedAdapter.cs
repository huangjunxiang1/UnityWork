using UnityEngine;
using System.Collections.Generic;
using ILRuntime.Other;
using System;
using System.Collections;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.CLR.Method;


public class IAsyncDisposedAdapter : CrossBindingAdaptor
{
    public override Type BaseCLRType
    {
        get
        {
            return typeof(IAsyncDisposed);
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
    internal class Adaptor : IAsyncDisposed, CrossBindingAdaptorType
    {
        ILTypeInstance instance;
        ILRuntime.Runtime.Enviorment.AppDomain appdomain;

        public Adaptor()
        {

        }

        public Adaptor(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
        {
            this.appdomain = appdomain;
            this.instance = instance;
        }

        public ILTypeInstance ILInstance { get { return instance; } }

        IMethod mMethod;
        bool mMethodGot;
        public bool Disposed
        {
            get
            {
                if (!mMethodGot)
                {
                    mMethod = instance.Type.GetMethod("get_Disposed", 0);
                    mMethod ??= instance.Type.GetMethod("IAsyncDisposed.get_Disposed", 0);
                    mMethodGot = true;
                }

                if (mMethod != null)
                {
                    var res = (bool)appdomain.Invoke(mMethod, instance, null);
                    return res;
                }
                else
                {
                    return false;
                }
            }
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
