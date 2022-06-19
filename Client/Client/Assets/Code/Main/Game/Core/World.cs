using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public class World : WObject
    {
        public World() : base((int)WObjectType.World)
        {
            this.Root.transform.position = Vector3.zero;
            this.Root.transform.localScale = Vector3.one;
            this.Root.transform.rotation = Quaternion.identity;
            GameObject.DontDestroyOnLoad(this.Root);
#if UNITY_EDITOR
            this.Name = "World";
#endif
        }

        public override void AddChild(WObject child)
        {
            base.AddChild(child);
            child.Root.transform.SetParent(this.Root.transform);
        }
        public override void Remove(long id)
        {
            WObject child = GetChild(id);
            base.Remove(id);
            child.Root.transform.parent = null;
        }
    }
}
