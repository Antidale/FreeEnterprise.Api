using Npgsql;

namespace FreeEnterprise.Api.IntegrationTests.BaseClasses;

public partial class TestBase(FixtureBase fixture)
{
    public FixtureBase FixtureBase = fixture;
    public void SetupProviderMock()
    {
        var connectionstring = FixtureBase.Container.GetConnectionString();
        var connection = new NpgsqlConnection(connectionstring);

        FixtureBase.ProviderMock.Setup(x => x.GetConnection()).Returns(connection);
    }
}
