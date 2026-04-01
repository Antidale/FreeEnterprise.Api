using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using FreeEnterprise.Api.Classes;
using FreeEnterprise.Api.Interfaces;
using FreeEnterprise.Api.RtggModels;
using FreeEnterprise.Api.Services;
using Microsoft.Extensions.Logging;

namespace FreeEnterprise.Api.UnitTests.ServiceTests;

public class RacetimeRaceUpdateServiceTests
{

    Mock<ILogger<RacetimeRaceUpdateService>> loggerMock;
    Mock<IRacetimeDataService> racetimeDataServiceMock;
    Mock<IRaceRespository> raceRespositoryMock;
    Mock<IRacerRepository> racerRepositoryMock;
    Mock<IRaceEntrantRepository> raceEntrantRepositoryMock;

    RacetimeRaceUpdateService sut;

    public RacetimeRaceUpdateServiceTests()
    {
        loggerMock = new(MockBehavior.Loose);
        racetimeDataServiceMock = new(MockBehavior.Loose);
        raceRespositoryMock = new(MockBehavior.Loose);
        racerRepositoryMock = new(MockBehavior.Loose);
        raceEntrantRepositoryMock = new(MockBehavior.Loose);

        sut = new RacetimeRaceUpdateService(loggerMock.Object, racetimeDataServiceMock.Object, raceRespositoryMock.Object, racerRepositoryMock.Object, raceEntrantRepositoryMock.Object);
    }

    [Fact]
    public async Task GetRtggRacesAsync_FiltersOutDbEnded()
    {
        //setup return for the data service
        racetimeDataServiceMock.Setup(x => x.GetRecentRecordedRtggRaces())
                               .ReturnsAsync(Response.SetSuccess(new List<RtggModels.Race>
                               {
                                    new() {
                                        Name = "room-one",
                                        Url = "room-one",
                                        DataUrl = "room-one/data",
                                        Status = new RtggModels.Status
                                        {
                                            Value = "finished",
                                            VerboseValue = "really, they're done",
                                            HelpText = "there's no helping you"
                                        },
                                        Info = "",
                                        TimeLimit = "24",
                                        Goal = new RtggModels.Goal("Beat Zeromus", false)
                                    },
                                    new() {
                                        Name = "room-two",
                                        Url = "room-two",
                                        DataUrl = "room-two/data",
                                        Status = new RtggModels.Status
                                        {
                                            Value = "finished",
                                            VerboseValue = "really, they're done again",
                                            HelpText = "there's no helping you"
                                        },
                                        Info = "",
                                        TimeLimit = "24",
                                        Goal = new RtggModels.Goal("Beat Zeromus", false)
                                    },
                                    new() {
                                        Name = "room-three",
                                        Url = "room-three",
                                        DataUrl = "room-three/data",
                                        Status = new RtggModels.Status
                                        {
                                            Value = "finished",
                                            VerboseValue = "really, they're triply",
                                            HelpText = "there's no helping you"
                                        },
                                        Info = "",
                                        TimeLimit = "24",
                                        Goal = new RtggModels.Goal("Beat Zeromus", false)
                                    },
                               }));

        //setup return
        raceRespositoryMock.Setup(x => x.GetEndedRacetimeRoomNames())
                           .ReturnsAsync(Response.SetSuccess(new List<string>
                           {
                            "room-one",
                            "room-four-not-recorded"
                           }));

        var result = await sut.GetRtggRacesAsync();

        result.Should().HaveCount(2, "room-one should be excluded the db response includes it in complete races, and so shouldn't be in the list to be changed");
        result.Select(x => x.Name).Should().BeEquivalentTo(["room-two", "room-three"]);

        using (new AssertionScope("Mock Verification"))
        {
            racetimeDataServiceMock.Verify(x => x.GetRecentRecordedRtggRaces(), Times.Once);
            racetimeDataServiceMock.VerifyNoOtherCalls();

            raceRespositoryMock.Verify(x => x.GetEndedRacetimeRoomNames(), Times.Once);
            raceRespositoryMock.VerifyNoOtherCalls();

        }

    }

    //Todo: Add more tests in this vein for other code paths in the method
    [Fact]
    public async Task GetRtggRacesAsync_ReturnsEmptyList_WhenApiResponseIsNotSuccessful()
    {
        racetimeDataServiceMock.Setup(x => x.GetRecentRecordedRtggRaces()).ReturnsAsync(Response<List<Race>>.BadRequest("some error"));

        raceRespositoryMock.Setup(x => x.GetEndedRacetimeRoomNames())
                           .ReturnsAsync(Response.SetSuccess(new List<string>
                           {
                            "room-one",
                            "room-four-not-recorded"
                           }));

        var result = await sut.GetRtggRacesAsync();
        result.Should().BeEmpty();

        using (new AssertionScope("Mock Verification"))
        {
            racetimeDataServiceMock.Verify(x => x.GetRecentRecordedRtggRaces(), Times.Once);
            racetimeDataServiceMock.VerifyNoOtherCalls();

            raceRespositoryMock.Verify(x => x.GetEndedRacetimeRoomNames(), Times.Never);
            raceRespositoryMock.VerifyNoOtherCalls();

            //todo: do verify on the logger?
        }
    }
}
