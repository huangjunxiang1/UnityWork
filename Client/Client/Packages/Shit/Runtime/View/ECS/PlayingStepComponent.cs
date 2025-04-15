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
            this.layers[layer]._startTime = math.min(startTime, endTime - 1);
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

        [ChangeSystem]
        static void Change(PlayingComponent a, PlayingStepComponent b)
        {
            int old = b.layers.Length;
            if (old == a.Layer) 
                return;
            Array.Resize(ref b.layers, a.Layer);
            for (int i = old; i < b.layers.Length; i++)
                b.layers[i] = new LayerData { _count = 1 };
        }
        [UpdateSystem]
        static void Update(PlayingStepComponent a, PlayingComponent b)
        {
            long utc = a.World.Timer.utc;
            for (int i = 0; i < a.min; i++)
            {
                var d = a.layers[i];
                if (!d.isPlaying)
                {
                    if (i == a.min - 1)
                    {
                        a.min--;
                        if (a.min == 0)
                            a.Enable = false;
                    }
                    continue;
                }
                b.SetTime01(((utc - d._startTime) * d._count / (float)(d._endTime - d._startTime)) % 1, i);
                if (utc >= d._endTime)
                {
                    a.layers[i].isPlaying = false;
                    d._count = 1;
                }
            }
        }
    }
}
