using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class MsgAttribute : Attribute
{
	public ushort OpCode { get; }//消息ID
	public int SortOrder { get; }//消息调用顺序权值

	public MsgAttribute(ushort opCode, int sortOrder = 0)
	{
		this.OpCode = opCode;
		this.SortOrder = sortOrder;
	}
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class MsgWithKeyAttribute : Attribute
{
	public ushort OpCode { get; }//消息ID
	public int SortOrder { get; }//消息调用顺序权值

	public MsgWithKeyAttribute(ushort opCode, int sortOrder = 0)
	{
		this.OpCode = opCode;
		this.SortOrder = sortOrder;
	}
}