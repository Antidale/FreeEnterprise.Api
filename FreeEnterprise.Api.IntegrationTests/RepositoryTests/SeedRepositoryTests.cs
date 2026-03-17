using FeInfo.Common.Requests;
using FluentAssertions;
using FreeEnterprise.Api.Interfaces;
using FreeEnterprise.Api.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Npgsql;
using Testcontainers.PostgreSql;
using Testcontainers.Xunit;
using Xunit.Sdk;

namespace FreeEnterprise.Api.IntegrationTests.RepositoryTests;

public class SeedRespistoryFixture(IMessageSink messageSink) : ContainerFixture<PostgreSqlBuilder, PostgreSqlContainer>(messageSink), IDisposable
{
    public Mock<ILogger<SeedRepository>> LoggerMock = new(MockBehavior.Loose);
    public Mock<IConnectionProvider> ProviderMock = new(MockBehavior.Loose);
    public int seedId = 0;

    protected override PostgreSqlBuilder Configure(PostgreSqlBuilder builder)
    {
        return builder.WithImage("postgres:17-alpine")
                      //I don't really love hard coding this in this manner, but it works well enough for now
                      .WithResourceMapping("../../../../free_enterprise_db/db/scripts/docker.sql", "/docker-entrypoint-initdb.d");
    }

    public void Dispose()
    {
        Task.Run(async () => await Container.StopAsync());
    }
}

public partial class SeedRepositoryTests(SeedRespistoryFixture fixture) : IClassFixture<SeedRespistoryFixture>
{
    [Fact]
    public async Task SaveRolledSeedAsync_ReturnsSuccessfulResponse()
    {
        //setup to abstract away?
        SetupProviderMock();
        var seed = new LogSeedRoled(101, new FeInfo.Common.DTOs.SeedInformation("1.0.0", "seedValue", "flags", "verification", "http://test.com"));
        await fixture.Container.StartAsync(TestContext.Current.CancellationToken);

        var sut = new SeedRepository(fixture.ProviderMock.Object, fixture.LoggerMock.Object);


        var response = await sut.SaveSeedRolledAsync(seed);

        response.Should().NotBeNull();
        response.Success.Should().BeTrue();
        response.Data.Should().BeGreaterThan(0);
        fixture.seedId = response.Data;
    }

    [Fact]
    public async Task GetSeedByIdAsync_ReturnsPreviouslySavedSeed()
    {
        SetupProviderMock();
        var seed = new LogSeedRoled(101, new FeInfo.Common.DTOs.SeedInformation("1.0.0", "seedValue", "flags", "verification", "http://test.com"));
        await fixture.Container.StartAsync(TestContext.Current.CancellationToken);

        var sut = new SeedRepository(fixture.ProviderMock.Object, fixture.LoggerMock.Object);

        var getResponse = await sut.GetSeedByIdAsync(fixture.seedId);
        getResponse.Should().NotBeNull();
        getResponse.Success.Should().BeTrue();
        getResponse.Data.Should().NotBeNull();
        getResponse.Data.Verification.Should().Be(seed.Info.Verification);
    }

    [Fact]
    public async Task GetSeedByIdAsync_ReturnsNotFoundForAnInvalidId()
    {
        SetupProviderMock();

        await fixture.Container.StartAsync(TestContext.Current.CancellationToken);

        var sut = new SeedRepository(fixture.ProviderMock.Object, fixture.LoggerMock.Object);

        var getResponse = await sut.GetSeedByIdAsync(-1);
        getResponse.Should().NotBeNull();
        getResponse.Success.Should().BeFalse();
        getResponse.Data.Should().BeNull();
    }


    private void SetupProviderMock()
    {
        var connectionstring = fixture.Container.GetConnectionString();
        var connection = new NpgsqlConnection(connectionstring);

        fixture.ProviderMock.Setup(x => x.GetConnection()).Returns(connection);
    }


}
