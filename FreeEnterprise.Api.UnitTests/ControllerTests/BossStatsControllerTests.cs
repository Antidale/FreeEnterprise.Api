using System.Threading.Tasks;
using FluentAssertions;
using FreeEnterprise.Api.Enpdoints.BossInfo;
using FreeEnterprise.Api.Interfaces;
using FreeEnterprise.Api.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FreeEnterprise.Api.UnitTests.ControllerTests
{
    public class BossStatsControllerTests
    {
        readonly Mock<IBossStatsRepository> bossStatsRepositoryMock;

        public BossStatsControllerTests()
        {
            bossStatsRepositoryMock = new Mock<IBossStatsRepository>();
        }

        [Theory]
        [InlineData(0, 0, StatusCodes.Status400BadRequest)]
        public async Task BossStatsController_Requires_ValidRequest(int locationId, int battleId, int statusCode)
        {
            bossStatsRepositoryMock
                .Setup(x => x.SearchAsync(It.IsAny<BossStatsSearchRequest>()))
                .ReturnsAsync([]);

            var request = new BossStatsSearchRequest
            {
                LocationId = locationId,
                BattleId = battleId
            };

            var response = await GetBossStatsEndpoint.GetBossStatsAsync(bossStatsRepositoryMock.Object, request);

            var responseProblem = response as ProblemHttpResult;
            responseProblem.StatusCode.Should().Be(statusCode);
            responseProblem.ContentType.Should().Be("application/problem+json");
        }
    }
}
