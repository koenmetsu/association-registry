﻿namespace AssociationRegistry.Admin.Api.Verenigingen;

using System.Linq;
using System.Threading.Tasks;
using Events;

public class VerenigingsRepository : IVerenigingsRepository
{
    private readonly IEventStore _eventStore;

    public VerenigingsRepository(IEventStore eventStore)
    {
        _eventStore = eventStore;
    }

    public async Task Save(Vereniging vereniging)
    {
        await _eventStore.Save(vereniging.VNummer, vereniging.Events.ToArray());
    }
}
