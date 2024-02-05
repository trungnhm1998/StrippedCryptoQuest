using System;
using Newtonsoft.Json;

namespace CryptoQuest.Networking
{
    /// <summary>
    /// Since server side dont have .0 trail behind decimals, we need to truncate them.
    /// So that the generated hash can match with the server.
    /// </summary>
    public class TruncateDecimalJsonConverter : JsonConverter
    {
        public TruncateDecimalJsonConverter() { }

        public override bool CanRead => false;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
        }

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(decimal) || objectType == typeof(float) || objectType == typeof(double));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (IsWholeValue(value))
            {
                writer.WriteRawValue(JsonConvert.ToString(Convert.ToInt64(value)));
                return;
            }

            writer.WriteRawValue(JsonConvert.ToString(value));
        }

        private static bool IsWholeValue(object value)
        {
            if (value is decimal decimalValue)
            {
                int precision = (Decimal.GetBits(decimalValue)[3] >> 16) & 0x000000FF;
                return precision == 0;
            }

            if (value is float floatValue)
            {
                return floatValue == Math.Truncate(floatValue);
            }

            if (value is double doubleValue)
            {
                return doubleValue == Math.Truncate(doubleValue);
            }

            return false;
        }
    }
}