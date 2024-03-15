using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public class SWorld : World
    {
        public SWorld() : base(1)
        {
            this.gameObject = new(nameof(SWorld));
            this.transform = this.gameObject.transform;
            if (Application.isPlaying)
                UnityEngine.Object.DontDestroyOnLoad(this.gameObject);
        }
        public GameObject gameObject { get; }
        public Transform transform { get; }
    }
}
