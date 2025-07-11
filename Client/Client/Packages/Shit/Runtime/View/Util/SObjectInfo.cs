using Event;
using Game;
using System.Collections;
using UnityEngine;

public class SObjectInfo : MonoBehaviour
{
    public long gid;
    public long actorId;

    /// <summary>
    /// animation event call
    /// </summary>
    /// <param name="param"></param>
    void Event(string param)
    {
        var e = new EC_AnimationEvent { param = param };
        if (actorId != 0)
            Client.World.Event.RunEvent(e, actorId: actorId);
        else
            Client.World.Event.RunEvent(e, gid: gid);
    }
}