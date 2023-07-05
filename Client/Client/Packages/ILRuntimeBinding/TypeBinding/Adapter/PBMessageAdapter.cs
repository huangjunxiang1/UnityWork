using UnityEngine;
using System.Collections.Generic;
using ILRuntime.Other;
using System;
using System.Collections;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.CLR.Method;
using System.Runtime.CompilerServices;
using PB;

public class PBMessageAdapter : CrossBindingAdaptor
{
    public override Type BaseCLRType
    {
        get
        {
            return typeof(PB.PBMessage);
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
    public class Adaptor : PB.PBMessage, CrossBindingAdaptorType
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

        public ILTypeInstance ILInstance { get { return instance; } set { instance = value; } }

        public ILRuntime.Runtime.Enviorment.AppDomain AppDomain { get { return appdomain; } set { appdomain = value; } }

        IMethod mWriteMethod;
        bool mWriteGot;
        public override void Write(PBWriter writer)
        {
            if (!mWriteGot)
            {
                mWriteMethod = instance.Type.GetMethod("Write", 1);
                mWriteGot = true;
            }
            if (mWriteMethod != null)
            {
                appdomain.Invoke(mWriteMethod, instance, writer);
            }
        }

        IMethod mReadMethod;
        bool mReadGot;
        public override void Read(PBReader reader)
        {
            if (!mReadGot)
            {
                mReadMethod = instance.Type.GetMethod("Read", 1);
                mReadGot = true;
            }
            if (mReadMethod != null)
            {
                appdomain.Invoke(mReadMethod, instance, reader);
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
