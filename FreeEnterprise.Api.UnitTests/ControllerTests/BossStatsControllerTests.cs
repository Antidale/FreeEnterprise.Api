using System.Net;
using System.Threading.Tasks;
using FeInfo.Common.DTOs;
using FluentAssertions;
using FreeEnterprise.Api.Controllers;
using FreeEnterprise.Api.Interfaces;
using FreeEnterprise.Api.Requests;
using Microsoft.AspNetCore.Mvc;

namespace FreeEnterprise.Api.UnitTests.ControllerTests
{
    public class BossStatsControllerTests
    {
        Mock<IBossStatsRepository> bossStatsRepositoryMock;


        public BossStatsControllerTests()
        {
            bossStatsRepositoryMock = new Mock<IBossStatsRepository>();
        }

        [Theory]
        [InlineData(0, 0, HttpStatusCode.BadRequest)]
        public async Task BossStatsController_Requires_ValidRequest(int locationId, int battleId, HttpStatusCode responseCode)
        {
            bossStatsRepositoryMock
                .Setup(x => x.SearchAsync(It.IsAny<BossStatsSearchRequest>()))
                .ReturnsAsync(new List<BossStat>());

            var sut = new BossStatsController(bossStatsRepositoryMock.Object);

            var request = new BossStatsSearchRequest
            {
                LocationId = locationId,
                BattleId = battleId
            };

            var response = await sut.Search(request);
            var statusCode = (HttpStatusCode)(response.Result as StatusCodeResult).StatusCode;
            statusCode.Should().Be(responseCode);

        }
    }
}
