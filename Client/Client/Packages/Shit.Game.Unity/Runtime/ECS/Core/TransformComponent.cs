using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public class TransformComponent : SComponent
    {
        Vector3 _p = default;
        Vector3 _s = Vector3.one;
        Quaternion _r = Quaternion.identity;

        public Vector3 position
        {
            get => _p;
            set
            {
                if (_p == value) return;
                _p = value;
                this.SetChange();
            }
        }
        public Vector3 scale
        {
            get => _s;
            set
            {
                if (_s == value) return;
                _s = value;
                this.SetChange();
            }
        }
        public Quaternion rotation
        {
            get => _r;
            set
            {
                if (_r == value) return;
                _r = value;
                this.SetChange();
            }
        }
        public Vector3 forward
        {
            get => _r * Vector3.forward;
            set => rotation = Quaternion.LookRotation(value);
        }
    }
}
