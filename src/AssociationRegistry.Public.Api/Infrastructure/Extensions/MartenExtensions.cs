namespace AssociationRegistry.Public.Api.Infrastructure.Extensions;

using Constants;
using Json;
using AssociationRegistry.Hosts.Marten;
using Marten;
using Marten.Events;
using Marten.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PostgreSqlOptionsSection = Hosts.Configuration.ConfigurationBindings.PostgreSqlOptionsSection;

public static class MartenExtensions
{
    public static IServiceCollection AddMarten(
        this IServiceCollection services,
        Hosts.Configuration.ConfigurationBindings.PostgreSqlOptionsSection postgreSqlOptions,
        IConfiguration configuration)
    {
        services.AddMarten(_ =>
        {
            var connectionString1 = CommonMartenConfigurator.BuildConnectionString(postgreSqlOptions);

            var opts = new StoreOptions();

            opts.Connection(connectionString1);

            if (!string.IsNullOrEmpty(postgreSqlOptions.Schema))
            {
                opts.Events.DatabaseSchemaName = postgreSqlOptions.Schema;
                opts.DatabaseSchemaName = postgreSqlOptions.Schema;
            }

            opts.Events.StreamIdentity = StreamIdentity.AsString;

            opts.Events.MetadataConfig.EnableAll();

            opts.Serializer(CommonMartenConfigurator.CreateSerializer(
                new NullableDateOnlyJsonConvertor(WellknownFormats.DateOnly),
                new DateOnlyJsonConvertor(WellknownFormats.DateOnly)));

            return opts;
        }).UseLightweightSessions();

        return services;
    }


}
