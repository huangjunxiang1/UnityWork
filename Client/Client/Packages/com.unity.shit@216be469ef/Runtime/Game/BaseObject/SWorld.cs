using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public class SWorld : SGameObject
    {
        public SWorld() : base(0)
        {
            this.GameRoot.transform.position = Vector3.zero;
            this.GameRoot.transform.localScale = Vector3.one;
            this.GameRoot.transform.rotation = Quaternion.identity;
            GameObject.DontDestroyOnLoad(this.GameRoot);
#if UNITY_EDITOR
            this.Name = nameof(SWorld);
#endif
        }
    }
}
