using GaloreAPIDemoV2.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GaloreAPIDemoV2.Converters
{
    public class DataPointOrError
    {
        public readonly DataPoint DataPoint;
        public readonly string Error;
        public DataPointOrError(DataPoint dataPoint, string error) {
            DataPoint = dataPoint;
            Error = error;
        }
    }

    public class DataPointOrErrorConverter : JsonConverter<DataPointOrError>
    {
        public override DataPointOrError Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                return new DataPointOrError(null!, reader.GetString());
            }
            else if (reader.TokenType == JsonTokenType.StartObject)
            {
                var dataPoint = JsonSerializer.Deserialize<DataPoint>(ref reader, options);
                return new DataPointOrError(dataPoint, "");
            }
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, DataPointOrError container, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
