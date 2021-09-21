using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum EventType
{
	AutoRigister,
	NoAutoRigister,
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class EventAttribute : Attribute
{
	public EventType EventType { get; }
	public Type ResponseType { get; }

	public EventAttribute(Type type) : this(EventType.AutoRigister, type)
	{

	}
	public EventAttribute(EventType eventType, Type type)
	{
		this.EventType = eventType;
		this.ResponseType = type;
	}
}