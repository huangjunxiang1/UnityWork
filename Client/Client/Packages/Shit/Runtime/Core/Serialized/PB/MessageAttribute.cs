using System;

namespace Main
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MessageAttribute : Attribute
    {
        public uint cmd { get; }
        public Type ResponseType { get; }

        public MessageAttribute(uint cmd)
        {
            this.cmd = cmd;
        }
        public MessageAttribute(uint cmd, Type ResponseType)
        {
            this.cmd = cmd;
            this.ResponseType = ResponseType;
        }
    }
}