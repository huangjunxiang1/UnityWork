using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;

namespace Game
{
    public class PlayingStepComponent : SComponent
    {
        struct LayerData
        {
            public bool isPlaying;
            public long _startTime;
            public long _endTime;
            public int _count;
        }
        LayerData[] layers = new LayerData[1]
        {
            new LayerData { _count = 1 },
        };
        int min = 1;

        public long GetStartTime(int layer = 0) => layers[layer]._startTime;
        public long GetEndTime(int layer = 0) => layers[layer]._endTime;
        public long GetCount(int layer = 0) => layers[layer]._count;

        public void Play(long startTime, long endTime, int count = 1, int layer = 0)
        {
            this.layers[layer].isPlaying = true;
            this.layers[layer]._startTime = startTime;
            this.layers[layer]._endTime = endTime;
            this.layers[layer]._count = count;
            this.Enable = true;
            this.SetChangeFlag();
            if (this.min <= layer)
                this.min = layer + 1;
        }
        public void Stop(int layer = 0)
        {
            this.layers[layer].isPlaying = false;
        }

        [Event]
        static void Change(Change<PlayingComponent, PlayingStepComponent> t)
        {
            int old = t.t2.layers.Length;
            if (old == t.t.Layer) 
                return;
            Array.Resize(ref t.t2.layers, t.t.Layer);
            for (int i = old; i < t.t2.layers.Length; i++)
                t.t2.layers[i] = new LayerData { _count = 1 };
        }
        [Event]
        static void Update(Update<PlayingStepComponent, PlayingComponent> t)
        {
            long utc = t.t.World.Timer.utc;
            for (int i = 0; i < t.t.min; i++)
            {
                var d = t.t.layers[i];
                if (!d.isPlaying)
                {
                    if (i == t.t.min - 1)
                    {
                        t.t.min--;
                        if (t.t.min == 0)
                            t.t.Enable = false;
                    }
                    continue;
                }
                t.t2.SetTime01(((utc - d._startTime) * d._count / (float)(d._endTime - d._startTime)) % 1, i);
                if (utc >= d._endTime)
                {
                    t.t.layers[i].isPlaying = false;
                    d._count = 1;
                }
            }
        }
    }
}
