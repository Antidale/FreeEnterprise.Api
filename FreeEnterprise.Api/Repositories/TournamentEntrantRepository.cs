using Dapper;
using FeInfo.Common.Requests;
using FreeEnterprise.Api.Classes;
using FreeEnterprise.Api.Interfaces;
using FreeEnterprise.Api.Models;

namespace FreeEnterprise.Api.Repositories;

public class EntrantRepository(IConnectionProvider connectionProvider) : IEntrantRepository
{
    public async Task<Response> UpdatePronounsAsync(UpdatePronouns updatePronouns)
    {
        using var connection = connectionProvider.GetConnection();
        connection.Open();

        const string updateSql = $"""
                                  update tournament.entrants
                                  set pronouns = @pronouns
                                  where {nameof(Entrant.user_id)} = @id
                                  """;
        try
        {
            var rowCount = await connection.ExecuteAsync(updateSql, new { pronouns = updatePronouns.Pronouns, id = updatePronouns.UserId.ToString() });

            return rowCount == 0 ? new Response().NotFound("User not found") : Response.SetSuccess();
        }
        catch (Exception ex)
        {
            return new Response().InternalServerError(ex.Message);
        }
    }

    public async Task<Response> UpdateAliasAsync(UpdateAlias updateAlias)
    {
        using var connection = connectionProvider.GetConnection();
        connection.Open();

        const string updateSql = $"""
                                  update tournament.registrations r
                                  set user_name_alias = @alias
                                  from tournament.tournament_registrations tr
                                  where tr.tournament_id = r.tournament_id 
                                  and tr.user_id = @userId
                                  and tr.entrant_id = r.entrant_id
                                  and (@tournamentName = '' 
                                      or tr.tournament_name = @tournamentName)
                                  """;
        var searchParams = new
        {
            alias = updateAlias.Alias,
            userId = updateAlias.UserId.ToString(),
            tournamentName = updateAlias.TournamentName
        };

        try
        {
            var rowCount = await connection.ExecuteAsync(updateSql, searchParams);

            return rowCount == 0 ? new Response().NotFound("No updates made") : Response.SetSuccess();
        }
        catch (Exception ex)
        {
            return new Response().InternalServerError(ex.Message);
        }
    }

    public async Task<Response> UpdateTwitchAsync(UpdateTwitch updateTwitch)
    {
        using var connection = connectionProvider.GetConnection();
        connection.Open();

        const string updateSql = $"""
                                  update tournament.entrants
                                  set {nameof(Entrant.twitch_name)} = @twitchName
                                  where {nameof(Entrant.user_id)} = @id
                                  """;
        var searchParams = new
        {
            twitchName = updateTwitch.TwitchName,
            id = updateTwitch.UserId.ToString()
        };

        try
        {
            var rowCount = await connection.ExecuteAsync(updateSql, searchParams);

            return rowCount == 0 ? new Response().NotFound("No updates made") : Response.SetSuccess();
        }
        catch (Exception ex)
        {
            return new Response().InternalServerError(ex.Message);
        }
    }
}