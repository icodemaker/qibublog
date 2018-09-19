using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Globalization;

namespace QiBuBlog.Util
{
    public static class JSONHelper
    {
        public static string Encode<T>(T t)
        {
            return Encode(t, Formatting.None);
        }

        private static string Encode<T>(T t, Formatting format)
        {
            var serializerSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Include,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            return JsonConvert.SerializeObject(t, format, serializerSettings);
        }

        public static T Decode<T>(string json)
        {
            var bigintConverter = new BigintConverter();
            return (T)JsonConvert.DeserializeObject(json, typeof(T));
        }
    }

    public class BigintConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(long) || objectType == typeof(ulong);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return 0;
            }
            else
            {
                var convertible = reader.Value as IConvertible;
                if (objectType == typeof(long))
                {
                    if (convertible != null) return convertible.ToInt64(CultureInfo.InvariantCulture);
                }
                else if (objectType == typeof(ulong))
                {
                    if (convertible != null) return convertible.ToUInt64(CultureInfo.InvariantCulture);
                }
                return 0;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteValue("0");
            }
            else if (value is Int64 || value is UInt64)
            {
                writer.WriteValue(value.ToString());
            }
            else
            {
                throw new Exception("Expected Bigint value");
            }
        }
    }
}
