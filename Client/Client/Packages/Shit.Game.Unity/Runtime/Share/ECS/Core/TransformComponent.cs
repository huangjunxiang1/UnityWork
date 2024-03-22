using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;

namespace Game
{
    public class TransformComponent : SComponent
    {
        float3 _p = 0;
        float3 _s = 1;
        quaternion _r = quaternion.identity;

        public float3 position
        {
            get => _p;
            set
            {
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
    }
}
