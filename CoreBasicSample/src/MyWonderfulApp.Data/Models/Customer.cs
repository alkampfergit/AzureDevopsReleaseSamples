using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Linq;

namespace MyWonderfulApp.Data.Models
{
    public class Customer
    {
        public CustomerId CustomerId { get; set; }

        public String CompanyName { get; set; }

        public String ContactName { get; set; }

        public String ContactTitle { get; set; }

        public static Customer Builder(IDataRecord arg)
        {
            return new Customer()
            {
                CustomerId = new CustomerId(arg["CustomerID"] as String),
                CompanyName = arg["CompanyName"] as String,
                ContactName = arg["ContactName"] as String,
                ContactTitle = arg["ContactTitle"] as String,
            };
        }
    }

    [JsonConverter(typeof(CustomerIdClassConverter))]
    public class CustomerId
    {
        private String _id;
        public String Id => _id;

        public CustomerId(String customerId)
        {
            if (customerId.Length != 5)
                throw new ArgumentException("Invalid Id");

            if (customerId.Any(c => !Char.IsLetter(c)))
                throw new ArgumentException("Invalid Id");

            _id = customerId;
        }
    }

    public class CustomerIdClassConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(CustomerId))
                return true;

            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value is String id)
            {
                return new CustomerId(id);
            }
            reader.Read();
            if ("id".Equals(reader.Value as String, StringComparison.OrdinalIgnoreCase))
            {
                //we have an id property, just read it
                reader.Read();
                return new CustomerId(reader.Value as String);
            }

            throw new ArgumentException("Value is not a valid id");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is CustomerId customerId)
            {
                var o = JToken.FromObject(customerId.Id);
                o.WriteTo(writer);
                return;
            }
            throw new ArgumentException("Value is not a valid id");
        }
    }
}
