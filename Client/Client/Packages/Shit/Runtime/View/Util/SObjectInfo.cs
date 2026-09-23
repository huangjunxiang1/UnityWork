using Event;
using System.Collections;
using UnityEngine;

public class SObjectInfo : MonoBehaviour
{
    public long gid;

    /// <summary>
    /// animation event call
    /// </summary>
    /// <param name="param"></param>
    void Event(string param)
    {
        var e = new EC_AnimationEvent { param = param };
        Game.Event.RunEvent(e, gid: gid);
    }
}