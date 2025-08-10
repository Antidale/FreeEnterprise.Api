namespace FreeEnterprise.Api.Classes;

public record RaceEntrant
{
    public string Name { get; set; } = string.Empty;
    public string TwitchName { get; set; } = string.Empty;
    public string RacetimeId { get; set; } = string.Empty;
    public TimeSpan? FinishTime { get; set; }
    public int? Placement { get; set; }
    /// <summary>
    /// A colleciton of key/value pairs, both stored as strings. Always should contain the following keys: comment, score, scoreChange. Other keys, like status, may be present depending on other factors
    /// </summary>
    public Dictionary<string, string> EntrantMetadata { get; set; } = [];

    /// <summary>
    /// Returns a new instance of this class, but ensures that score, scoreChanged, and comment all are present in the dictionary. If they were not previously included they have empty strings
    /// </summary>
    /// <returns></returns>
    public RaceEntrant EnsureExpectedMetadata()
    {
        var defaultDictionary = new Dictionary<string, string>
        {
            ["score"] = "",
            ["scoreChange"] = "",
            ["comment"] = ""
        };
        var metadata = this.EntrantMetadata.Concat(defaultDictionary.Where(kpv => !EntrantMetadata.ContainsKey(kpv.Key))).ToDictionary(x => x.Key, y => y.Value);

        return this with { EntrantMetadata = metadata };
    }
}
