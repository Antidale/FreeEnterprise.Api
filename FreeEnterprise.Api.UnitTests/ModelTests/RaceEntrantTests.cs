using System.Text.Json;
using FluentAssertions;
using FluentAssertions.Execution;

namespace FreeEnterprise.Api.UnitTests.ModelTests;

public class RaceEntrantTests
{
    readonly string entrantJson = """
    {
            "user": {
                "id": "dm1LPWjGxwBEnVx6",
                "full_name": "Fleury#8244",
                "name": "Fleury",
                "discriminator": "8244",
                "url": "/user/dm1LPWjGxwBEnVx6/fleury",
                "avatar": "https://racetime.gg/media/fleury-talk.png",
                "pronouns": "he/him",
                "flair": "",
                "twitch_name": "lqfleury14",
                "twitch_display_name": "LQFleury14",
                "twitch_channel": "https://www.twitch.tv/lqfleury14",
                "can_moderate": false
            },
            "team": null,
            "status": {
                "value": "forfiet",
                "verbose_value": "DNF",
                "help_text": ".ff"
            },
            "finish_time": "P0DT01H34M42.116959S",
            "finished_at": "2025-07-11T01:47:01.388Z",
            "place": 3,
            "place_ordinal": "3rd",
            "score": 1875,
            "score_change": -9,
            "comment": "comment here",
            "has_comment": false,
            "stream_live": false,
            "stream_override": false,
            "actions": []
        }
""";

    [Fact]
    public void ToRaceEntrant_PopulatesDataCorrectly()
    {
        var sut = JsonSerializer.Deserialize<RtggModels.Entrant>(entrantJson, JsonSerializerOptions.Web)
                                .ToRaceEntrant("fake url");

        sut.room_name.Should().Be("fake url");
        sut.placement.Should().Be(3);
        sut.racetime_id.Should().Be("dm1LPWjGxwBEnVx6");
        sut.finish_time.Should().Be("P0DT01H34M42.116959S");
        //testing metadata in a seperate test
    }

    [Theory]
    [InlineData("forfeit")]
    [InlineData("dq")]
    public void ToRaceEntrant_PopulatesMetadataForNotDoneStatuses(string statusValue)
    {
        var entrant = JsonSerializer.Deserialize<RtggModels.Entrant>(entrantJson, JsonSerializerOptions.Web);

        entrant.Status.Value = statusValue;
        var sut = entrant.ToRaceEntrant("fake url");

        var expectedKeys = new List<string>
        {
            "score",
            "scoreChange",
            "comment",
            "status"
        };

        sut.metadata.Keys.Should().HaveCount(expectedKeys.Count);
        sut.metadata.Keys.Should().BeEquivalentTo(expectedKeys);

        using (new AssertionScope("Score Metadata"))
        {
            sut.metadata.TryGetValue("score", out var score)
                        .Should().BeTrue();
            score.Should().Be(1875.ToString());
        }

        using (new AssertionScope("scoreChange"))
        {
            sut.metadata.TryGetValue("scoreChange", out var scoreChange).Should().BeTrue();

            scoreChange.Should().Be((-9).ToString());
        }

        using (new AssertionScope("comment"))
        {
            sut.metadata.TryGetValue("comment", out var comment).Should().BeTrue();
            comment.Should().Be("comment here");
        }

        using (new AssertionScope("status"))
        {
            sut.metadata.TryGetValue("status", out var status).Should().BeTrue();
            status.Should().Be(statusValue);
        }
    }

    [Fact]
    public void ToRaceEntrant_PopulatesMetadataCorrectlyForDone()
    {
        var entrant = JsonSerializer.Deserialize<RtggModels.Entrant>(entrantJson, JsonSerializerOptions.Web);

        entrant.Status.Value = "Done";
        var sut = entrant.ToRaceEntrant("fake url");

        var expectedKeys = new List<string>
        {
            "score",
            "scoreChange",
            "comment"
        };

        sut.metadata.Keys.Should().HaveCount(expectedKeys.Count);
        sut.metadata.Keys.Should().BeEquivalentTo(expectedKeys);

        using (new AssertionScope("Score Metadata"))
        {
            sut.metadata.TryGetValue("score", out var score)
                        .Should().BeTrue();
            score.Should().Be(1875.ToString());
        }

        using (new AssertionScope("scoreChange"))
        {
            sut.metadata.TryGetValue("scoreChange", out var scoreChange).Should().BeTrue();

            scoreChange.Should().Be((-9).ToString());
        }

        using (new AssertionScope("comment"))
        {
            sut.metadata.TryGetValue("comment", out var comment).Should().BeTrue();
            comment.Should().Be("comment here");
        }

        using (new AssertionScope("status"))
        {
            sut.metadata.TryGetValue("status", out var status).Should().BeFalse();
        }
    }

}
