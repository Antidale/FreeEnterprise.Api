using System;

namespace FreeEnterprise.Api.Classes;

public record RaceEntrant
{
    public string Name { get; set; } = string.Empty;
    public string TwitchName { get; set; } = string.Empty;
    public TimeSpan? FinishTime { get; set; }
    public int? Placement { get; set; }
    public Dictionary<string, string> Metadata { get; set; } = [];

    /// <summary>
    /// Returns a new instance of this class, but ensures that score, scoreChanged, and comment all are present in the dictionary. If they were not previously included they have empty strings
    /// <returns></returns>
    public RaceEntrant EnsureExpectedMetadata()
    {
        var defaultDictionary = new Dictionary<string, string>
        {
            ["score"] = "",
            ["scoreChange"] = "",
            ["comment"] = ""
        };
        var metadata = this.Metadata.Concat(defaultDictionary.Where(kpv => !Metadata.ContainsKey(kpv.Key))).ToDictionary(x => x.Key, y => y.Value);

        return this with { Metadata = metadata };
    }
}
