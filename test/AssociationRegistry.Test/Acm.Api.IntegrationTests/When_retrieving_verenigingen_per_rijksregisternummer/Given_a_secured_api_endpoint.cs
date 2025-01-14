﻿namespace AssociationRegistry.Test.Acm.Api.IntegrationTests.When_retrieving_verenigingen_per_rijksregisternummer;

using System.Net;
using Fixtures;
using FluentAssertions;
using Xunit;

public class Given_a_secured_api_endpoint: IClassFixture<VerenigingAcmApiFixture>
{
    private readonly AcmIntegrationTestHelper _testHelper;
    private const string Route = "/v1/verenigingen";

    public Given_a_secured_api_endpoint(VerenigingAcmApiFixture fixture)
    {
        _testHelper = new AcmIntegrationTestHelper(fixture);
    }

    [Fact]
    public async Task When_authenticated_with_correct_scope_Then_we_get_a_successful_response()
    {
        var client = await _testHelper.CreateAcmClient("dv_verenigingsregister_hoofdvertegenwoordigers");
        var response = await client.GetAsync($"{Route}?rijksregisternummer=123456");
        response.Should().BeSuccessful();
    }

    [Fact]
    public async Task When_authenticated_with_incorrect_scope_Then_we_get_an_unauthorized_response()
    {
        var client = await _testHelper.CreateAcmClient("vo_info");
        var response = await client.GetAsync($"{Route}?rijksregisternummer=123456");
        response.Should().HaveStatusCode(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task When_not_authenticated_Then_we_get_an_unauthorized_response()
    {
        var response = await _testHelper.HttpClient.GetAsync($"{Route}?rijksregisternummer=123456");
        response.Should().HaveStatusCode(HttpStatusCode.Unauthorized);
    }
}
