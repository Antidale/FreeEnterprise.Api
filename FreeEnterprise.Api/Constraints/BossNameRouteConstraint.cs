using System;
using FeInfo.Common.Enums;

namespace FreeEnterprise.Api.Constraints;

public class BossNameRouteConstraint : IRouteConstraint
{
    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
    {
        var matchingValue = values[routeKey]?.ToString();
        var names = Enum.GetNames<BossName>();
        var enumValues = Enum.GetValues<BossName>().Select(x => (int)x).ToList();

        if (int.TryParse(matchingValue, out var intValue))
        {
            //Special case, this is a valid part of the enum, but will not be in the database and so we can consider it invalid for the purposes of the route constraint
            if (intValue == (int)BossName.Unknown)
                return false;
            return enumValues.Contains(intValue);
        }

        if (Enum.TryParse(matchingValue, true, out BossName bossName))
        {
            return bossName != BossName.Unknown;
        }

        return false;
    }

}
