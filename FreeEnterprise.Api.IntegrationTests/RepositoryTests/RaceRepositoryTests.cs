using FeInfo.Common.Requests;
using FluentAssertions.Execution;
using FluentAssertions.Extensions;
using FreeEnterprise.Api.IntegrationTests.BaseClasses;
using FreeEnterprise.Api.Models;
using FreeEnterprise.Api.Repositories;
using Microsoft.Extensions.Logging;

namespace FreeEnterprise.Api.IntegrationTests.RepositoryTests;

public class RaceRepositoryFixture(IMessageSink messageSink) : FixtureBase(messageSink), IAsyncDisposable
{
    public Mock<ILogger<RaceRepository>> LoggerMock = new(MockBehavior.Loose);
}

public partial class RaceRepositoryTests(RaceRepositoryFixture fixture) : TestBase(fixture), IClassFixture<RaceRepositoryFixture>
{
    [Fact]
    public async Task CreateRaceAsync_SavesRaceCorrectly()
    {
        //arrange
        var createRaceRequest = new CreateRaceRoom(
            UserId: 1.ToString(),
            RoomName: "TestRoom",
            RaceType: "FFA",
            RaceHost: "RT.gg",
            Metadata: new Dictionary<string, string>
            {
                ["Goal"] = "Beat Zeromus",
                ["Description"] = "Test Pickup",
            }
        );

        var _sut = new RaceRepository(FixtureBase.ProviderMock.Object, fixture.LoggerMock.Object);

        //act
        SetupProviderMock();
        var raceId = await _sut.CreateRaceAsync(createRaceRequest);

        //basic validation to ensure 
        raceId.Should().NotBeNull();
        raceId.Success.Should().BeTrue($"The race should have been created successfully. response error: {raceId.ErrorMessage}");
        raceId.Data.Should().BeGreaterThan(0);

        //assert
        SetupProviderMock();
        var savedRaceResult = await _sut.GetRaceAsync(raceId!.Data.ToString());
        savedRaceResult.Should().NotBeNull();
        savedRaceResult.Success.Should().BeTrue("We should be getting the race back from the database");
        savedRaceResult.Data.Should().NotBeNull();
        var savedRace = savedRaceResult.Data;
        savedRace.EndedAt.Should().BeNull();
        savedRace.RoomName.Should().Be(createRaceRequest.RoomName);
        savedRace.RaceHost.Should().Be(createRaceRequest.RaceHost);
        savedRace.RaceType.Should().Be(createRaceRequest.RaceType);
        savedRace.Metadata.Should().BeEquivalentTo(createRaceRequest.Metadata);
    }

    [Fact]
    public async Task MergeRacesAsync_InsertsANewRace()
    {
        var sut = new RaceRepository(FixtureBase.ProviderMock.Object, fixture.LoggerMock.Object);

        var race = new Race
        {
            race_host = "testHost",
            race_type = "integration_test",
            room_name = "integration_room",
            ended_at = DateTimeOffset.UtcNow,
            metadata = new Dictionary<string, string>
            {
                ["Goal"] = "Integration Testing",
                ["Description"] = "Know Things Work",
                ["Status"] = "finished",
                ["EntrantsCount"] = "2"
            }
        };

        SetupProviderMock();
        var mergeResult = await sut.MergeRacesAsync([race]);
        mergeResult.Success.Should().BeTrue($"We should have successfully added the race, but found an error {mergeResult.ErrorMessage}");

        SetupProviderMock();
        var getRaceResult = await sut.GetRaceAsync(race.room_name);
        getRaceResult.Should().NotBeNull();
        getRaceResult.Success.Should().BeTrue();
        var retrievedRace = getRaceResult.Data;
        retrievedRace.Should().NotBeNull();
        retrievedRace.RaceHost.Should().Be(race.race_host);
        retrievedRace.EndedAt.Should().BeCloseTo(race.ended_at, 1.Milliseconds());
        retrievedRace.Metadata.Should().BeEquivalentTo(race.metadata);
    }

    [Fact]
    public async Task MergeRacesAsync_InsertsANewRaceAndUpdatesAnExistingOne()
    {
        var sut = new RaceRepository(FixtureBase.ProviderMock.Object, fixture.LoggerMock.Object);

        //Races get created initially with only goal/description set
        var createRaceRequest = new CreateRaceRoom(
            UserId: 1.ToString(),
            RoomName: "TestRoom-UpdateMerge",
            RaceType: "FFA",
            RaceHost: "IntegrationTesting",
            Metadata: new Dictionary<string, string>
            {
                ["Goal"] = "MergeAsync Testing",
                ["Description"] = "Test Updating an Existing Race",
            }
        );

        SetupProviderMock();
        await sut.CreateRaceAsync(createRaceRequest);

        var existingRace = new Race
        {
            race_host = createRaceRequest.RaceHost,
            room_name = createRaceRequest.RoomName,
            race_type = createRaceRequest.RaceType,
            ended_at = DateTimeOffset.UtcNow.AddHours(-6),
            metadata = createRaceRequest.Metadata
        };
        existingRace.metadata.Add("EntrantsCount", "5");
        existingRace.metadata.Add("Status", "finished");


        var newRace = new Race
        {
            race_host = "testHost",
            race_type = "integration_test",
            room_name = "integration_room",
            ended_at = DateTimeOffset.UtcNow,
            metadata = new Dictionary<string, string>
            {
                ["Goal"] = "Integration Testing",
                ["Description"] = "Know Things Work",
                ["Status"] = "finished",
                ["EntrantsCount"] = "20"
            }
        };

        //act 
        SetupProviderMock();
        var mergeResult = await sut.MergeRacesAsync([existingRace, newRace]);
        mergeResult.Success.Should().BeTrue($"We should have successfully added the race, but found an error {mergeResult.ErrorMessage}");


        using (new AssertionScope("Existing Race updates"))
        {
            SetupProviderMock();
            var getExistingRaceResult = await sut.GetRaceAsync(existingRace.room_name);
            getExistingRaceResult.Should().NotBeNull();
            getExistingRaceResult.Success.Should().BeTrue();
            var retrievedExistingRace = getExistingRaceResult.Data;
            retrievedExistingRace.Should().NotBeNull();
            retrievedExistingRace.RaceHost.Should().Be(existingRace.race_host);
            retrievedExistingRace.EndedAt.Should().BeCloseTo(existingRace.ended_at, 1.Milliseconds());
            retrievedExistingRace.Metadata.Should().BeEquivalentTo(existingRace.metadata);
        }

        using (new AssertionScope("New Race Inserts"))
        {
            SetupProviderMock();
            var getNewRaceResult = await sut.GetRaceAsync(newRace.room_name);
            getNewRaceResult.Should().NotBeNull();
            getNewRaceResult.Success.Should().BeTrue($"But we found an error message {getNewRaceResult.ErrorMessage}");
            var retrievedNewRace = getNewRaceResult.Data;
            retrievedNewRace.Should().NotBeNull();
            retrievedNewRace.RaceHost.Should().Be(newRace.race_host);
            retrievedNewRace.EndedAt.Should().BeCloseTo(newRace.ended_at, 1.Milliseconds());
            retrievedNewRace.Metadata.Should().BeEquivalentTo(newRace.metadata);
        }
    }
}
