namespace AssociationRegistry.Acm.Api.Infrastructure.Extensions;

using Constants;
using Hosts.Configuration.ConfigurationBindings;
using JasperFx.CodeGeneration;
using Json;
using AssociationRegistry.Hosts.Marten;
using Marten;
using Marten.Events;
using Marten.Events.Daemon.Resiliency;
using Marten.Services;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Schema.VerenigingenPerInsz;

public static class MartenExtensions
{
    public static IServiceCollection AddMarten(
        this IServiceCollection services,
        PostgreSqlOptionsSection postgreSqlOptions,
        IConfiguration configuration)
    {
        var martenConfiguration = services
                                 .AddSingleton(postgreSqlOptions)
                                 .AddMarten(
                                      serviceProvider =>
                                      {
                                          var opts = new StoreOptions();
                                          ConfigureStoreOptions(opts, postgreSqlOptions, serviceProvider.GetRequiredService<IHostEnvironment>().IsDevelopment());

                                          return opts;
                                      });

        if (configuration["ApplyAllDatabaseChangesDisabled"]?.ToLowerInvariant() != "true")
            martenConfiguration.ApplyAllDatabaseChangesOnStartup();

        if (configuration["ProjectionDaemonDisabled"]?.ToLowerInvariant() != "true")
            martenConfiguration.AddAsyncDaemon(DaemonMode.HotCold);

        return services;
    }

    public static void ConfigureStoreOptions(StoreOptions opts, PostgreSqlOptionsSection postgreSqlOptions, bool isDevelopment)
    {
        opts.Connection(CommonMartenConfigurator.BuildConnectionString(postgreSqlOptions));

        if (!postgreSqlOptions.Schema.IsNullOrEmpty())
        {
            opts.Events.DatabaseSchemaName = postgreSqlOptions.Schema;
            opts.DatabaseSchemaName = postgreSqlOptions.Schema;
        }

        opts.Events.StreamIdentity = StreamIdentity.AsString;
        opts.Serializer(CommonMartenConfigurator.CreateSerializer(
            new NullableDateOnlyJsonConvertor(WellknownFormats.DateOnly),
            new DateOnlyJsonConvertor(WellknownFormats.DateOnly)));
        opts.Events.MetadataConfig.EnableAll();
        opts.AddPostgresProjections();

        opts.Projections.DaemonLockId = 2;

        opts.RegisterDocumentType<VerenigingenPerInszDocument>();
        opts.RegisterDocumentType<VerenigingDocument>();

        if (isDevelopment)
        {
            opts.GeneratedCodeMode = TypeLoadMode.Dynamic;
        }
        else
        {
            opts.GeneratedCodeMode = TypeLoadMode.Static;
            opts.SourceCodeWritingEnabled = false;
        }
    }


}
