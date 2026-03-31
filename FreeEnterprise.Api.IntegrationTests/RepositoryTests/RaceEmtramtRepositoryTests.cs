using System;

using FreeEnterprise.Api.IntegrationTests.BaseClasses;
using FreeEnterprise.Api.Models;
using FreeEnterprise.Api.Repositories;
using Microsoft.Extensions.Logging;


namespace FreeEnterprise.Api.IntegrationTests.RepositoryTests;

public class RaceEntramtRepositoryFixture(IMessageSink messageSink) : FixtureBase(messageSink)
{
    public Mock<ILogger<SeedRepository>> SeedLoggerMock = new(MockBehavior.Loose);
    public Mock<ILogger<RacerRepository>> RacerLoggerMock = new(MockBehavior.Loose);
    public Mock<ILogger<RaceEntrantRepository>> LoggerMock = new(MockBehavior.Loose);
}

public partial class RaceEntrantRepositoryTests(RaceEntramtRepositoryFixture fixture) : TestBase(fixture), IClassFixture<RaceEntramtRepositoryFixture>
{
    //arrange

    //insert seed

    //insert race

    //act

    //assert
}
