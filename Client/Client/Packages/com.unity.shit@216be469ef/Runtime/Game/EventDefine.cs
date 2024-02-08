using Game;

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
public class Awake<T>
{
    public Awake(T o) => target = o;
    public T target { get; }
}
public class Dispose<T> 
{
    public Dispose(T o) => target = o;
    public T target { get; }
}
public class Move<T>
{
    public Move(T o, SObject old)
    {
        this.target = o;
        this.old = old;
    }
    public T target { get; }
    public SObject old { get; }
}
public class Enable<T>
{
    public Enable(T o) => target = o;
    public T target { get; }
}
public abstract class __Change { }
public class Change<T1> : __Change
{
    public Change(T1 v1) => this.target = v1;
    public T1 target { get; }
}
public class Change<T1, T2> : __Change
{
    public Change(T1 t1, T2 t2)
    {
        this.target = t1;
        this.target2 = t2;
    }
    public T1 target { get; }
    public T2 target2 { get; }
}
public class Change<T1, T2, T3> : __Change
{
    public Change(T1 t1, T2 t2, T3 t3)
    {
        this.target = t1;
        this.target2 = t2;
        this.target3 = t3;
    }
    public T1 target { get; }
    public T2 target2 { get; }
    public T3 target3 { get; }
}
public class Change<T1, T2, T3, T4> : __Change
{
    public Change(T1 t1, T2 t2, T3 t3, T4 t4)
    {
        this.target = t1;
        this.target2 = t2;
        this.target3 = t3;
        this.target4 = t4;
    }
    public T1 target { get; }
    public T2 target2 { get; }
    public T3 target3 { get; }
    public T4 target4 { get; }
}
public class Change<T1, T2, T3, T4, T5> : __Change
{
    public Change(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
    {
        this.target = t1;
        this.target2 = t2;
        this.target3 = t3;
        this.target4 = t4;
        this.target5 = t5;
    }
    public T1 target { get; }
    public T2 target2 { get; }
    public T3 target3 { get; }
    public T4 target4 { get; }
    public T5 target5 { get; }
}