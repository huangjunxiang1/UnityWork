using UnityEngine;

public class Language
{
    /// <summary>
    /// 语言包ID
    /// </summary>
    public int id { get; }
    /// <summary>
    /// 中文
    /// </summary>
    public string cn { get; }
    /// <summary>
    /// 英语
    /// </summary>
    public string en { get; }

    public Language(DBuffer buffer)
    {
        this.id = buffer.ReadInt();
        this.cn = buffer.ReadString();
        this.en = buffer.ReadString();
    }
}
