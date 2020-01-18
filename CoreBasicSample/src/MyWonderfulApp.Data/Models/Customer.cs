using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Linq;

namespace MyWonderfulApp.Data.Models
{
    public class Customer
    {
        public CustomerIdClass CustomerId { get; set; }

        public String CompanyName { get; set; }

        public String ContactName { get; set; }

        public String ContactTitle { get; set; }

        public static Customer Builder(IDataReader arg)
        {
            return new Customer()
            {
                CustomerId = new CustomerIdClass(arg["CustomerID"] as String),
                CompanyName = arg["CompanyName"] as String,
                ContactName = arg["ContactName"] as String,
                ContactTitle = arg["ContactTitle"] as String,
            };
        }
    }

[JsonConverter(typeof(CustomerIdClassConverter))]
public class CustomerIdClass
{
    public CustomerIdClass(String customerId)
    {
        if (customerId.Length != 5)
            throw new ArgumentException("Invalid Id", nameof(customerId));

        if (customerId.Any(c => !Char.IsLetter(c)))
            throw new ArgumentException("Invalid Id", nameof(customerId));

        Id = customerId;
    }

    public String Id { get; private set; }
}

public class CustomerIdClassConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        if (objectType == typeof(CustomerIdClass))
            return true;

        return false;
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.Read()) 
        {
            if (reader.TokenType == JsonToken.PropertyName && "id".Equals(reader.Value as string, StringComparison.OrdinalIgnoreCase)) 
            {
                reader.Read();
                return new CustomerIdClass(reader.Value as string);
            }
        }
        throw new ArgumentException("Value is not a valid id");
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        if (value is CustomerIdClass customerId) 
        {
            var o = new JObject();
            o["id"] = customerId.Id;
            o.WriteTo(writer);
            return;
        }
        throw new ArgumentException("Value is not a valid id");
    }
}
}
