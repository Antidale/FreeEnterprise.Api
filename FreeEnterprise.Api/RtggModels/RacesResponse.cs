namespace FreeEnterprise.Api.RtggModels;

public record class RacesResponse
{
    public int Count { get; set; }
    public int NumPages { get; set; }
    public List<Race> Races { get; set; } = [];
}
