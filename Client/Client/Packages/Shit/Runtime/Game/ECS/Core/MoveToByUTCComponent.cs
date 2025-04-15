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

        [InSystem]
        static void In(MoveToByUTCComponent a, TransformComponent b)
        {
            a._isMoving = false;
            a._start = a._end = b.position;
            a.rotation = b.rotation;
            a._startTime = a.World.Timer.utc - 1;
            a._endTime = a.World.Timer.utc;
        }
        [UpdateSystem]
        static void Update(MoveToByUTCComponent a, TransformComponent b)
        {
            if (!a._isMoving) return;
            float lerp = (a.World.Timer.utc - a._startTime) / (float)(a._endTime - a._startTime);
            lerp = math.clamp(lerp, 0, 1);
            b.position = math.lerp(a._start, a._end, lerp);
            if (lerp < 1)
            {
                var r = quaternion.LookRotation(math.normalize(a._end - a._start), math.up());
                b.rotation = math.slerp(b.rotation, r, math.clamp(a.World.DeltaTime * a.RotateSpeed, 0, 1));
            }
            else
            {
                b.rotation = math.slerp(b.rotation, a._r, math.clamp(a.World.DeltaTime * a.RotateSpeed, 0, 1));
                if (a._task != null)
                {
                    var old = a._task;
                    a._task = null;
                    a._isMoving = true;
                    old.TrySetResult(true);
                }
            }
        }
    }
}
