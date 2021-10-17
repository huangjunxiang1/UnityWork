using UnityEngine;
using System.Collections.Generic;
using ILRuntime.Other;
using System;
using System.Collections;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.CLR.Method;
using System.Runtime.CompilerServices;

public class IAsyncStateMachineAdapter : CrossBindingAdaptor
{
    public override Type BaseCLRType
    {
        get
        {
            return typeof(IAsyncStateMachine);
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
    public class Adaptor : IAsyncStateMachine, CrossBindingAdaptorType
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

        IMethod mMoveNextMethod;
        bool mMoveNextMethodGot;
        public void MoveNext()
        {
            if (instance != null)
            {
                if (!mMoveNextMethodGot)
                {
                    mMoveNextMethod = instance.Type.GetMethod("MoveNext", 0);
                    mMoveNextMethodGot = true;
                }

                if (mMoveNextMethod != null)
                {
                    appdomain.Invoke(mMoveNextMethod, instance, null);
                }
            }
        }

        IMethod mSetStateMachineMethod;
        bool mSetStateMachineMethodGot;
        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
            if (instance != null)
            {
                if (!mSetStateMachineMethodGot)
                {
                    mSetStateMachineMethod = instance.Type.GetMethod("SetStateMachine", 1);
                    mSetStateMachineMethodGot = true;
                }

                if (mSetStateMachineMethod != null)
                {
                    appdomain.Invoke(mSetStateMachineMethod, instance, stateMachine);
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
