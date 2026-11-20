using cfg;
using System;
using System.Collections.Generic;
using System.Text;

public static class TabL
{
    public static Tables Data { get; private set; }

    public static void Init(Tables tables) => Data = tables;
}