using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Main;

namespace Game
{
    public class WUnit : WObject
    {
        public WUnit(long unitId) : base(unitId)
        {
           
        }

        public Animator Animator { get; private set; }
        public float Speed { get; set; } = 1;

        public override void SetRes(GameObject res)
        {
            base.SetRes(res);
            this.Animator = res.GetComponent<Animator>();
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
