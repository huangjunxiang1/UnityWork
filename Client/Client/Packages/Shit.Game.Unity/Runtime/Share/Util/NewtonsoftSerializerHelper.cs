using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Unity.Mathematics;

namespace Game
{
    public static class UnityMathematicsJsonConverter
    {
        public static readonly List<JsonConverter> Converters = new() 
        {
            new Float2Converter(), new Float3Converter(), new Float4Converter(),
            new Int2Converter(), new Int3Converter(), new Int4Converter(),
        };

        class Float2Converter : JsonConverter<float2>
        {
            public override float2 ReadJson(JsonReader reader, Type objectType, float2 existingValue, bool hasExistingValue, JsonSerializer serializer) => existingValue;
            public override void WriteJson(JsonWriter writer, float2 value, JsonSerializer serializer) => writer.WriteValue(value.ToString());
        }
        class Float3Converter : JsonConverter<float3>
        {
            public override float3 ReadJson(JsonReader reader, Type objectType, float3 existingValue, bool hasExistingValue, JsonSerializer serializer) => existingValue;
            public override void WriteJson(JsonWriter writer, float3 value, JsonSerializer serializer) => writer.WriteValue(value.ToString());
        }
        class Float4Converter : JsonConverter<float4>
        {
            public override float4 ReadJson(JsonReader reader, Type objectType, float4 existingValue, bool hasExistingValue, JsonSerializer serializer) => existingValue;
            public override void WriteJson(JsonWriter writer, float4 value, JsonSerializer serializer) => writer.WriteValue(value.ToString());
        }
        class Int2Converter : JsonConverter<int2>
        {
            public override int2 ReadJson(JsonReader reader, Type objectType, int2 existingValue, bool hasExistingValue, JsonSerializer serializer) => existingValue;
            public override void WriteJson(JsonWriter writer, int2 value, JsonSerializer serializer) => writer.WriteValue(value.ToString());
        }
        class Int3Converter : JsonConverter<int3>
        {
            public override int3 ReadJson(JsonReader reader, Type objectType, int3 existingValue, bool hasExistingValue, JsonSerializer serializer) => existingValue;
            public override void WriteJson(JsonWriter writer, int3 value, JsonSerializer serializer) => writer.WriteValue(value.ToString());
        }
        class Int4Converter : JsonConverter<int4>
        {
            public override int4 ReadJson(JsonReader reader, Type objectType, int4 existingValue, bool hasExistingValue, JsonSerializer serializer) => existingValue;
            public override void WriteJson(JsonWriter writer, int4 value, JsonSerializer serializer) => writer.WriteValue(value.ToString());
        }
    }
}
