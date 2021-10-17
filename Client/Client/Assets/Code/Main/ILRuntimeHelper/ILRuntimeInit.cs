using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class ILRuntimeInit
{
    public static void Init(ILRuntime.Runtime.Enviorment.AppDomain app)
    {
        app.RegisterCrossBindingAdaptor(new IAsyncStateMachineAdapter());
        app.RegisterCrossBindingAdaptor(new IMessageAdapter());
        app.RegisterCrossBindingAdaptor(new IRequestAdapter());
        app.RegisterCrossBindingAdaptor(new IResponseAdapter());

        //xx1

        //xx2

    }
}