using UnityEngine;

public class Scene
{
    /// <summary>
    /// id
    /// </summary>
    public int id { get; }
    /// <summary>
    /// xxxx
    /// </summary>
    public string name { get; }

    public Scene(WBuffer buffer)
    {
        this.id = buffer.ReadInt();
        this.name = buffer.ReadString();
    }
}
