using FluentAssertions;
using FreeEnterprise.Api.Classes;

namespace FreeEnterprise.Api.UnitTests.ClassesTests;

public class RaceTests
{
    [Fact]
    public void RaceDetail_Metadata_Defaults_ToEmptyDictionary()
    {
        var sut = new RaceDetail();
        sut.Metadata.Should().BeEmpty();
    }

    [Fact]
    public void RaceDetail_WithFilteredMetadata_RemovesFiterPrefix()
    {
        var presetMetadata = new Dictionary<string, string>
        {
            ["CR_RoomId"] = "1234",
            ["Status"] = "Cancelled"
        };

        var expectedMetadata = new Dictionary<string, string>
        {
            ["Status"] = "Cancelled"
        };

        var sut = new RaceDetail
        {
            Metadata = presetMetadata
        }.WithFilteredMetadata("CR_");
        sut.Metadata.Should().NotBeEmpty();
        sut.Metadata.Should().HaveCount(1);
        sut.Metadata.Should().BeEquivalentTo(expectedMetadata);
    }
}
