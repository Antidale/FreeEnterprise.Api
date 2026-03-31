using FeInfo.Common.Requests;
using FreeEnterprise.Api.IntegrationTests.BaseClasses;
using FreeEnterprise.Api.Models;
using FreeEnterprise.Api.Repositories;
using Microsoft.Extensions.Logging;

namespace FreeEnterprise.Api.IntegrationTests.RepositoryTests;

public class RaceEntrantRepositoryFixture(IMessageSink messageSink) : FixtureBase(messageSink)
{
    public Mock<ILogger<RaceEntrantRepository>> LoggerMock = new(MockBehavior.Loose);
}

public partial class RaceEntrantRepositoryTests(RaceEntrantRepositoryFixture fixture) : TestBase(fixture), IClassFixture<RaceEntrantRepositoryFixture>
{
    [Fact]
    public async Task MergeInsertsAnEntrant()
    {
        //arrange
        var sut = new RaceEntrantRepository(FixtureBase.ProviderMock.Object, fixture.LoggerMock.Object);
        var raceLogger = new Mock<ILogger<RaceRepository>>(MockBehavior.Loose);
        //assert
        var raceRepo = new RaceRepository(FixtureBase.ProviderMock.Object, raceLogger.Object);

        var racerLogger = new Mock<ILogger<RacerRepository>>(MockBehavior.Loose);
        var racerRepo = new RacerRepository(FixtureBase.ProviderMock.Object, racerLogger.Object);

        var testModifier = Random.Shared.GetHexString(5);
        var roomName = $"RE_Test_{testModifier}";
        var racetimeId = $"racer_id_{testModifier}";

        var race = new CreateRaceRoom("fake", roomName, "FFA", "Host", []);
        var newRacer = new Racer
        {
            racetime_display_name = "new racer",
            racetime_id = racetimeId,
            twitch_name = "newRacer"
        };

        var entrant = new CreateRaceEntrantModel
        {
            room_name = roomName,
            racetime_id = racetimeId,
            finish_time = "P0DT01H34M42.116959S"
        };

        SetupProviderMock();
        //These should be passing in other contexts, no particular need to validate them here
        await raceRepo.CreateRaceAsync(race);
        SetupProviderMock();
        await racerRepo.MergeRacersAsync([newRacer]);


        //act
        SetupProviderMock();
        var response = await sut.InsertRaceEntrants([entrant]);

        response.Success.Should().BeTrue($"Error message from insert: {response.ErrorMessage}");

        SetupProviderMock();
        var returnedRace = await raceRepo.GetRaceAsync(roomName);

        returnedRace.Success.Should().BeTrue();

    }
}
