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
            for (int i = 0; i < TypesCache.MainTypes.Length; i++)
            {
                if (typeof(SystemM).IsAssignableFrom(TypesCache.MainTypes[i]))
                {
                    if (TypesCache.MainTypes[i] == typeof(SystemM))
                        continue;
                    SystemM sys = (SystemM)Activator.CreateInstance(TypesCache.MainTypes[i]);
                    systems.Add(sys);
                }
            }
            systems.Sort((x, y) =>
            {
                var ax = x.GetType().GetCustomAttributes(typeof(SystemExecuteOrderAttribute), true);
                var ay = y.GetType().GetCustomAttributes(typeof(SystemExecuteOrderAttribute), true);

                int ox = 0;
                int oy = 0;
                if (ax.Length > 0)
                    ox = ((SystemExecuteOrderAttribute)ax[0]).SortOrder;
                if (ay.Length > 0)
                    oy = ((SystemExecuteOrderAttribute)ay[0]).SortOrder;
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
