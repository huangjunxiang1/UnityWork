using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public class WRoot : WObject
    {
        private WRoot(GameObject gameObject) : base((int)ObjectType.WRoot, gameObject)
        {

        }

        static WRoot _inst;
        public static WRoot Inst
        {
            get
            {
                if (_inst == null)
                {
                    GameObject go = GameObject.Find("WRoot");
                    if (!go) go = new GameObject("WRoot");

                    go.transform.position = Vector3.zero;
                    go.transform.localScale = Vector3.one;
                    go.transform.rotation = Quaternion.identity;
                    GameObject.DontDestroyOnLoad(go);
                    _inst = new WRoot(go);
                }
                return _inst;
            }
        }

        public override void AddChild(WObject child)
        {
            base.AddChild(child);
            child.GameObject.transform.SetParent(this.GameObject.transform);
        }
        public override void AddChildAt(WObject child, int index)
        {
            base.AddChildAt(child, index);
            child.GameObject.transform.SetParent(this.GameObject.transform);
        }
        public override void Remove(long id)
        {
            WObject child = GetChild(id);
            base.Remove(id);
            child.GameObject.transform.SetParent(this.GameObject.transform);
        }
    }
}
