using main;
using NUnit.Framework;
using PB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ComprehensiveExample.Tests
{
    public class ProtocolBuffersTestRunner
    {
        [Test]
        public static void RunAllTests()
        {
            try
            {
                TestBasicSerializationDeserialization();
                Console.WriteLine("✓ 基本序列化/反序列化测试通过");

                TestOptionalFields();
                Console.WriteLine("✓ 可选字段测试通过");

                TestRepeatedFields();
                Console.WriteLine("✓ 重复字段测试通过");

                TestMapFields();
                Console.WriteLine("✓ 映射字段测试通过");

                TestMergeOperations();
                Console.WriteLine("✓ 合并操作测试通过");

                TestNestedMessages();
                Console.WriteLine("✓ 嵌套消息测试通过");

                TestEmptyAndDefaultValues();
                Console.WriteLine("✓ 空值和默认值测试通过");

                Console.WriteLine("\n🎉 所有测试通过！");
            }
            catch (Exception ex)
            {
                throw new Exception($"测试失败: {ex.Message}", ex);
            }
        }

        public static void TestBasicSerializationDeserialization()
        {
            // 创建原始消息
            var original = new ComprehensiveMessage
            {
                double_field = 3.14159,
                optional_double_field = 2.71828,
                float_field = 1.41421f,
                optional_float_field = 1.73205f,
                int32_field = 42,
                optional_int32_field = 100,
                int64_field = 123456789012345L,
                optional_int64_field = 987654321098765L,
                uint32_field = 4294967295,
                optional_uint32_field = 1234567890,
                uint64_field = 18446744073709551615UL,
                optional_uint64_field = 12345678901234567890UL,
                sint32_field = -12345,
                optional_sint32_field = -67890,
                sint64_field = -123456789012345L,
                optional_sint64_field = -987654321098765L,
                fixed32_field = 400000000,
                optional_fixed32_field = 300000000,
                fixed64_field = 1800000000000000000L,
                optional_fixed64_field = 9000000000000000000L,
                bool_field = true,
                optional_bool_field = false,
                string_field = "Hello, Protocol Buffers!",
                optional_string_field = "Optional string value",
                bytes_field = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05 },
                optional_bytes_field = new byte[] { 0xFF, 0xFE, 0xFD, 0xFC },
                enum_field = UserRole.ROLE_ADMIN,
                optional_enum_field = UserRole.ROLE_MODERATOR
            };

            // 序列化
            var writer = new PBWriter(new MemoryStream(10));
            original.Write(writer);

            // 反序列化
            var deserialized = new ComprehensiveMessage();
            deserialized.Read(new PBReader(writer.ToBytes()));

            // 验证所有基本字段
            if (Math.Abs(original.double_field - deserialized.double_field) > 0.0001)
                throw new Exception("double_field 序列化失败");

            if (Math.Abs(original.optional_double_field - deserialized.optional_double_field) > 0.0001)
                throw new Exception("optional_double_field 序列化失败");

            if (Math.Abs(original.float_field - deserialized.float_field) > 0.0001)
                throw new Exception("float_field 序列化失败");

            if (original.int32_field != deserialized.int32_field)
                throw new Exception("int32_field 序列化失败");

            if (original.int64_field != deserialized.int64_field)
                throw new Exception("int64_field 序列化失败");

            if (original.uint32_field != deserialized.uint32_field)
                throw new Exception("uint32_field 序列化失败");

            if (original.uint64_field != deserialized.uint64_field)
                throw new Exception("uint64_field 序列化失败");

            if (original.sint32_field != deserialized.sint32_field)
                throw new Exception("sint32_field 序列化失败");

            if (original.sint64_field != deserialized.sint64_field)
                throw new Exception("sint64_field 序列化失败");

            if (original.fixed32_field != deserialized.fixed32_field)
                throw new Exception("fixed32_field 序列化失败");

            if (original.fixed64_field != deserialized.fixed64_field)
                throw new Exception("fixed64_field 序列化失败");

            if (original.bool_field != deserialized.bool_field)
                throw new Exception("bool_field 序列化失败");

            if (original.optional_bool_field != deserialized.optional_bool_field)
                throw new Exception("optional_bool_field 序列化失败");

            if (original.string_field != deserialized.string_field)
                throw new Exception("string_field 序列化失败");

            if (original.optional_string_field != deserialized.optional_string_field)
                throw new Exception("optional_string_field 序列化失败");

            if (!original.bytes_field.SequenceEqual(deserialized.bytes_field))
                throw new Exception("bytes_field 序列化失败");

            if (!original.optional_bytes_field.SequenceEqual(deserialized.optional_bytes_field))
                throw new Exception("optional_bytes_field 序列化失败");

            if (original.enum_field != deserialized.enum_field)
                throw new Exception("enum_field 序列化失败");

            if (original.optional_enum_field != deserialized.optional_enum_field)
                throw new Exception("optional_enum_field 序列化失败");
        }

        public static void TestOptionalFields()
        {
            var message = new ComprehensiveMessage
            {
                double_field = 1.0,
                string_field = "Required field"
            };

            // 不设置可选字段进行序列化
            var writer = new PBWriter(new MemoryStream(10));
            message.Write(writer);
            var deserialized1 = new ComprehensiveMessage();
            deserialized1.Read(new PBReader(writer.ToBytes()));

            // 设置可选字段
            message.optional_double_field = 2.0;
            message.optional_string_field = "Now set";
            message.optional_enum_field = UserRole.ROLE_GUEST;

            var writer2 = new PBWriter(new MemoryStream(10));
            message.Write(writer2);
            var deserialized2 = new ComprehensiveMessage();
            deserialized2.Read(new PBReader(writer2.ToBytes()));

            if (Math.Abs(deserialized2.optional_double_field - 2.0) > 0.0001)
                throw new Exception("可选字段 optional_double_field 设置失败");

            if (deserialized2.optional_string_field != "Now set")
                throw new Exception("可选字段 optional_string_field 设置失败");

            if (deserialized2.optional_enum_field != UserRole.ROLE_GUEST)
                throw new Exception("可选字段 optional_enum_field 设置失败");
        }

        public static void TestRepeatedFields()
        {
            var message = new ComprehensiveMessage();

            // 添加重复字段数据
            message.tags.AddRange(new[] { "tag1", "tag2", "tag3" });
            message.scores.AddRange(new[] { 95, 87, 92, 78 });
            message.role_history.AddRange(new[] { UserRole.ROLE_USER, UserRole.ROLE_ADMIN, UserRole.ROLE_GUEST });
            message.optional_double_list.AddRange(new[] { 1.1, 2.2, 3.3 });
            message.binary_chunks.AddRange(new[]
            {
                new byte[] { 0xAA, 0xBB, 0xCC },
                new byte[] { 0xDD, 0xEE, 0xFF }
            });

            // 序列化和反序列化
            var writer = new PBWriter(new MemoryStream(10));
            message.Write(writer);
            var deserialized = new ComprehensiveMessage();
            deserialized.Read(new PBReader(writer.ToBytes()));

            // 验证重复字段
            if (!message.tags.SequenceEqual(deserialized.tags))
                throw new Exception("tags 重复字段序列化失败");

            if (!message.scores.SequenceEqual(deserialized.scores))
                throw new Exception("scores 重复字段序列化失败");

            if (!message.role_history.SequenceEqual(deserialized.role_history))
                throw new Exception("role_history 重复字段序列化失败");

            if (!message.optional_double_list.SequenceEqual(deserialized.optional_double_list))
                throw new Exception("optional_double_list 重复字段序列化失败");

            if (message.binary_chunks.Count != deserialized.binary_chunks.Count)
                throw new Exception("binary_chunks 数量不匹配");

            for (int i = 0; i < message.binary_chunks.Count; i++)
            {
                if (!message.binary_chunks[i].SequenceEqual(deserialized.binary_chunks[i]))
                    throw new Exception($"binary_chunks[{i}] 序列化失败");
            }
        }

        public static void TestMapFields()
        {
            var message = new ComprehensiveMessage();

            // 添加映射字段数据
            message.string_map["key1"] = "value1";
            message.string_map["key2"] = "value2";
            message.id_to_name_map[1] = "Alice";
            message.id_to_name_map[2] = "Bob";
            message.role_assignments["admin"] = UserRole.ROLE_ADMIN;
            message.role_assignments["user"] = UserRole.ROLE_USER;
            message.resource_map["config"] = new byte[] { 0x11, 0x22, 0x33 };

            // 添加嵌套消息到映射
            message.location_map["home"] = new Address
            {
                street = "123 Main St",
                city = "New York",
                country = "USA",
                postal_code = "10001"
            };

            // 序列化和反序列化
            var writer = new PBWriter(new MemoryStream(10));
            message.Write(writer);
            var deserialized = new ComprehensiveMessage();
            deserialized.Read(new PBReader(writer.ToBytes()));

            // 验证映射字段
            if (message.string_map.Count != deserialized.string_map.Count)
                throw new Exception("string_map 数量不匹配");

            foreach (var kvp in message.string_map)
            {
                if (!deserialized.string_map.ContainsKey(kvp.Key) || deserialized.string_map[kvp.Key] != kvp.Value)
                    throw new Exception($"string_map[{kvp.Key}] 序列化失败");
            }

            foreach (var kvp in message.id_to_name_map)
            {
                if (!deserialized.id_to_name_map.ContainsKey(kvp.Key) || deserialized.id_to_name_map[kvp.Key] != kvp.Value)
                    throw new Exception($"id_to_name_map[{kvp.Key}] 序列化失败");
            }

            foreach (var kvp in message.role_assignments)
            {
                if (!deserialized.role_assignments.ContainsKey(kvp.Key) || deserialized.role_assignments[kvp.Key] != kvp.Value)
                    throw new Exception($"role_assignments[{kvp.Key}] 序列化失败");
            }

            foreach (var kvp in message.resource_map)
            {
                if (!deserialized.resource_map.ContainsKey(kvp.Key) || !deserialized.resource_map[kvp.Key].SequenceEqual(kvp.Value))
                    throw new Exception($"resource_map[{kvp.Key}] 序列化失败");
            }

            // 验证嵌套消息映射
            if (message.location_map.Count != deserialized.location_map.Count)
                throw new Exception("location_map 数量不匹配");

            foreach (var kvp in message.location_map)
            {
                if (!deserialized.location_map.ContainsKey(kvp.Key))
                    throw new Exception($"location_map[{kvp.Key}] 键缺失");

                var originalAddr = kvp.Value;
                var deserializedAddr = deserialized.location_map[kvp.Key];

                if (originalAddr.street != deserializedAddr.street ||
                    originalAddr.city != deserializedAddr.city ||
                    originalAddr.country != deserializedAddr.country ||
                    originalAddr.postal_code != deserializedAddr.postal_code)
                {
                    throw new Exception($"location_map[{kvp.Key}] 嵌套消息序列化失败");
                }
            }
        }

        public static void TestMergeOperations()
        {
            // 创建第一个消息
            var message1 = new ComprehensiveMessage
            {
                double_field = 3.14,
                float_field = 2.71f,
                int32_field = 100,
                string_field = "First Message",
                bytes_field = new byte[] { 0x01, 0x02 },
                enum_field = UserRole.ROLE_USER
            };
            message1.tags.Add("tag1");
            message1.scores.Add(100);
            message1.string_map["common_key"] = "first_value";
            message1.string_map["unique_key1"] = "value1";

            // 创建第二个消息
            var message2 = new ComprehensiveMessage
            {
                int64_field = 999L,
                uint32_field = 888,
                optional_string_field = "Optional from second",
                optional_bytes_field = new byte[] { 0x03, 0x04 },
                optional_enum_field = UserRole.ROLE_ADMIN
            };
            message2.tags.Add("tag2");
            message2.scores.Add(200);
            message2.string_map["common_key"] = "second_value"; // 这个应该覆盖第一个
            message2.string_map["unique_key2"] = "value2";

            // 序列化第二个消息
            var writer = new PBWriter(new MemoryStream(10));
            message2.Write(writer);
            byte[] message2Data = writer.ToBytes();
            ComprehensiveMessage tmp = new();
            tmp.Read(new PBReader(message2Data));

            // 合并到第一个消息
            message1.Merge(tmp);

            // 验证合并结果
            if (Math.Abs(message1.double_field) > 0.0001)
                throw new Exception("合并后 double_field 值错误");

            if (message1.int64_field != 999L)
                throw new Exception("合并后 int64_field 值错误");

            if (message1.uint32_field != 888)
                throw new Exception("合并后 uint32_field 值错误");

            if (message1.optional_string_field != "Optional from second")
                throw new Exception("合并后 optional_string_field 值错误");

            if (!message1.optional_bytes_field.SequenceEqual(new byte[] { 0x03, 0x04 }))
                throw new Exception("合并后 optional_bytes_field 值错误");

            if (message1.optional_enum_field != UserRole.ROLE_ADMIN)
                throw new Exception("合并后 optional_enum_field 值错误");

            // 验证重复字段合并
            if (message1.tags.Count != 1 || !message1.tags.Contains("tag2"))
                throw new Exception("tags 重复字段合并失败");

            if (message1.scores.Count != 1 || !message1.scores.Contains(200))
                throw new Exception("scores 重复字段合并失败");

            // 验证映射字段合并
            if (message1.string_map["common_key"] != "second_value")
                throw new Exception("映射字段合并时重复键未正确覆盖");

            if (message1.string_map.Count != 2)
                throw new Exception("映射字段合并后数量错误");
        }

        public static void TestNestedMessages()
        {
            var message = new ComprehensiveMessage();

            // 设置嵌套消息
            message.address_field = new Address
            {
                street = "123 Main St",
                city = "New York",
                country = "USA",
                postal_code = "10001",
                location = new GeoLocation
                {
                    latitude = 40.7128,
                    longitude = -74.0060,
                    altitude = 10.5f
                }
            };

            message.optional_address_field = new Address
            {
                street = "456 Oak Ave",
                city = "Los Angeles",
                country = "USA",
                postal_code = "90210"
            };

            // 添加嵌套消息到重复字段
            message.addresses.Add(new Address
            {
                street = "789 Pine Rd",
                city = "Chicago",
                country = "USA",
                postal_code = "60601"
            });

            // 序列化和反序列化
            var writer = new PBWriter(new MemoryStream(10));
            message.Write(writer);
            var deserialized = new ComprehensiveMessage();
            deserialized.Read(new PBReader(writer.ToBytes()));

            // 验证嵌套消息
            if (message.address_field.street != deserialized.address_field.street ||
                message.address_field.city != deserialized.address_field.city ||
                message.address_field.country != deserialized.address_field.country ||
                message.address_field.postal_code != deserialized.address_field.postal_code)
            {
                throw new Exception("address_field 嵌套消息序列化失败");
            }

            if (Math.Abs(message.address_field.location.latitude - deserialized.address_field.location.latitude) > 0.0001 ||
                Math.Abs(message.address_field.location.longitude - deserialized.address_field.location.longitude) > 0.0001 ||
                Math.Abs(message.address_field.location.altitude - deserialized.address_field.location.altitude) > 0.0001)
            {
                throw new Exception("address_field.location 嵌套消息序列化失败");
            }

            if (message.optional_address_field.street != deserialized.optional_address_field.street)
                throw new Exception("optional_address_field 嵌套消息序列化失败");

            if (message.addresses.Count != deserialized.addresses.Count)
                throw new Exception("addresses 重复嵌套消息数量不匹配");

            if (message.addresses[0].street != deserialized.addresses[0].street)
                throw new Exception("addresses[0] 嵌套消息序列化失败");
        }

        public static void TestEmptyAndDefaultValues()
        {
            var message = new ComprehensiveMessage
            {
                string_field = "Only this field is set"
            };

            // 序列化和反序列化
            var writer = new PBWriter(new MemoryStream(10));
            message.Write(writer);
            var deserialized = new ComprehensiveMessage();
            deserialized.Read(new PBReader(writer.ToBytes()));

            // 验证默认值
            if (deserialized.double_field != 0.0)
                throw new Exception("double_field 默认值错误");

            if (deserialized.float_field != 0.0f)
                throw new Exception("float_field 默认值错误");

            if (deserialized.int32_field != 0)
                throw new Exception("int32_field 默认值错误");

            if (deserialized.int64_field != 0L)
                throw new Exception("int64_field 默认值错误");

            if (deserialized.bool_field != false)
                throw new Exception("bool_field 默认值错误");

            if (deserialized.string_field != "Only this field is set")
                throw new Exception("string_field 值错误");

            if (deserialized.bytes_field != null && deserialized.bytes_field.Length != 0)
                throw new Exception("bytes_field 默认值错误");

            if (deserialized.enum_field != 0)
                throw new Exception("enum_field 默认值错误");

            // 验证空的重复字段
            if (deserialized.tags.Count != 0)
                throw new Exception("tags 默认值错误");

            if (deserialized.scores.Count != 0)
                throw new Exception("scores 默认值错误");

            if (deserialized.addresses.Count != 0)
                throw new Exception("addresses 默认值错误");

            // 验证空的映射字段
            if (deserialized.string_map.Count != 0)
                throw new Exception("string_map 默认值错误");

            if (deserialized.id_to_name_map.Count != 0)
                throw new Exception("id_to_name_map 默认值错误");
        }
    }
}