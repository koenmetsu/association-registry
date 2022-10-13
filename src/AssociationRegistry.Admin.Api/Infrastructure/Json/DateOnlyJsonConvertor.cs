namespace AssociationRegistry.Admin.Api.Infrastructure.Json;

using System;
using System.Globalization;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

public class DateOnlyJsonConvertor : JsonConverter<DateOnly>
{
    private readonly string _format;

    public DateOnlyJsonConvertor(string format)
    {
        _format = format;
    }

    public override void WriteJson(JsonWriter writer, DateOnly value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString(_format, CultureInfo.InvariantCulture));
    }

    public override DateOnly ReadJson(JsonReader reader, Type objectType, DateOnly existingValue, bool hasExistingValue, JsonSerializer serializer)
        => DateOnly.ParseExact((string)reader.Value!, _format, CultureInfo.InvariantCulture);
}