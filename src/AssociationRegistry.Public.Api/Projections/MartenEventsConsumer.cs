namespace AssociationRegistry.Public.Api.Projections;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Events;
using Marten.Events;

public class MartenEventsConsumer : IMartenEventsConsumer
{
    private readonly ElasticEventHandler _eventHandler;

    public MartenEventsConsumer(ElasticEventHandler eventHandler)
    {
        _eventHandler = eventHandler;
    }

    public Task ConsumeAsync(IReadOnlyList<StreamAction> streamActions)
    {
        foreach (var @event in streamActions.SelectMany(streamAction => streamAction.Events))
            if (@event.EventType == typeof(VerenigingWerdGeregistreerd))
                _eventHandler.HandleEvent((VerenigingWerdGeregistreerd)@event.Data);

        return Task.CompletedTask;
    }
}
