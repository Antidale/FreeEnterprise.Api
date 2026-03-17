
using FreeEnterprise.Api.IntegrationTests.BaseClasses;
using FreeEnterprise.Api.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using Testcontainers.PostgreSql;
using Xunit.Sdk;

namespace FreeEnterprise.Api.IntegrationTests.RepositoryTests;

public class RacerRepositoryFixture(IMessageSink messageSink) : FixtureBase(messageSink), IAsyncDisposable
{

    public Mock<ILogger<RacerRepository>> LoggerMock = new(MockBehavior.Loose);

    protected override PostgreSqlBuilder Configure(PostgreSqlBuilder builder)
    {
        return builder.WithImage("postgres:17-alpine")
                      //I don't really love hard coding this in this manner, but it works well enough for now
                      .WithResourceMapping("../../../../free_enterprise_db/db/scripts/docker.sql", "/docker-entrypoint-initdb.d");
    }
}

public partial class RacerRepositoryTests(RacerRepositoryFixture fixture) : TestBase(fixture)
{

}
