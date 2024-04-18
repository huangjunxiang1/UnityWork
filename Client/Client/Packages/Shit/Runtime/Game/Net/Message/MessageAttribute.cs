using System;

namespace Game
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MessageAttribute : Attribute
    {
        public int cmd { get; }
        public Type ResponseType { get; }

        public MessageAttribute(int cmd)
        {
            this.cmd = cmd;
        }
        public MessageAttribute(int cmd, Type ResponseType)
        {
            this.cmd = cmd;
            this.ResponseType = ResponseType;
        }
    }
}