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
            var rowCount = await connection.ExecuteAsync(updateSql, new { pronouns = updatePronouns.Pronouns, id = updatePronouns.UserId});

            return rowCount == 0 ? new Response().NotFound("User not found") : new Response().SetSuccess();
        }
        catch (Exception ex)
        {
            return new Response().InternalServerError(ex.Message);
        }
    }
}