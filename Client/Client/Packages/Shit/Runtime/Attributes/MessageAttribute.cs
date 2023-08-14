using System;

namespace Main
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MessageAttribute : Attribute
    {
        public uint cmd { get; }
        public Type ResponseType { get; }

        public MessageAttribute(ushort mainCmd, ushort subCmd)
        {
            cmd = (uint)(mainCmd | subCmd << 16);
        }
        public MessageAttribute(ushort mainCmd, ushort subCmd, Type ResponseType)
        {
            cmd = (uint)(mainCmd | subCmd << 16);
            this.ResponseType = ResponseType;
        }
    }
}