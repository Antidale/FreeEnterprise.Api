using System.Net;
using NUnit.Framework;
using Moq;
using FreeEnterprise.Api.Interfaces;
using FreeEnterprise.Api.Controllers;
using FreeEnterprise.Api.Requests;
using FreeEnterprise.Api.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace FreeEnterprise.Api.UnitTets.ControllerTests
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
            Assert.AreEqual((HttpStatusCode)(response.Result as StatusCodeResult).StatusCode, responseCode);

        }
    }
}
