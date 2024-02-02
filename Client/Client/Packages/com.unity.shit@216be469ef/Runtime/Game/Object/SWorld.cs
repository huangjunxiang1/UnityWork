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
        public SWorld() : base()
        {
            this.GameRoot.transform.position = Vector3.zero;
            this.GameRoot.transform.localScale = Vector3.one;
            this.GameRoot.transform.rotation = Quaternion.identity;
            GameObject.DontDestroyOnLoad(this.GameRoot);
        }
        public override void SetGameObject(GameObject res, bool release = true)
        {
            throw new Exception("Error OP SWorld");
        }
        public override void Dispose()
        {
            throw new Exception("Error OP SWorld");
        }
    }
}
