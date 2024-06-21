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
                if (TouchHelper.isTouchUI != null && TouchHelper.isTouchUI()) return;
                if (target.World.ObjectManager.TryGetByGid(gid, out var o))
                    target.World.Event.RunGenericEventAndBaseType(typeof(EC_ClickSObject<>), o);
                else
                    Loger.Error("Not contains " + gid);
            }
        }

        Click click;

        static void EventWatcher(EventWatcher<EC_GameObjectReplace, ColliderClickComponent> t)
        {
            if (t.t.old && t.t.old.TryGetComponent<Click>(out var c)) GameObject.DestroyImmediate(c);
        }
        [Event]
        static void AnyChange(AnyChange<ColliderClickComponent, GameObjectComponent> t)
        {
            if (t.t2.gameObject)
            {
                t.t.click = t.t2.gameObject.GetComponent<Click>() ?? t.t2.gameObject.AddComponent<Click>();
                t.t.click.gid = t.t.gid;
                t.t.click.target = t.t;
            }
        }
        [Event]
        static void Out(Out<GameObjectComponent, ColliderClickComponent> t)
        {
            if (t.t2.click)
                GameObject.DestroyImmediate(t.t2.click);
        }
    }
}
