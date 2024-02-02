using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class EC_GameInit { }
public class EC_GameStart { }
public class EC_NetError
{
    public int code;
}
public class EC_QuitGame
{

}
public class EC_InScene
{
    public int sceneId;
    public int sceneType;
}
public class EC_OutScene
{
    public int sceneId;
    public int sceneType;
}
public class EC_ReceiveMessage
{
    public PB.PBMessage message;
}
public class EC_NewSObject<T> where T : SObject
{
    public EC_NewSObject(T o) => obj = o;
    public T obj { get; }
}
public class EC_DisposeSObject<T> where T : SObject
{
    public EC_DisposeSObject(T o) => obj = o;
    public T obj { get; }
}