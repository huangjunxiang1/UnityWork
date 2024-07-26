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
        public static float DefaultRotateSpeed = 20;
        public float RotateSpeed = DefaultRotateSpeed;

        STask<bool> task;
        float3 start;
        float3 end;
        quaternion r;
        long startTime;
        long endTime;

        public void MoveTo(float3 start, float3 end, quaternion quaternion, long startTime, long endTime)
        {
            this.start = start;
            this.end = end;
            this.startTime = math.min(startTime, endTime - 1);
            this.endTime = endTime;
            this.r = quaternion;
            this.SetChange();
            var old = task;
            task = null;
            old?.TrySetResult(false);
        }
        public STask<bool> MoveToAsync(float3 start, float3 end, quaternion quaternion, long startTime, long endTime)
        {
            this.start = start;
            this.end = end;
            this.startTime = math.min(startTime, endTime - 1);
            this.endTime = endTime;
            this.r = quaternion;
            this.SetChange();
            var old = task;
            task = new();
            old?.TrySetResult(false);
            return task;
        }

        [Event]
        static void In(In<MoveToByUTCComponent, TransformComponent> t)
        {
            t.t.start = t.t.end = t.t2.position;
            t.t.r = t.t2.rotation;
            t.t.startTime = t.t.World.Timer.utc - 1;
            t.t.endTime = t.t.World.Timer.utc;
        }
        [Event]
        static void Update(Update<MoveToByUTCComponent, TransformComponent> t)
        {
            float lerp = (t.t.World.Timer.utc - t.t.startTime) / (float)(t.t.endTime - t.t.startTime);
            lerp = math.clamp(lerp, 0, 1);
            t.t2.position = math.lerp(t.t.start, t.t.end, lerp);
            if (lerp < 1)
            {
                var r = quaternion.LookRotation(math.normalize(t.t.end - t.t.start), math.up());
                t.t2.rotation = math.slerp(t.t2.rotation, r, math.clamp(t.t.World.DeltaTime * t.t.RotateSpeed, 0, 1));
            }
            else
            {
                t.t2.rotation = math.slerp(t.t2.rotation, t.t.r, math.clamp(t.t.World.DeltaTime * t.t.RotateSpeed, 0, 1));
                if (t.t.task != null)
                {
                    var old = t.t.task;
                    t.t.task = null;
                    old.TrySetResult(true);
                }
            }
        }
    }
}
