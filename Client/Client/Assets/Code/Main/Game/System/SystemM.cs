using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Main;

namespace Game
{
    public abstract class SystemM
    {
        static List<SystemM> systems = new List<SystemM>();

        [Event((int)EventIDM.Init)]
        static void Init()
        {
            for (int i = 0; i < Types.MainTypes.Length; i++)
            {
                if (typeof(SystemM).IsAssignableFrom(Types.MainTypes[i]))
                {
                    if (Types.MainTypes[i] == typeof(SystemM))
                        continue;
                    SystemM sys = (SystemM)Activator.CreateInstance(Types.MainTypes[i]);
                    systems.Add(sys);
                }
            }
            systems.Sort((x, y) =>
            {
                var ax = Reflection.GetAttribute(x.GetType(), typeof(SystemExecuteOrderAttribute));
                var ay = Reflection.GetAttribute(y.GetType(), typeof(SystemExecuteOrderAttribute));

                int ox = 0;
                int oy = 0;
                if (ax != null)
                    ox = ((SystemExecuteOrderAttribute)ax).SortOrder;
                if (ay != null)
                    oy = ((SystemExecuteOrderAttribute)ay).SortOrder;
                return oy - ox;
            });

            for (int i = 0; i < systems.Count; i++)
                systems[i].Enter();

            Timer.Add(0, -1, update);
        }

        static void update()
        {
            for (int i = 0; i < systems.Count; i++)
            {
                try
                {
                    systems[i].Update();
                }
                catch (Exception e)
                {
#if ILRuntime
                    Loger.Error($"System Updata Error ILRuntimeStackTrace={e.Data["StackTrace"]}" + e);
#else
                    Loger.Error("System Updata Error=" + e);
#endif
                }
            }
        }

        protected abstract void Enter();
        protected abstract void Update();
    }
}
