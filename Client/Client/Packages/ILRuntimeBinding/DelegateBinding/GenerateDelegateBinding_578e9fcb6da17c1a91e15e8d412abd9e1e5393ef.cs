using System;

[ILRuntimeDelegateBinding]
unsafe static class GenerateDelegateBinding_578e9fcb6da17c1a91e15e8d412abd9e1e5393ef
{
    public static void Binding(ILRuntime.Runtime.Enviorment.AppDomain appdomain)
    {
        //xx1Start
        appdomain.DelegateManager.RegisterMethodDelegate<System.Int32>();
        appdomain.DelegateManager.RegisterMethodDelegate<FairyGUI.NTexture>();
        appdomain.DelegateManager.RegisterFunctionDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance, ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int32>();
        appdomain.DelegateManager.RegisterMethodDelegate<FairyGUI.GObject>();
        //xx1End

        //xx2Start
        appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<System.Int32>>((act) =>
        {
            return new UnityEngine.Events.UnityAction<System.Int32>((arg0) =>
            {
                ((Action<System.Int32>)act)(arg0);
            });
        });
        appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction>((act) =>
        {
            return new UnityEngine.Events.UnityAction(() =>
            {
                ((Action)act)();
            });
        });
        appdomain.DelegateManager.RegisterDelegateConvertor<System.Comparison<ILRuntime.Runtime.Intepreter.ILTypeInstance>>((act) =>
        {
            return new System.Comparison<ILRuntime.Runtime.Intepreter.ILTypeInstance>((x, y) =>
            {
                return ((Func<ILRuntime.Runtime.Intepreter.ILTypeInstance, ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int32>)act)(x, y);
            });
        });
        appdomain.DelegateManager.RegisterDelegateConvertor<FairyGUI.UIPackage.CreateObjectCallback>((act) =>
        {
            return new FairyGUI.UIPackage.CreateObjectCallback((result) =>
            {
                ((Action<FairyGUI.GObject>)act)(result);
            });
        });
        appdomain.DelegateManager.RegisterDelegateConvertor<System.Predicate<ILRuntime.Runtime.Intepreter.ILTypeInstance>>((act) =>
        {
            return new System.Predicate<ILRuntime.Runtime.Intepreter.ILTypeInstance>((obj) =>
            {
                return ((Func<ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Boolean>)act)(obj);
            });
        });
        //xx2End
    }
}
