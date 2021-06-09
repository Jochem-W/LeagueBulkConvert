using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SimpleGltf.Json.Converters
{
    public class AccessorConverter : JsonConverter<Accessor>
    {
        public override Accessor Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, Accessor value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value.GltfAsset.Accessors.IndexOf(value));
        }
    }
}