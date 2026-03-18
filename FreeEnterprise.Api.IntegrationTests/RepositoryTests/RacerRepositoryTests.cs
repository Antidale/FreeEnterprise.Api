using FreeEnterprise.Api.IntegrationTests.BaseClasses;
using FreeEnterprise.Api.Repositories;
using Microsoft.Extensions.Logging;

namespace FreeEnterprise.Api.IntegrationTests.RepositoryTests;

public class RacerRepositoryFixture(IMessageSink messageSink) : FixtureBase(messageSink), IAsyncDisposable
{

    public Mock<ILogger<RacerRepository>> LoggerMock = new(MockBehavior.Loose);
}

public partial class RacerRepositoryTests(RacerRepositoryFixture fixture) : TestBase(fixture), IClassFixture<RacerRepositoryFixture>
{
    [Fact]
    public async Task MergeRacersAsync_InsertsNewRacer()
    {
        var newRacer = new Models.Racer
        {
            id = 0,
            racetime_display_name = "new racer",
            racetime_id = "newracer#1234",
            twitch_name = "newRacer"
        };
        var racerList = new List<Models.Racer> { newRacer };

        SetupProviderMock();

        var sut = new RacerRepository(FixtureBase.ProviderMock.Object, fixture.LoggerMock.Object);

        await sut.MergeRacersAsync(racerList);

        SetupProviderMock();
        var returnedRacer = await sut.GetRacerAsync(newRacer.twitch_name);

        returnedRacer.Success.Should().BeTrue();
        returnedRacer.Data.Should().NotBeNull();
        returnedRacer.Data.RacetimeId.Should().Be(newRacer.racetime_id);
        returnedRacer.Data.RacetimeName.Should().Be(newRacer.racetime_display_name);
    }

    [Fact]
    public async Task MergeRacersAsync_UpdatesExistingRacer()
    {
        var newRacer = new Models.Racer
        {
            racetime_display_name = "new racer",
            racetime_id = "newracer#1234",
            twitch_name = "newRacer"
        };

        var untouchedRacer = new Models.Racer
        {
            racetime_display_name = "No Changes",
            racetime_id = "noChanges#1234",
            twitch_name = "noChanges"
        };

        var racerList = new List<Models.Racer> { newRacer, untouchedRacer };

        //Initial Insert
        SetupProviderMock();

        var sut = new RacerRepository(FixtureBase.ProviderMock.Object, fixture.LoggerMock.Object);

        await sut.MergeRacersAsync(racerList);

        //Update the racer
        SetupProviderMock();

        var updatedRacer = new Models.Racer
        {
            racetime_display_name = newRacer.racetime_display_name + "-updated",
            racetime_id = newRacer.racetime_id,
            twitch_name = newRacer.twitch_name
        };

        var updatedRacersList = new List<Models.Racer> { updatedRacer, untouchedRacer };

        await sut.MergeRacersAsync(updatedRacersList);

        //Verify update occured
        SetupProviderMock();
        var returnedRacer = await sut.GetRacerAsync(updatedRacer.racetime_id);

        returnedRacer.Success.Should().BeTrue();
        returnedRacer.Data.Should().NotBeNull();
        returnedRacer.Data.RacetimeName.Should().Be(updatedRacer.racetime_display_name);
        returnedRacer.Data.TwitchName.Should().Be(updatedRacer.twitch_name);

        SetupProviderMock();

        var returnedNoChangesRacer = await sut.GetRacerAsync(untouchedRacer.racetime_id);
        returnedNoChangesRacer.Success.Should().BeTrue();
        returnedNoChangesRacer.Data.Should().NotBeNull();
        returnedNoChangesRacer.Data.RacetimeName.Should().Be(untouchedRacer.racetime_display_name);
        returnedNoChangesRacer.Data.TwitchName.Should().Be(untouchedRacer.twitch_name);
    }
}
