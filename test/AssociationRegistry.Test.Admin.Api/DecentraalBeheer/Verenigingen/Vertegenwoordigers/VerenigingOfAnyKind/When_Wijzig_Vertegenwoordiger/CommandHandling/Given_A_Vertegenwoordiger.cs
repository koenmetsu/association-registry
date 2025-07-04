﻿namespace AssociationRegistry.Test.Admin.Api.DecentraalBeheer.Verenigingen.Vertegenwoordigers.VerenigingOfAnyKind.When_Wijzig_Vertegenwoordiger.CommandHandling;

using AssociationRegistry.DecentraalBeheer.Vertegenwoordigers.WijzigVertegenwoordiger;
using AssociationRegistry.Events;
using AssociationRegistry.Framework;
using AssociationRegistry.Test.Common.AutoFixture;
using AssociationRegistry.Test.Common.Framework;
using AssociationRegistry.Test.Common.Scenarios.CommandHandling.FeitelijkeVereniging;
using AssociationRegistry.Vereniging.Emails;
using AssociationRegistry.Vereniging.SocialMedias;
using AssociationRegistry.Vereniging.TelefoonNummers;
using AutoFixture;
using Common.StubsMocksFakes.VerenigingsRepositories;
using Xunit;

public class Given_A_Vertegenwoordiger
{
    private readonly WijzigVertegenwoordigerCommandHandler _commandHandler;
    private readonly Fixture _fixture;
    private readonly FeitelijkeVerenigingWerdGeregistreerdWithAPrimairVertegenwoordigerScenario _scenario;
    private readonly VerenigingRepositoryMock _verenigingRepositoryMock;

    public Given_A_Vertegenwoordiger()
    {
        _fixture = new Fixture().CustomizeAdminApi();

        _scenario = new FeitelijkeVerenigingWerdGeregistreerdWithAPrimairVertegenwoordigerScenario();
        _verenigingRepositoryMock = new VerenigingRepositoryMock(_scenario.GetVerenigingState());

        _commandHandler = new WijzigVertegenwoordigerCommandHandler(_verenigingRepositoryMock);
    }

    [Fact]
    public async ValueTask Then_A_VertegenwoordigerWerdGewijzigd_Event_Is_Saved_With_The_Next_Id()
    {
        var command = new WijzigVertegenwoordigerCommand(
            _scenario.VCode,
            new WijzigVertegenwoordigerCommand.CommandVertegenwoordiger(
                _scenario.VertegenwoordigerWerdToegevoegd.VertegenwoordigerId,
                _fixture.Create<string?>(),
                _fixture.Create<string?>(),
                _fixture.Create<Email>(),
                _fixture.Create<TelefoonNummer>(),
                _fixture.Create<TelefoonNummer>(),
                _fixture.Create<SocialMedia>(),
                IsPrimair: false));

        await _commandHandler.Handle(new CommandEnvelope<WijzigVertegenwoordigerCommand>(command, _fixture.Create<CommandMetadata>()));

        _verenigingRepositoryMock.ShouldHaveSavedExact(
            new VertegenwoordigerWerdGewijzigd(
                _scenario.VertegenwoordigerWerdToegevoegd.VertegenwoordigerId,
                command.Vertegenwoordiger.IsPrimair!.Value,
                command.Vertegenwoordiger.Roepnaam!,
                command.Vertegenwoordiger.Rol!,
                _scenario.VertegenwoordigerWerdToegevoegd.Voornaam,
                _scenario.VertegenwoordigerWerdToegevoegd.Achternaam,
                command.Vertegenwoordiger.Email!.Waarde,
                command.Vertegenwoordiger.Telefoon!.Waarde,
                command.Vertegenwoordiger.Mobiel!.Waarde,
                command.Vertegenwoordiger.SocialMedia!.Waarde
            )
        );
    }
}
