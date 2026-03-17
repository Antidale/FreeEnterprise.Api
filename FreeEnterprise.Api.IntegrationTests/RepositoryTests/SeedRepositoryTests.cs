using FeInfo.Common.Requests;
using FreeEnterprise.Api.IntegrationTests.BaseClasses;
using FreeEnterprise.Api.Repositories;
using Microsoft.Extensions.Logging;

namespace FreeEnterprise.Api.IntegrationTests.RepositoryTests;

public class SeedRespistoryFixture(IMessageSink messageSink) : FixtureBase(messageSink)
{
    public Mock<ILogger<SeedRepository>> LoggerMock = new(MockBehavior.Loose);
    public int seedId = 0;
}

public partial class SeedRepositoryTests(SeedRespistoryFixture fixture) : TestBase(fixture), IClassFixture<SeedRespistoryFixture>
{
    [Fact]
    public async Task SaveRolledSeedAsync_ReturnsSuccessfulResponse()
    {
        //setup to abstract away?
        SetupProviderMock();
        var seed = new LogSeedRoled(101, new FeInfo.Common.DTOs.SeedInformation("1.0.0", "seedValue", "flags", "verification", "http://test.com"));
        await FixtureBase.Container.StartAsync(TestContext.Current.CancellationToken);

        var sut = new SeedRepository(FixtureBase.ProviderMock.Object, fixture.LoggerMock.Object);

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
        await FixtureBase.Container.StartAsync(TestContext.Current.CancellationToken);

        var sut = new SeedRepository(FixtureBase.ProviderMock.Object, fixture.LoggerMock.Object);

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

        await FixtureBase.Container.StartAsync(TestContext.Current.CancellationToken);

        var sut = new SeedRepository(FixtureBase.ProviderMock.Object, fixture.LoggerMock.Object);

        var getResponse = await sut.GetSeedByIdAsync(-1);
        getResponse.Should().NotBeNull();
        getResponse.Success.Should().BeFalse();
        getResponse.Data.Should().BeNull();
    }
}
