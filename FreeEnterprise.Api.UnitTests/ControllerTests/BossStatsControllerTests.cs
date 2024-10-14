using FeInfo.Common.DTOs;
using FreeEnterprise.Api.Controllers;
using FreeEnterprise.Api.Interfaces;
using FreeEnterprise.Api.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace FreeEnterprise.Api.UnitTests.ControllerTests
{
    public class BossStatsControllerTests
    {
        Mock<IBossStatsRepository> bossStatsRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            bossStatsRepositoryMock = new Mock<IBossStatsRepository>();
        }

        [TestCase(0, 0, HttpStatusCode.BadRequest)]
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
            Assert.That((HttpStatusCode)(response.Result as StatusCodeResult).StatusCode, Is.EqualTo(responseCode));
        }
    }
}
