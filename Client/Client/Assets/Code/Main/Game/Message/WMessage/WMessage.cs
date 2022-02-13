using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main;

namespace Game
{
    public interface IWMessage
    {
        void Read(DBuffer buffer);
        void Write(DBuffer buffer);
    }
    public interface IWRequest : IWMessage
    {
        int RpcId { get; set; }
    }
    public interface IWResponse : IWMessage
    {
        int Error { get; set; }
        string Message { get; set; }
        int RpcId { get; set; }
    }

    public class Request : IWRequest
    {
        [Key(1)]
        public int RpcId { get; set; }

        public virtual void Read(DBuffer buffer)
        {

        }

        public virtual void Write(DBuffer buffer)
        {
           
        }
    }
    public class Response : IWResponse
    {
        [Key(1)]
        public int Error { get; set; }
        [Key(2)]
        public string Message { get; set; }
        [Key(3)]
        public int RpcId { get; set; }

        public virtual void Read(DBuffer buffer)
        {
            
        }

        public virtual void Write(DBuffer buffer)
        {
           
        }
    }
}
