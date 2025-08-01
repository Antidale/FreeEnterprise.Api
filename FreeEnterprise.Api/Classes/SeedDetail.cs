namespace FreeEnterprise.Api.Classes;

public record SeedDetail(
    int SeedId,
    string Flagset,
    string Verification,
    string SourceUrl,
    string Seed,
    string Version,
    float Rank
)
{

}
