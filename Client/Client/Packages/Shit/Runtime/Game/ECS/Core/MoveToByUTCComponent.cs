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

        STask<bool> _task;
        float3 _start;
        float3 _end;
        quaternion _r;
        long _startTime;
        long _endTime;
        bool _isMoving = false;

        public quaternion rotation
        {
            get => _r;
            set
            {
                if (value.Equals(_r)) return;
                _r = value;
                this.SetChangeFlag();
            }
        }

        public void MoveTo(float3 start, float3 end, quaternion quaternion, long startTime, long endTime)
        {
            this._isMoving = true;
            this._start = start;
            this._end = end;
            this._startTime = math.min(startTime, endTime - 1);
            this._endTime = endTime;
            this._r = quaternion;
            this.SetChangeFlag();
            var old = _task;
            _task = null;
            old?.TrySetResult(false);
        }
        public void MoveTo(float3 start, float3 end, long startTime, long endTime)
        {
            this._isMoving = true;
            this._start = start;
            this._end = end;
            this._startTime = math.min(startTime, endTime - 1);
            this._endTime = endTime;
            this._r = quaternion.LookRotation(math.normalize(end - start), math.up());
            this.SetChangeFlag();
            var old = _task;
            _task = null;
            old?.TrySetResult(false);
        }
        public STask<bool> MoveToAsync(float3 start, float3 end, quaternion quaternion, long startTime, long endTime)
        {
            this._isMoving = true;
            this._start = start;
            this._end = end;
            this._startTime = math.min(startTime, endTime - 1);
            this._endTime = endTime;
            this._r = quaternion;
            this.SetChangeFlag();
            var old = _task;
            _task = new();
            old?.TrySetResult(false);
            return _task;
        }
        public STask<bool> MoveToAsync(float3 start, float3 end, long startTime, long endTime)
        {
            this._isMoving = true;
            this._start = start;
            this._end = end;
            this._startTime = math.min(startTime, endTime - 1);
            this._endTime = endTime;
            this._r = quaternion.LookRotation(math.normalize(end - start), math.up());
            this.SetChangeFlag();
            var old = _task;
            _task = new();
            old?.TrySetResult(false);
            return _task;
        }

        [Event]
        static void In(In<MoveToByUTCComponent, TransformComponent> t)
        {
            t.t._isMoving = false;
            t.t._start = t.t._end = t.t2.position;
            t.t.rotation = t.t2.rotation;
            t.t._startTime = t.t.World.Timer.utc - 1;
            t.t._endTime = t.t.World.Timer.utc;
        }
        [Event]
        static void Update(Update<MoveToByUTCComponent, TransformComponent> t)
        {
            if (!t.t._isMoving) return;
            float lerp = (t.t.World.Timer.utc - t.t._startTime) / (float)(t.t._endTime - t.t._startTime);
            lerp = math.clamp(lerp, 0, 1);
            t.t2.position = math.lerp(t.t._start, t.t._end, lerp);
            if (lerp < 1)
            {
                var r = quaternion.LookRotation(math.normalize(t.t._end - t.t._start), math.up());
                t.t2.rotation = math.slerp(t.t2.rotation, r, math.clamp(t.t.World.DeltaTime * t.t.RotateSpeed, 0, 1));
            }
            else
            {
                t.t2.rotation = math.slerp(t.t2.rotation, t.t._r, math.clamp(t.t.World.DeltaTime * t.t.RotateSpeed, 0, 1));
                if (t.t._task != null)
                {
                    var old = t.t._task;
                    t.t._task = null;
                    t.t._isMoving = true;
                    old.TrySetResult(true);
                }
            }
        }
    }
}
