using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class MsgAttribute : Attribute
{
	public ushort OpCode { get; }//消息ID
	public int SortOrder { get; }//消息调用顺序权值

	public MsgAttribute(ushort opCode)
	{
		this.OpCode = opCode;
	}
	public MsgAttribute(ushort opCode, int sortOrder)
	{
		this.OpCode = opCode;
		this.SortOrder = sortOrder;
	}
}
