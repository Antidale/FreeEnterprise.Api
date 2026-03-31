using System;

using FreeEnterprise.Api.IntegrationTests.BaseClasses;
using FreeEnterprise.Api.Models;
using FreeEnterprise.Api.Repositories;
using Microsoft.Extensions.Logging;


namespace FreeEnterprise.Api.IntegrationTests.RepositoryTests;

public class RaceEntrantRepositoryFixture(IMessageSink messageSink) : FixtureBase(messageSink)
{
    public Mock<ILogger<SeedRepository>> SeedLoggerMock = new(MockBehavior.Loose);
    public Mock<ILogger<RacerRepository>> RacerLoggerMock = new(MockBehavior.Loose);
    public Mock<ILogger<RaceEntrantRepository>> LoggerMock = new(MockBehavior.Loose);
}

public partial class RaceEntrantRepositoryTests(RaceEntrantRepositoryFixture fixture) : TestBase(fixture), IClassFixture<RaceEntrantRepositoryFixture>
{
    //arrange

    //insert seed

    //insert race

    //act

    //assert
}
