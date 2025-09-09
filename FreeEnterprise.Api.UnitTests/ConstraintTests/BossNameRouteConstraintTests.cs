using System;
using FeInfo.Common.Enums;
using FluentAssertions;
using FreeEnterprise.Api.Constraints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FreeEnterprise.Api.UnitTests.ConstraintTests;

public class BossNameRouteConstraintTests
{
    readonly Mock<HttpContext> httpContextMock;
    readonly Mock<IRouter> routerMock;

    public BossNameRouteConstraintTests()
    {
        httpContextMock = new Mock<HttpContext>();
        routerMock = new Mock<IRouter>();
    }

    [Theory]
    [InlineData(BossName.DMist)]
    [InlineData("DMist")]
    [InlineData("dmist")]
    [InlineData(4)]
    public void BossNameConstraintMatches_EnumIntsCorrectly(object value)
    {
        var routeKey = "bossName";
        var routeDictionary = new RouteValueDictionary
        {
            { routeKey, value }
        };
        var sut = new BossNameRouteConstraint();

        var result = sut.Match(httpContextMock.Object, routerMock.Object, routeKey, routeDictionary, RouteDirection.IncomingRequest);

        result.Should().BeTrue($"{value} was expected to convert to a valid instance of a BossName, but did not.");
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(37)]
    [InlineData("dmust")]
    [InlineData("Octomann")]
    [InlineData("Unknown")]
    [InlineData("unknown")]
    public void BossNameContstraint_PreventsOutOfRangeValues(object value)
    {
        var routeKey = "bossName";
        var routeDictionary = new RouteValueDictionary
        {
            { routeKey, value }
        };
        var sut = new BossNameRouteConstraint();

        var result = sut.Match(httpContextMock.Object, routerMock.Object, routeKey, routeDictionary, RouteDirection.IncomingRequest);

        result.Should().BeFalse($"{value} was not expected to convert to a valid instance of a BossName, but did.");
    }
}
