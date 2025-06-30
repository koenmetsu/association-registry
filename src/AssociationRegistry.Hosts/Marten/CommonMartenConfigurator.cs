namespace AssociationRegistry.Hosts.Marten;

using AssociationRegistry.Hosts.Configuration.ConfigurationBindings;
using Marten.Services;
using Newtonsoft.Json;

public static class CommonMartenConfigurator
{
    public static string BuildConnectionString(PostgreSqlOptionsSection options)
        => options.GetConnectionString();

    public static JsonNetSerializer CreateSerializer(params JsonConverter[] converters)
    {
        var serializer = new JsonNetSerializer();
        serializer.Customize(settings =>
        {
            settings.DateParseHandling = DateParseHandling.None;
            foreach (var converter in converters)
                settings.Converters.Add(converter);
        });
        return serializer;
    }
}
