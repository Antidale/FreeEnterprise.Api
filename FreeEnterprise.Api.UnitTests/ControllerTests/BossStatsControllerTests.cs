using System.Net;
using Xunit;
using NSubstitute;
using FreeEnterprise.Api.Interfaces;
using FreeEnterprise.Api.Controllers;
using FreeEnterprise.Api.Requests;
using FreeEnterprise.Api.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FreeEnterprise.Api.UnitTets.ControllerTests
{
	public class BossStatsControllerTests
	{
		IBossStatsRepository bossStatsRepositoryMock;

		[Fact]
		public async Task BossStatsController_Requires_ValidRequest()
		{
            var request = new BossStatsSearchRequest
            {
                LocationId = 1,
                BattleId = 1
            };

            bossStatsRepositoryMock = Substitute.For<IBossStatsRepository>();
            bossStatsRepositoryMock.SearchAsync(Arg.Any<BossStatsSearchRequest>()).Returns(new List<BossStat>());

			var sut = new BossStatsController(bossStatsRepositoryMock);

			var response = await sut.Search(request);
			
			Assert.Equal(HttpStatusCode.OK, (HttpStatusCode)(response.Result as StatusCodeResult).StatusCode);

		}
	}
}
