using Dapper;
using FeInfo.Common.Emums;
using FeInfo.Common.Requests;
using FeInfo.Common.Responses;
using FreeEnterprise.Api.Classes;
using FreeEnterprise.Api.Constants;
using FreeEnterprise.Api.Interfaces;
using FreeEnterprise.Api.Models;
using Microsoft.OpenApi.Services;
using System.Net;

namespace FreeEnterprise.Api.Repositories
{
    public class TournamentRepository : ITournamentRepository
    {
        private readonly IConnectionProvider _connectionProvider;

        public TournamentRepository(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public async Task<Response<RegistrationPeriodChangeResponse>> UpdateRegistrationWindow(RegistrationPeriodStatusChange request)
        {
            using var connection = _connectionProvider.GetConnection();
            connection.Open();
            try
            {
                var searchObject = new
                {
                    GuildId = request.GuildId.ToString(),
                    request.TournamentName
                };

                var sql = GetRegistrationStatusChangeSql(request.NewStatus);

                var searchResult = (await connection.QueryAsync<Tournament>(sql, searchObject)).ToList();
                if (searchResult.Count != 1)
                {
                    var message = searchResult.Count == 0
                        ? $"No tournaments can have their registration {request.NewStatus}"
                        : $"There are multiple tournaments that could be {request.NewStatus}, please specify a tournament name";

                    return new Response<RegistrationPeriodChangeResponse>().SetError(message, HttpStatusCode.BadRequest);
                }

                var tournament = searchResult.First();

                sql = GetRegistrationUpdateSql(request.NewStatus);

                var rowCount = await connection.ExecuteAsync(sql, new { tournament.id });

                _ = ulong.TryParse(tournament.tracking_channel_id, out var channelId);
                _ = ulong.TryParse(tournament.tracking_message_id, out var messageId);

                return new Response<RegistrationPeriodChangeResponse>(
                    new RegistrationPeriodChangeResponse(channelId, messageId, request.NewStatus),
                    success: true
                );

            }
            catch (Exception ex)
            {
                return new Response<RegistrationPeriodChangeResponse>().SetError(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Response<int>> CreateTournamentAsync(CreateTournament createTournamentRequest)
        {
            using var connection = _connectionProvider.GetConnection();
            connection.Open();
            try
            {
                var tournamentObject = new
                {
                    GuildId = createTournamentRequest.GuildId.ToString(),
                    createTournamentRequest.GuildName,
                    ChannelId = createTournamentRequest.TrackingChannelId.ToString(),
                    MessageId = createTournamentRequest.TrackingMessageId.ToString(),
                    createTournamentRequest.TournamentName,
                    RoleId = createTournamentRequest.RegistrantRoleId.ToString(),
                    RegistrationStart = createTournamentRequest.RegistrationStart?.ToUniversalTime() ?? null,
                    RegistrationEnd = createTournamentRequest.RegistrationEnd?.ToUniversalTime() ?? null,
                };

                var sql = 
$@"Insert Into tournament.tournaments(
    {nameof(Tournament.guild_id)},
    {nameof(Tournament.guild_name)},
    {nameof(Tournament.tracking_channel_id)},
    {nameof(Tournament.tracking_message_id)},
    {nameof(Tournament.tournament_name)},
    {nameof(Tournament.role_id)},
    {nameof(Tournament.registration_start)},
    {nameof(Tournament.registration_end)}
)
Values(@GuildId, @GuildName, @ChannelId, @MessageId, @TournamentName, @RoleId, @RegistrationStart, @RegistrationEnd);";
                var insertResponse = await connection.ExecuteAsync(sql, tournamentObject);
                return new Response<int>(insertResponse, success: true);
            }
            catch (Exception ex)
            {
                return new Response<int>(0, errorMessage: ex.Message, errorStatusCode: HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Response<RegistrationResponse>> RegisterPlayerAsync(Registration request)
        {
            using var connection = _connectionProvider.GetConnection();
            connection.Open();
            try
            {
                var tournamentSearchSql = TournamentRegistrationConstants.SearchTournamentsForRegistration;
                var searchObject = new
                {
                    GuildId = request.GuildId.ToString(),
                    request.TournamentName
                };

                var searchResult = (await connection.QueryAsync<Tournament>(tournamentSearchSql, searchObject)).ToList();

                if (searchResult.Count != 1)
                {
                    var message = searchResult.Count == 0
                        ? $"No tournaments accepting registration found"
                        : $"There are multiple tournaments with open registration, please specify a tournament name";

                    return new Response<RegistrationResponse>().SetError(message, HttpStatusCode.BadRequest);
                }

                var entrant = connection.QueryFirstOrDefaultAsync<Entrant>(
$@"select
      {nameof(Entrant.id)}
    , {nameof(Entrant.user_id)}
    , {nameof(Entrant.user_name)}
    , {nameof(Entrant.pronouns)}
from tournament.tournaments
where user_id = @UserId
",
                    new { request.UserId });

                var insertSql =
$@"INSERT INTO tournament.entrants(user_id, user_name, pronouns)
VALUES
(@UserId, @UserName, @Pronouns)
Returning id;
";
            }
            catch(Exception ex) 
            { 
                return new Response<RegistrationResponse>().SetError(ex.Message, HttpStatusCode.InternalServerError);
            }


            var stuff = new Response<RegistrationResponse>();
                throw new NotImplementedException();
        }


        public Task<Response<RegistrationResponse>> DropPlayerAsync(Registration registration)
        {
            throw new NotImplementedException();
        }

        private static string GetRegistrationStatusChangeSql(RegistrationPeriodStatus desiredStatus)
        {
            return desiredStatus switch
            {
                RegistrationPeriodStatus.Opened => TournamentRegistrationConstants.SearchRegistrationWindowForOpeningSql,

                RegistrationPeriodStatus.Closed => TournamentRegistrationConstants.SearchRegistrationWindowForClosingSql,
                
                _ => string.Empty,
            };
        }

        private static string GetRegistrationUpdateSql(RegistrationPeriodStatus desiredStatus)
        {
            return desiredStatus switch
            {
                RegistrationPeriodStatus.Opened => TournamentRegistrationConstants.OpenRegistrationWindowSql,

                RegistrationPeriodStatus.Closed => TournamentRegistrationConstants.CloseRegistrationWindowSql,

                _ => string.Empty
            };
        }
    }
}
