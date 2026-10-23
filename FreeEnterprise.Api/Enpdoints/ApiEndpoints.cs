namespace FreeEnterprise.Api.Enpdoints;

/* 
    TODO: After this initial refactor to Minimal API,
    make a consistency decsion on singular vs plural route names.

    I currently plan on going to plural (e.g. api/Racers), but will read a bit more of the singular approach arguments to see if they make me reconsider.
*/
public class ApiEndpoints
{
    private const string ApiBase = "api";

    public static class Races
    {
        private const string Base = $"{ApiBase}/races";
        public const string Create = Base;
        public const string GetAll = Base;
        public const string Get = $"{Base}/{{idOrSlug}}";
        public const string GetRaceSeed = $"{Get}/seed";
    }

    public static class BattleLocations
    {
        private const string Base = $"{ApiBase}/BattleLocation";
        public const string GetAll = Base;
    }

    public static class BossBattle
    {
        private const string Base = $"{ApiBase}/BossBattle";
        public const string GetAll = Base;
    }

    public static class BossStats
    {
        private const string Base = $"{ApiBase}/BossStats";
        public const string GetAll = Base;
    }

    public static class Entrants
    {
        private const string Base = $"{ApiBase}/Entrant";
        public const string UpdateTwitch = $"{Base}/UpdateTwitch";
        public const string UpdatePronouns = $"{Base}/UpdatePronouns";
        public const string UpdateAlias = $"{Base}/UpdateAlias";
    }

    public static class Equipment
    {
        private const string Base = $"{ApiBase}/Equipment";
        public const string GetAll = Base;
        public const string GetAllArmor = $"{Base}/Armor";
        public const string GetArmorById = $"{Base}/Armor/{{id:int}}";
        public const string GetAllWeapons = $"{Base}/Weapons";
        public const string GetWeaponById = $"{Base}/Weapons/{{id:int}}";
    }

    public static class Guides
    {
        private const string Base = $"{ApiBase}/Guide";
        public const string Search = Base;
        public const string BossStrat = $"{Base}/boss-strategies/{{bossName:bossNameEnum}}";
    }

    public static class Racers
    {
        private const string Base = $"{ApiBase}/Racers";
        public const string GetAll = Base;
        public const string GetByIdOrName = $"{Base}/{{idOrName}}";
        public const string GetRacerRaces = $"{GetByIdOrName}/races";
        public const string CompareRacers = $"{GetByIdOrName}/head-to-head/{{opponentIdOrName}}";
        public const string SearchRacetimeUser = $"{Base}/racetime-search";
    }

    public static class Seeds
    {
        private const string Base = $"{ApiBase}/Seed";
        public const string SearchSeeds = Base;
        public const string GetById = $"{Base}/{{id:int}}";
        public const string GetSeedHtml = $"{GetById}/html";
    }

    public static class Tournament
    {
        private const string Base = $"{ApiBase}/Tournament";
        public const string GetAll = Base;
        public const string GetTournamentRegistrations = $"{Base}/{{id:int}}/registrants";
        public const string Create = Base;
        public const string UpdateRegistrationWindow = $"{Base}/UpdateRegistrationWindow";
        public const string Register = $"{Base}/Register";
        public const string Drop = $"{Base}/Drop";
    }

    public static class Test
    {
        private const string Base = $"{ApiBase}/Test";
        public const string Create = Base;
        public const string GetAll = Base;
        public const string Get = $"Base/{{idOrSlug}}";
    }
}
