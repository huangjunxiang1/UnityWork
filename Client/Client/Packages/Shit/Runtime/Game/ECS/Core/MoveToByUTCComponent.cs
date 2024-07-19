using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;

namespace Game
{
    public class MoveToByUTCComponent : SComponent
    {
        STask task;
        float3 start;
        float3 end;
        long startTime;
        long endTime;

        public void MoveTo(float3 start, float3 end, long startTime, long endTime)
        {
            this.start = start;
            this.end = end;
            this.startTime = math.min(startTime, endTime - 1);
            this.endTime = endTime;
            this.SetChange();
            task = null;
        }
        public STask MoveToAsync(float3 start, float3 end, long startTime, long endTime)
        {
            this.start = start;
            this.end = end;
            this.startTime = math.min(startTime, endTime - 1);
            this.endTime = endTime;
            this.SetChange();
            return task ??= new();
        }

        [Event]
        static void In(In<MoveToByUTCComponent, TransformComponent> t)
        {
            t.t.start = t.t.end = t.t2.position;
            t.t.startTime = t.t.World.Timer.utc - 1;
            t.t.endTime = t.t.World.Timer.utc;
        }
        [Event]
        static void Update(Update<MoveToByUTCComponent, TransformComponent> t)
        {
            float lerp = (t.t.World.Timer.utc - t.t.startTime) / (float)(t.t.endTime - t.t.startTime);
            lerp = math.clamp(lerp, 0, 1);
            t.t2.position = math.lerp(t.t.start, t.t.end, lerp);
            if (t.t.task != null && lerp >= 1)
            {
                var task = t.t.task;
                t.t.task = null;
                task.TrySetResult();
            }
        }
    }
}
