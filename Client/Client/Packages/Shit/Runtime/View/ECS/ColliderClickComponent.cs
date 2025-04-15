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
        class Click : MonoBehaviour, IPointerClickHandler, IPointerDownHandler
        {
            public long gid;
            public ColliderClickComponent target;

            public void OnPointerDown(PointerEventData eventData)
            {
                if (UIGlobalConfig.isTouchUI != null && UIGlobalConfig.isTouchUI()) return;
                if (target.World.ObjectManager.TryGetByGid(gid, out var o))
                {
                    var os = ArrayCache.Get<object>(2);
                    os[0] = o;
                    os[1] = eventData;
                    target.World.Event.RunGenericEventAndBaseType(typeof(EC_PointerDown<>), os);
                }
                else
                    Loger.Error("Not contains " + gid);
            }

            public void OnPointerClick(PointerEventData eventData)
            {
                if (UIGlobalConfig.isTouchUI != null && UIGlobalConfig.isTouchUI()) return;
                if (target.World.ObjectManager.TryGetByGid(gid, out var o))
                {
                    var os = ArrayCache.Get<object>(2);
                    os[0] = o;
                    os[1] = eventData;
                    target.World.Event.RunGenericEventAndBaseType(typeof(EC_PointerClick<>), os);
                }
                else
                    Loger.Error("Not contains " + gid);
            }
        }

        Click click;

        [EventWatcherSystem]
        static void EventWatcher(EC_GameObjectReplace a, ColliderClickComponent b)
        {
            if (a.old && a.old.TryGetComponent<Click>(out var c)) GameObject.DestroyImmediate(c);
        }
        [AnyChangeSystem]
        static void AnyChange(ColliderClickComponent a, GameObjectComponent b)
        {
            if (b.gameObject)
            {
                a.click = b.gameObject.GetComponent<Click>() ?? b.gameObject.AddComponent<Click>();
                a.click.gid = a.gid;
                a.click.target = a;
            }
        }
        [OutSystem]
        static void Out(GameObjectComponent a, ColliderClickComponent b)
        {
            if (b.click)
                GameObject.DestroyImmediate(b.click);
        }
    }
}
