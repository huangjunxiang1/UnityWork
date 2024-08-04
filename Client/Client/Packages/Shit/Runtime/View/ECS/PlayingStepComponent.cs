using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class PlayingStepComponent : SComponent
    {
        long _startTime;
        long _endTime;
        int _count = 1;

        public long startTime
        {
            get => _startTime;
            set
            {
                if (_startTime == value) return;
                _startTime = value;
                this.Enable = true;
                this.SetChange();
            }
        }
        public long endTime
        {
            get => _endTime;
            set
            {
                if (_endTime == value) return;
                _endTime = value;
                this.Enable = true;
                this.SetChange();
            }
        }
        public int count
        {
            get => _count;
            set
            {
                if (_count == value) return;
                _count = value;
                this.Enable = true;
                this.SetChange();
            }
        }

        [Event]
        static void Update(Update<PlayingStepComponent, PlayingComponent> t)
        {
            long utc = t.t.World.Timer.utc;
            t.t2.time01 = (((utc - t.t.startTime) * t.t.count / (float)(t.t.endTime - t.t.startTime))) % 1;
            if (utc >= t.t.endTime)
            {
                t.t.Enable = false;
                t.t._count = 1;
            }
        }
    }
}
