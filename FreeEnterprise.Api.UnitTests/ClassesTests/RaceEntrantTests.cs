using FluentAssertions;
using FreeEnterprise.Api.Classes;

namespace FreeEnterprise.Api.UnitTests.ClassesTests;

public class RaceEntrantTests
{
    [Fact]
    public void RaceEntrant_Defaults_ToEmptyDictioanry()
    {
        var sut = new RaceEntrant();
        sut.EntrantMetadata.Should().BeEmpty();
    }

    [Fact]
    public void RaceEntrant_EnsureExpectedMetadata_AddsExpectedKeys_ToDefault()
    {
        var sut = new RaceEntrant().EnsureExpectedMetadata();
        sut.EntrantMetadata.Should().NotBeEmpty();
        sut.EntrantMetadata.Should().HaveCount(3);
        sut.EntrantMetadata.Should().BeEquivalentTo(new Dictionary<string, string>
        {
            ["score"] = "",
            ["scoreChange"] = "",
            ["comment"] = ""
        });
    }

    [Fact]
    public void RaceEntrant_EnsureExpectedMetadata_ShouldNotOverride_ExistingSharedKeys()
    {
        var presetDictioanry = new Dictionary<string, string>
        {
            ["score"] = "1234",
            ["scoreChange"] = "234",
            ["comment"] = "oatly is made from oat-people"
        };

        var sut = new RaceEntrant
        {
            EntrantMetadata = presetDictioanry
        }.EnsureExpectedMetadata();
        sut.EntrantMetadata.Should().NotBeEmpty();
        sut.EntrantMetadata.Should().HaveCount(3);
        sut.EntrantMetadata.Should().BeEquivalentTo(presetDictioanry);
    }

    [Fact]
    public void RaceEntrant_EnsureExpectedMetadata_WillFillInMissingKeys()
    {
        var presetDictioanry = new Dictionary<string, string>
        {
            ["score"] = "1234",
            ["comment"] = "oatly is made from oat-people"
        };

        var sut = new RaceEntrant
        {
            EntrantMetadata = presetDictioanry
        }.EnsureExpectedMetadata();
        sut.EntrantMetadata.Should().NotBeEmpty();
        sut.EntrantMetadata.Should().HaveCount(3);
        sut.EntrantMetadata.Should().BeEquivalentTo(new Dictionary<string, string>
        {
            ["score"] = "1234",
            ["scoreChange"] = "",
            ["comment"] = "oatly is made from oat-people"
        });
    }

    [Fact]
    public void RaceEntrant_EnsureExpectedMetadata_DoesNotRemoveKeys()
    {
        var presetDictioanry = new Dictionary<string, string>
        {
            ["test_score"] = "1234",
            ["test_scoreChange"] = "234",
            ["test_comment"] = "oatly is made from oat-people"
        };

        var expectedDictionary = new Dictionary<string, string>
        {
            ["test_score"] = "1234",
            ["test_scoreChange"] = "234",
            ["test_comment"] = "oatly is made from oat-people",
            ["score"] = "",
            ["scoreChange"] = "",
            ["comment"] = ""
        };

        var sut = new RaceEntrant
        {
            EntrantMetadata = presetDictioanry
        }.EnsureExpectedMetadata();
        sut.EntrantMetadata.Should().NotBeEmpty();
        sut.EntrantMetadata.Should().HaveCount(expectedDictionary.Count);
        sut.EntrantMetadata.Should().BeEquivalentTo(expectedDictionary);
    }
}
