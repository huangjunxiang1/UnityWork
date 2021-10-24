using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class InputS
{
    static InputS()
    {
        Timer.Add(0, -1, update);
    }

    public static Eventer OnMouseButtonDown { get; } = new Eventer();
    public static Eventer OnMouseButtonUp { get; } = new Eventer();
    public static Eventer OnMouseButton { get; } = new Eventer();

    public static Eventer OnKeyDown { get; } = new Eventer();
    public static Eventer OnKeyUp { get; } = new Eventer();
    public static Eventer OnKey { get; } = new Eventer();

    public static Eventer OnAnyDown { get; } = new Eventer();
    public static Eventer anyKey { get; } = new Eventer();

    static void update()
    {
        for (int i = 0; i <= 2; i++)
        {
            if (Input.GetMouseButtonDown(i))
            {
                OnMouseButtonDown.Call(i);
            }
            if (Input.GetMouseButtonUp(0))
                OnMouseButtonDown.Call();
            if (Input.GetMouseButtonDown(0))
                OnMouseButtonDown.Call();
        }
       
    }
}
