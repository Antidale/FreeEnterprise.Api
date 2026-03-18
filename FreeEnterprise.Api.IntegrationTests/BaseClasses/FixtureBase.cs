using Testcontainers.PostgreSql;
using Testcontainers.Xunit;

namespace FreeEnterprise.Api.IntegrationTests.BaseClasses;

public abstract class FixtureBase(IMessageSink messageSink) : ContainerFixture<PostgreSqlBuilder, PostgreSqlContainer>(messageSink), IAsyncDisposable
{
    public Mock<IConnectionProvider> ProviderMock = new(MockBehavior.Loose);

    protected override PostgreSqlBuilder Configure(PostgreSqlBuilder builder)
    {
        return builder.WithImage("postgres:17-alpine")
                      //I don't really love hard coding this in this manner, but it works well enough for now
                      .WithResourceMapping("../../../../free_enterprise_db/db/scripts/docker.sql", "/docker-entrypoint-initdb.d");
    }

    public async ValueTask DisposeAsync()
    {
        await Container.StopAsync();
        GC.SuppressFinalize(this);
    }
}
