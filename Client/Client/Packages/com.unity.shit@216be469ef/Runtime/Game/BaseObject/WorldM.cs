using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public class WorldM : WObjectM
    {
        public WorldM() : base(0)
        {
            this.goRoot.transform.position = Vector3.zero;
            this.goRoot.transform.localScale = Vector3.one;
            this.goRoot.transform.rotation = Quaternion.identity;
            GameObject.DontDestroyOnLoad(this.goRoot);
#if UNITY_EDITOR
            this.Name = "World";
#endif
        }
    }
}
