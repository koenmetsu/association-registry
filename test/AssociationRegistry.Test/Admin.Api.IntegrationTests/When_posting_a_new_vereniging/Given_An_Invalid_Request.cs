namespace AssociationRegistry.Test.Admin.Api.IntegrationTests.When_posting_a_new_vereniging;

using System.Net;
using System.Text;
using AssociationRegistry.Admin.Api.Verenigingen;
using AutoFixture;
using Fixtures;
using FluentAssertions;
using Xunit;

[Collection(VerenigingAdminApiCollection.Name)]
public class Given_An_Invalid_Request
{
    private readonly VerenigingAdminApiFixture _apiFixture;
    private readonly Fixture _fixture;

    public Given_An_Invalid_Request(VerenigingAdminApiFixture apiFixture)
    {
        _apiFixture = apiFixture;
        _fixture = new Fixture();
    }

    [Fact]
    public async Task Then_it_returns_a_badrequest_response()
    {
        var content = GetContent();
        var response = await _apiFixture.HttpClient!.PostAsync("/v1/verenigingen", content);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    private StringContent GetContent()
        => new(
            GetJsonBody(),
            Encoding.UTF8,
            "application/json");

    private string GetJsonBody()
        => GetType()
            .GetAssociatedResourceJson($"{nameof(Given_An_Invalid_Request)}_{nameof(Then_it_returns_a_badrequest_response)}");
}
