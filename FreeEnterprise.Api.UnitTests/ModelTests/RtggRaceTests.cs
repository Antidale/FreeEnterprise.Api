using System;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using FreeEnterprise.Api.RtggModels;

namespace FreeEnterprise.Api.UnitTests.ModelTests;

public class RtggRaceTests
{
    string jsonText = """
{
    "name": "ff4fe/hyper-silvera-5340",
    "status": {
        "value": "finished",
        "verbose_value": "Finished",
        "help_text": "This race has been completed"
    },
    "url": "/ff4fe/hyper-silvera-5340",
    "data_url": "/ff4fe/hyper-silvera-5340/data",
    "goal": {
        "name": "Beat Zeromus",
        "custom": false
    },
    "info": "doors pickup\nhttps://ff4fe.galeswift.com/get?id=bBAYBgAUAAAAAAAAAAAAAAAAAAIAAAAB4AwBIAQAAAQAAAABgAhAgAAAAUAAABJAAFQBcKL0ACAICAIAB.YF38EVF2AF\nTent/Star/Potion/Staff",
    "entrants_count": 4,
    "entrants_count_finished": 4,
    "entrants_count_inactive": 0,
    "opened_at": "2025-07-10T23:39:35.331Z",
    "started_at": "2025-07-11T00:12:19.271Z",
    "time_limit": "P1DT00H00M00S",
    "opened_by_bot": null,
    "entrants": [
        {
            "user": {
                "id": "Va0eMonEYM3l9pyJ",
                "full_name": "judgejoe00#6305",
                "name": "judgejoe00",
                "discriminator": "6305",
                "url": "/user/Va0eMonEYM3l9pyJ/judgejoe00",
                "avatar": null,
                "pronouns": "he/him",
                "flair": "",
                "twitch_name": "judgejoe00",
                "twitch_display_name": "judgejoe00",
                "twitch_channel": "https://www.twitch.tv/judgejoe00",
                "can_moderate": false
            },
            "team": null,
            "status": {
                "value": "done",
                "verbose_value": "Finished",
                "help_text": "Finished the race."
            },
            "finish_time": "P0DT01H25M06.484013S",
            "finished_at": "2025-07-11T01:37:25.755Z",
            "place": 1,
            "place_ordinal": "1st",
            "score": 2172,
            "score_change": 62,
            "comment": null,
            "has_comment": false,
            "stream_live": false,
            "stream_override": false,
            "actions": []
        },
        {
            "user": {
                "id": "57ZKD3gXleokyANO",
                "full_name": "Inven#8827",
                "name": "Inven",
                "discriminator": "8827",
                "url": "/user/57ZKD3gXleokyANO/inven",
                "avatar": "https://racetime.gg/media/AYAYA-56.png",
                "pronouns": "he/him",
                "flair": "",
                "twitch_name": "inven",
                "twitch_display_name": "inven",
                "twitch_channel": "https://www.twitch.tv/inven",
                "can_moderate": false
            },
            "team": null,
            "status": {
                "value": "done",
                "verbose_value": "Finished",
                "help_text": "Finished the race."
            },
            "finish_time": "P0DT01H29M14.781716S",
            "finished_at": "2025-07-11T01:41:34.053Z",
            "place": 2,
            "place_ordinal": "2nd",
            "score": 1229,
            "score_change": 359,
            "comment": null,
            "has_comment": false,
            "stream_live": true,
            "stream_override": false,
            "actions": []
        },
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
                "value": "done",
                "verbose_value": "Finished",
                "help_text": "Finished the race."
            },
            "finish_time": "P0DT01H34M42.116959S",
            "finished_at": "2025-07-11T01:47:01.388Z",
            "place": 3,
            "place_ordinal": "3rd",
            "score": 1875,
            "score_change": -9,
            "comment": null,
            "has_comment": false,
            "stream_live": false,
            "stream_override": false,
            "actions": []
        },
        {
            "user": {
                "id": "XzVwZWqV0RB5k8eb",
                "full_name": "skarcerer#6386",
                "name": "skarcerer",
                "discriminator": "6386",
                "url": "/user/XzVwZWqV0RB5k8eb/skarcerer",
                "avatar": "https://racetime.gg/media/BogWitch.gif",
                "pronouns": "they/them",
                "flair": "moderator",
                "twitch_name": "skarcerer",
                "twitch_display_name": "Skarcerer",
                "twitch_channel": "https://www.twitch.tv/skarcerer",
                "can_moderate": true
            },
            "team": null,
            "status": {
                "value": "done",
                "verbose_value": "Finished",
                "help_text": "Finished the race."
            },
            "finish_time": "P0DT01H47M45.275482S",
            "finished_at": "2025-07-11T02:00:04.546Z",
            "place": 4,
            "place_ordinal": "4th",
            "score": 1457,
            "score_change": -38,
            "comment": null,
            "has_comment": false,
            "stream_live": true,
            "stream_override": false,
            "actions": []
        }
    ],
    "ended_at": "2025-07-11T02:00:04.597Z",
    "cancelled_at": null,
    "recordable": true,
    "recorded": true
}
""";

    Race sut;

    public RtggRaceTests()
    {
        sut = JsonSerializer.Deserialize<Race>(jsonText, JsonSerializerOptions.Web);
    }

    [Fact]
    public async Task RaceDeserialzesCorrectly()
    {
        sut.Should().NotBeNull();

        sut.Name.Should().Be("ff4fe/hyper-silvera-5340");
        sut.Goal.Name.Should().Be("Beat Zeromus");
        sut.Info.Should().Be("doors pickup\nhttps://ff4fe.galeswift.com/get?id=bBAYBgAUAAAAAAAAAAAAAAAAAAIAAAAB4AwBIAQAAAQAAAABgAhAgAAAAUAAABJAAFQBcKL0ACAICAIAB.YF38EVF2AF\nTent/Star/Potion/Staff");
        sut.Recordable.Should().Be(true);
        sut.Recorded.Should().Be(true);
        sut.Status.Value.Should().Be("finished");
    }

    [Fact]
    public void ToRaceModel_StripsNamePrefixCorrectly()
    {
        var model = sut.ToRaceModel();
        model.room_name.Should().NotBe(sut.Name);
        model.room_name.Should().Be("hyper-silvera-5340");
    }

    [Fact]
    public void ToRaceModel_StripsUrlFromInfo()
    {
        var model = sut.ToRaceModel();
        model.metadata.TryGetValue("Description", out var description);
        description.Should().NotContain("http");
        description.Should().Be("doors pickup Tent/Star/Potion/Staff");
    }

    [Fact]
    public void ToCreateEntrantModels_MakesModelsCorrectly()
    {
        var entrants = sut.ToCreateEntrantModels();
        entrants.Should().HaveCount(4);
        entrants.Select(x => x.room_name).Distinct().Should().HaveCount(1, "There should be a single room name associated with the race");
        entrants.First().room_name.Should().NotStartWith("ff4fe/");
        entrants.Select(x => x.room_name).Distinct().Should().NotBeEmpty();
    }
}
