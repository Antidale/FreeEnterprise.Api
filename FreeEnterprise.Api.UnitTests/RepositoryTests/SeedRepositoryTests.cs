
using System.Threading.Tasks;
using Dapper;
using FluentAssertions;
using Npgsql;
using Testcontainers.PostgreSql;
using Testcontainers.Xunit;

namespace FreeEnterprise.Api.UnitTests.RepositoryTests;

public class SeedRepositoryTests(ITestOutputHelper testOutputHelper) : ContainerTest<PostgreSqlBuilder, PostgreSqlContainer>(testOutputHelper)
{
    // protected override PostgreSqlBuilder Configure(PostgreSqlBuilder builder)
    // {
    //     return builder.WithImage("postgres:17");
    // }

    // [Fact]
    // public async Task FirstTest()
    // {
    //     var dbContainer = new PostgreSqlBuilder().WithImage("postgres:17-alpine").Build();
    //     await dbContainer.StartAsync(TestContext.Current.CancellationToken);
    //     var connectionstring = dbContainer.GetConnectionString();
    //     using var connection = new NpgsqlConnection(connectionstring);
    //     connection.Open();
    //     const int expected = 1;
    //     var actual = await connection.QueryFirstAsync<int>("select 1");
    //     actual.Should().Be(expected);

    //     await Container.StopAsync(TestContext.Current.CancellationToken);
    // }


}
