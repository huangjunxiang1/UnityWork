using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using Unity.Mathematics.Geometry;

namespace Game
{
    public class TransformComponent : SComponent
    {
        [Sirenix.OdinInspector.ShowInInspector]
        float3 _p = 0;

        [Sirenix.OdinInspector.ShowInInspector]
        float3 _s = 1;

        [Sirenix.OdinInspector.ShowInInspector]
        quaternion _r = quaternion.identity;

        [Sirenix.OdinInspector.ShowInInspector]
        MinMaxAABB _aabb = new(float.MinValue, float.MaxValue);

        public float3 position
        {
            get => _p;
            set
            {
                value = math.clamp(value, _aabb.Min, _aabb.Max);
                if (math.all(_p == value)) return;
                _p = value;
                this.SetChange();
            }
        }
        public float3 scale
        {
            get => _s;
            set
            {
                if (math.all(_s == value)) return;
                _s = value;
                this.SetChange();
            }
        }
        public quaternion rotation
        {
            get => _r;
            set
            {
                if (math.all(_r.value == value.value)) return;
                _r = value;
                this.SetChange();
            }
        }
        public float3 forward
        {
            get => math.mul(_r, math.forward());
            set => rotation = quaternion.LookRotation(value, math.up());
        }
        public MinMaxAABB AABB
        {
            get => _aabb;
            set
            {
                if (_aabb.Equals(value)) return;
                _aabb = value;
                _p = math.clamp(_p, _aabb.Min, _aabb.Max);
                this.SetChange();
            }
        }

        public void ResetAABB() => AABB = new(float.MinValue, float.MaxValue);
    }
}
