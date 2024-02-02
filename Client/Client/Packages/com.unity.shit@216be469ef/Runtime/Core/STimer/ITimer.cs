using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface ITimer
{
    public bool TimerEnable { get; set; }
    bool RigisterTimer() => STimer.RigisterTimer(this);
    void RemoveTimer()
    {
        if (this is not IDispose)
            STimer.RemoveTimer(this);
        else
            STimer.SetRemovedTimerFlag();
    }
}
