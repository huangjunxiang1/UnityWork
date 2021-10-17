using Main;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[ResponseType(typeof(R2C_Login))]
[Message(CMDL.C2R_Login)]
[ProtoContract]
public partial class C2R_Login : IRequest
{
	[ProtoMember(90)]
	public int RpcId { get; set; }

	[ProtoMember(1)]
	public string Account { get; set; }

	[ProtoMember(2)]
	public string Password { get; set; }

}

[Message(CMDL.R2C_Login)]
[ProtoContract]
public partial class R2C_Login : IResponse
{
	[ProtoMember(90)]
	public int RpcId { get; set; }

	[ProtoMember(91)]
	public int Error { get; set; }

	[ProtoMember(92)]
	public string Message { get; set; }

	[ProtoMember(1)]
	public string Address { get; set; }

	[ProtoMember(2)]
	public long Key { get; set; }

	[ProtoMember(3)]
	public long GateId { get; set; }

}

[ResponseType(typeof(G2C_LoginGate))]
[Message(CMDL.C2G_LoginGate)]
[ProtoContract]
public partial class C2G_LoginGate : IRequest
{
	[ProtoMember(90)]
	public int RpcId { get; set; }

	[ProtoMember(1)]
	public long Key { get; set; }

	[ProtoMember(2)]
	public long GateId { get; set; }

}

[Message(CMDL.G2C_LoginGate)]
[ProtoContract]
public partial class G2C_LoginGate : IResponse
{
	[ProtoMember(90)]
	public int RpcId { get; set; }

	[ProtoMember(91)]
	public int Error { get; set; }

	[ProtoMember(92)]
	public string Message { get; set; }

	[ProtoMember(1)]
	public long PlayerId { get; set; }

}
