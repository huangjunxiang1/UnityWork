using System;

unsafe static class DelegateBinding
{
    public static void Binding(ILRuntime.Runtime.Enviorment.AppDomain appdomain)
    {
        //xx1Start
        appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.Texture>();
        appdomain.DelegateManager.RegisterMethodDelegate<Main.IMessage>();
        appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.AudioClip>();
        appdomain.DelegateManager.RegisterMethodDelegate<global::EventerContent>();
        appdomain.DelegateManager.RegisterMethodDelegate<System.String, System.String, System.Type, FairyGUI.PackageItem>();
        appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.InputSystem.InputAction.CallbackContext>();
        //xx1End

        //xx2Start
        appdomain.DelegateManager.RegisterDelegateConvertor<FairyGUI.EventCallback0>((act) =>
        {
            return new FairyGUI.EventCallback0(() =>
            {
                ((Action)act)();
            });
        });
        appdomain.DelegateManager.RegisterDelegateConvertor<FairyGUI.UIPackage.LoadResourceAsync>((act) =>
        {
            return new FairyGUI.UIPackage.LoadResourceAsync((name, extension, type, item) =>
            {
                ((Action<System.String, System.String, System.Type, FairyGUI.PackageItem>)act)(name, extension, type, item);
            });
        });
        //xx2End

        foreach (var type in typeof(DelegateBinding).Assembly.GetTypes())
        {
            foreach (var method in type.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static))
            {
                if (method.GetCustomAttributes(typeof(ILRuntimeDelegateBindingAttribute), false).Length > 0)
                    method.Invoke(null, new object[] { appdomain });
            }
        }
    }
}
