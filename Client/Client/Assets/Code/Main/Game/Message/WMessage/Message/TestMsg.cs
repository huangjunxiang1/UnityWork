using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Main;

namespace Game
{
    [Message(0)]
    [ResponseType(typeof(TestRep))]
    public partial class TestReq
    {
        [Key(2)]
        public List<Item> items;
    }
    public partial class TestReq : Request
    {
        public override void Read(WBuffer buffer)
        {
            int key;
            while ((key = buffer.ReadInt()) != 0)
            {
                if (key == 1)
                    this.RpcId = buffer.ReadInt();
                if (key == 2)
                {
                    int len = buffer.ReadInt();
                    items = new List<Item>(len);
                    for (int i = 0; i < len; i++)
                    {
                        Item t = new Item();
                        t.Read(buffer);
                        items.Add(t);
                    }
                }
            }
        }

        public override void Write(WBuffer buffer)
        {
            if (RpcId != default)
            {
                buffer.Write(1);
                buffer.Write(RpcId);
            }
            if (items != default)
            {
                buffer.Write(2);
                int len = items.Count;
                buffer.Write(len);
                for (int i = 0; i < len; i++)
                    items[i].Write(buffer);
            }
            buffer.Write(0);
        }
    }

    [Message(1)]
    public partial class TestRep
    {

    }
    public partial class TestRep : Response
    {
        public override void Read(WBuffer buffer)
        {
            int key;
            while ((key = buffer.ReadInt()) != 0)
            {
                if (key == 1)
                    this.Error = buffer.ReadInt();
                if (key == 2)
                    this.Message = buffer.ReadString();
                if (key == 3)
                    this.RpcId = buffer.ReadInt();
            }
        }

        public override void Write(WBuffer buffer)
        {
            if (this.Error != default)
            {
                buffer.Write(1);
                buffer.Write(Error);
            }
            if (this.Message != default)
            {
                buffer.Write(2);
                buffer.Write(Message);
            }
            if (this.RpcId != default)
            {
                buffer.Write(3);
                buffer.Write(RpcId);
            }
            buffer.Write(0);
        }
    }


    public class Item : IWMessage
    {
        [Key(1)]
        public int test;
        public void Read(WBuffer buffer)
        {
            int key;
            while ((key = buffer.ReadInt()) != 0)
            {
                if (key == 1)
                    this.test = buffer.ReadInt();
            }
        }

        public void Write(WBuffer buffer)
        {
            if (this.test != default)
            {
                buffer.Write(1);
                buffer.Write(this.test);
            }
            buffer.Write(0);
        }
    }

}
