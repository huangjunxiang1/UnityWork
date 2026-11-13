using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using ProtoBuf; // 必须引用 protobuf-net

// 使用你生成的命名空间
using Main;

namespace ComprehensiveExample.Tests
{
    public class AllTypesProtocolTests
    {
        [Test]
        public void Test_AllTypesMessage_FullCoverage()
        {
            // ==========================================
            // 1. 实例化并填充数据
            // ==========================================
            var original = new AllTypesMessage
            {
                // --- 基础标量 ---
                DoubleF = 3.14159,
                FloatF = 1.414f,
                Int32F = 42,
                Int64F = 123456789012345L,
                Uint32F = 4294967295,
                Uint64F = 18446744073709551615UL,
                Sint32F = -12345,
                Sint64F = -123456789012345L,
                Fixed32F = 400000000,
                Fixed64F = 1800000000000000000L,
                Sfixed32F = -300000000,
                Sfixed64F = -9000000000000000000L,
                BoolF = true,
                StringF = "Hello Protobuf",
                BytesF = new byte[] { 0x01, 0x02, 0x03 },

                // --- Optional 可选字段 ---
                OptInt32 = 100,
                OptString = "Optional String",
                OptBytes = new byte[] { 0xFF, 0xEE },
                OptBool = false,
                OptEnum = Status.StatusActive,
                OptMsg = new ChildMessage { Id = 1, Name = "Opt Child" },

                // --- Repeated 数值类型（生成的是数组，直接赋值） ---
                RepDoubles = new double[] { 1.1, 2.2 },
                RepFloats = new float[] { 3.3f, 4.4f },
                RepInt32s = new int[] { 10, 20 },
                RepUint64s = new ulong[] { 30UL, 40UL },
                RepBools = new bool[] { true, false },

                // --- 嵌套与递归 ---
                NestedData = new AllTypesMessage.InnerNested
                {
                    DeepId = 100,
                    // 注意：生成的属性名是小写的 deepest
                    deepest = new AllTypesMessage.InnerNested.Deepest { Data = "Nested Deep Data" }
                },
                TreeRoot = new TreeNode
                {
                    Id = 1,
                    Left = new TreeNode { Id = 2 },
                    Right = new TreeNode { Id = 3, Right = new TreeNode { Id = 4 } }
                },
                EmptyData = new Empty()
            };

            // --- Repeated 引用类型（生成的是只读 List，需要 Add） ---
            original.RepStrings.AddRange(new[] { "A", "B" });
            original.RepBytes.Add(new byte[] { 0xAA });
            original.RepBytes.Add(new byte[] { 0xBB });
            original.RepEnums.Add(Status.StatusActive);
            original.RepEnums.Add(Status.StatusInactive);
            original.RepMsgs.Add(new ChildMessage { Id = 2, Name = "Child A" });
            original.RepMsgs.Add(new ChildMessage { Id = 3, Name = "Child B" });

            // --- Map 字典（生成的是只读 Dictionary，需要 Add） ---
            original.MapKI32s.Add(1, "v1");
            original.MapKI64s.Add(2L, 2.2);
            original.MapKU32s.Add(3U, 3.3f);
            original.MapKU64s.Add(4UL, true);
            original.MapKSi32s.Add(-5, 5);
            original.MapKSi64s.Add(-6L, 6L);
            original.MapKF32s.Add(7U, 7U);
            original.MapKF64s.Add(8UL, 8UL);
            original.MapKSf32s.Add(-9, -9);
            original.MapKSf64s.Add(-10L, -10L);
            original.MapKBools.Add(true, "bool key");
            original.MapKStrs.Add("k_str", new byte[] { 0x11, 0x22 });
            original.MapVEnums.Add("enum_key", Status.StatusInactive);
            original.MapVMsgs.Add("msg_key", new ChildMessage { Id = 4, Name = "Map Child" });

            // --- OneOf 联合体（最后赋值生效） ---
            original.UnionStr = "I am a string union";


            // ==========================================
            // 2. 序列化与反序列化
            // ==========================================
            var deserialized = CloneViaProtobuf(original);


            // ==========================================
            // 3. 断言与验证
            // ==========================================

            // --- 基础标量 ---
            Assert.AreEqual(original.DoubleF, deserialized.DoubleF);
            Assert.AreEqual(original.FloatF, deserialized.FloatF);
            Assert.AreEqual(original.Int32F, deserialized.Int32F);
            Assert.AreEqual(original.Int64F, deserialized.Int64F);
            Assert.AreEqual(original.Uint32F, deserialized.Uint32F);
            Assert.AreEqual(original.Uint64F, deserialized.Uint64F);
            Assert.AreEqual(original.Sint32F, deserialized.Sint32F);
            Assert.AreEqual(original.Sint64F, deserialized.Sint64F);
            Assert.AreEqual(original.Fixed32F, deserialized.Fixed32F);
            Assert.AreEqual(original.Fixed64F, deserialized.Fixed64F);
            Assert.AreEqual(original.Sfixed32F, deserialized.Sfixed32F);
            Assert.AreEqual(original.Sfixed64F, deserialized.Sfixed64F);
            Assert.AreEqual(original.BoolF, deserialized.BoolF);
            Assert.AreEqual(original.StringF, deserialized.StringF);
            CollectionAssert.AreEqual(original.BytesF, deserialized.BytesF);

            // --- Optional 字段 ---
            Assert.IsTrue(deserialized.ShouldSerializeOptInt32());
            Assert.AreEqual(original.OptInt32, deserialized.OptInt32);
            Assert.IsTrue(deserialized.ShouldSerializeOptString());
            Assert.AreEqual(original.OptString, deserialized.OptString);
            CollectionAssert.AreEqual(original.OptBytes, deserialized.OptBytes);
            Assert.IsTrue(deserialized.ShouldSerializeOptBool());
            Assert.AreEqual(original.OptBool, deserialized.OptBool);
            Assert.IsTrue(deserialized.ShouldSerializeOptEnum());
            Assert.AreEqual(original.OptEnum, deserialized.OptEnum);
            Assert.AreEqual(original.OptMsg.Name, deserialized.OptMsg.Name);

            // --- 数组 (Repeated 数值) ---
            CollectionAssert.AreEqual(original.RepDoubles, deserialized.RepDoubles);
            CollectionAssert.AreEqual(original.RepFloats, deserialized.RepFloats);
            CollectionAssert.AreEqual(original.RepInt32s, deserialized.RepInt32s);
            CollectionAssert.AreEqual(original.RepUint64s, deserialized.RepUint64s);
            CollectionAssert.AreEqual(original.RepBools, deserialized.RepBools);

            // --- 列表 (Repeated 引用) ---
            CollectionAssert.AreEqual(original.RepStrings, deserialized.RepStrings);
            CollectionAssert.AreEqual(original.RepEnums, deserialized.RepEnums);
            Assert.AreEqual(2, deserialized.RepBytes.Count);
            CollectionAssert.AreEqual(original.RepBytes[0], deserialized.RepBytes[0]);
            Assert.AreEqual(2, deserialized.RepMsgs.Count);
            Assert.AreEqual("Child B", deserialized.RepMsgs[1].Name);

            // --- Map 字典 ---
            Assert.AreEqual("v1", deserialized.MapKI32s[1]);
            Assert.AreEqual(2.2, deserialized.MapKI64s[2L]);
            Assert.AreEqual(true, deserialized.MapKU64s[4UL]);
            Assert.AreEqual("bool key", deserialized.MapKBools[true]);
            CollectionAssert.AreEqual(new byte[] { 0x11, 0x22 }, deserialized.MapKStrs["k_str"]);
            Assert.AreEqual(Status.StatusInactive, deserialized.MapVEnums["enum_key"]);
            Assert.AreEqual("Map Child", deserialized.MapVMsgs["msg_key"].Name);

            // --- OneOf 联合体 ---
            Assert.IsTrue(deserialized.ShouldSerializeUnionStr());
            Assert.AreEqual("I am a string union", deserialized.UnionStr);
            Assert.IsFalse(deserialized.ShouldSerializeUnionInt()); // 验证其他字段未被设置

            // 验证重新赋值 OneOf 会清除前一个值
            original.UnionInt = 999;
            var deserialized2 = CloneViaProtobuf(original);
            Assert.IsTrue(deserialized2.ShouldSerializeUnionInt());
            Assert.AreEqual(999, deserialized2.UnionInt);
            Assert.IsFalse(deserialized2.ShouldSerializeUnionStr());
            Assert.AreEqual("", deserialized2.UnionStr); // 默认值是空字符串

            // --- 嵌套、递归 ---
            Assert.AreEqual(100, deserialized.NestedData.DeepId);
            Assert.AreEqual("Nested Deep Data", deserialized.NestedData.deepest.Data);
            Assert.AreEqual(1, deserialized.TreeRoot.Id);
            Assert.AreEqual(2, deserialized.TreeRoot.Left.Id);
            Assert.AreEqual(4, deserialized.TreeRoot.Right.Right.Id);
            Assert.IsNotNull(deserialized.EmptyData);
        }

        // 辅助序列化方法
        private T CloneViaProtobuf<T>(T obj) where T : class
        {
            using (var ms = new MemoryStream())
            {
                Serializer.Serialize(ms, obj);
                ms.Position = 0;
                return Serializer.Deserialize<T>(ms);
            }
        }
    }
}