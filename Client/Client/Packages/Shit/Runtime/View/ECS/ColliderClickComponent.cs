using Core;
using Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class ColliderClickComponent : SComponent
    {
        class Click : MonoBehaviour, IPointerClickHandler
        {
            public long gid;
            public ColliderClickComponent target;

            public void OnPointerClick(PointerEventData eventData)
            {
                if (target == null || !target.Enable) return;
                if (target.World.ObjectManager.TryGetByGid(gid, out var o))
                    target.World.Event.RunEvent(new EC_ClickObject { obj = o });
                else
                    Loger.Error("不存在的对象 " + gid);
            }
        }

        Click click;

        [Event]
        static void AnyChange(AnyChange<ColliderClickComponent, GameObjectComponent> t)
        {
            if (t.t2.gameObject)
            {
                t.t.click = t.t2.gameObject.GetComponent<Click>() ?? t.t2.gameObject.AddComponent<Click>();
                t.t.click.gid = t.t.gid;
                t.t.click.target = t.t;
            }
            t.t2.Replace -= Replace;
            t.t2.Replace += Replace;
        }
        [Event]
        static void Out(Out<GameObjectComponent, ColliderClickComponent> t)
        {
            if (t.t2.click)
                t.t2.click.target = null;
        }

        static void Replace(GameObject a, GameObject b)
        {
            var click = a?.GetComponent<Click>();
            if (click)
                GameObject.DestroyImmediate(click);
        }
    }
}
