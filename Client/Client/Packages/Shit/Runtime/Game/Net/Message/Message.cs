using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class Message : ProtoBuf.IExtensible
{
    public long rpc { get; set; }
    public long actorId { get; set; }
    public string error { get; set; }

    private IExtension __pbn__extensionData;
    public IExtension GetExtensionObject(bool createIfMissing) => Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);
}