using Dapper;
using FeInfo.Common.DTOs;
using FeInfo.Common.Emums;
using FeInfo.Common.Requests;
using FeInfo.Common.Responses;
using FreeEnterprise.Api.Classes;
using FreeEnterprise.Api.Constants;
using FreeEnterprise.Api.Interfaces;
using FreeEnterprise.Api.Models;
using System.Data;
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

        public async Task<Response<ChangeRegistrationPeriodResponse>> UpdateRegistrationWindow(ChangeRegistrationPeriod request)
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

                    return new Response<ChangeRegistrationPeriodResponse>().SetError(message, HttpStatusCode.BadRequest);
                }

                var tournament = searchResult.First();

                sql = GetRegistrationUpdateSql(request.NewStatus);

                var rowCount = await connection.ExecuteAsync(sql, new { tournament.id });

                _ = ulong.TryParse(tournament.tracking_channel_id, out var channelId);
                _ = ulong.TryParse(tournament.tracking_message_id, out var messageId);

                return new Response<ChangeRegistrationPeriodResponse>(
                    new ChangeRegistrationPeriodResponse(channelId, messageId, request.NewStatus),
                    success: true
                );

            }
            catch (Exception ex)
            {
                return new Response<ChangeRegistrationPeriodResponse>().SetError(ex.Message, HttpStatusCode.InternalServerError);
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

        public async Task<Response<ChangeRegistrationResponse>> RegisterPlayerAsync(ChangeRegistration request)
        {
            using var connection = _connectionProvider.GetConnection();
            connection.Open();
            try
            {
                var entrantId = await UpsertEntrantAsync(connection, new Entrant(request.UserId.ToString(), request.UserName, request.Pronouns));

                if (entrantId == 0)
                {
                    return new Response<ChangeRegistrationResponse>().SetError("Error adding entrant", HttpStatusCode.InternalServerError);
                }

                var tournaments = await GetOpenTournamentsAsync(connection, request.GuildId.ToString(), request.TournamentName);

                if (tournaments == null || tournaments.Count == 0)
                {
                    return new Response<ChangeRegistrationResponse>().SetError("No open tournaments to join were found", HttpStatusCode.NotFound);
                }

                var openTournamentIds = tournaments.Select(x => x.id).ToList();

                var registrations = (await connection.QueryAsync<Models.Registration>("select * from tournament.registrations where entrant_id = @entrantId and tournament_id = Any(@openTournamentIds);",
                    new { entrantId, openTournamentIds })).ToList();
                var registrationTournamentIds = registrations.Select(x => x.tournament_id).ToList();

                var unregisteredTournaments = tournaments.Where(x => !registrationTournamentIds.Contains(x.id)).ToList();

                var junk = unregisteredTournaments.Count switch
                {
                    0 => ("No open tournaments to join were found", HttpStatusCode.NotFound),
                    1 => (string.Empty, HttpStatusCode.OK),
                    > 1 => ($"More than one possible tournament is open, please resubmit with a tournament name. Open tournaments in this server: {string.Join(", ", unregisteredTournaments.Select(x => x.tournament_name))}", HttpStatusCode.Conflict),
                    _ => ("Unexpected Result", HttpStatusCode.InternalServerError)
                };

                if (!string.IsNullOrEmpty(junk.Item1))
                {
                    return new Response<ChangeRegistrationResponse>().SetError(junk.Item1, junk.Item2);
                }

                var tournament = unregisteredTournaments.First();

                var insertSql =
@"insert into tournament.registrations(tournament_id, entrant_id, user_name_alias)
VALUES
(@tournamentId, @entrantId, @userNameAlias);";

                var userNameAlias = string.IsNullOrWhiteSpace(request.Alias) ? request.UserName : request.Alias;
                var insertCount = await connection.ExecuteAsync(insertSql, new { tournamentId = tournament.id, entrantId, userNameAlias });
                if (insertCount != 1)
                {
                    return new Response<ChangeRegistrationResponse>().SetError("registration failed", HttpStatusCode.InternalServerError);
                }

                var entrantCount = await connection.ExecuteScalarAsync<int>("select count(*) from tournament.registrations where tournament_id = @id", new { tournament.id });

                _ = ulong.TryParse(tournament.tracking_channel_id, out var channelId);
                _ = ulong.TryParse(tournament.tracking_message_id, out var messageId);
                _ = ulong.TryParse(tournament.role_id, out var roleId);

                return new Response<ChangeRegistrationResponse>().SetSuccess(
                    new ChangeRegistrationResponse(
                        entrantCount,
                        channelId,
                        messageId,
                        roleId,
                        tournament.tournament_name));
            }
            catch (Exception ex)
            {
                return new Response<ChangeRegistrationResponse>().SetError(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Response<ChangeRegistrationResponse>> DropPlayerAsync(ChangeRegistration request)
        {
            await Task.Delay(1);
            throw new NotImplementedException();

            using var connection = _connectionProvider.GetConnection();
            connection.Open();
            try
            {
                var sql =
@"select * 
from tournament.tournament_registrations 
where user_id = @UserId
and guild_id = @GuildId
and (tournament_name = @TournamentName or @TournamentName = '')
and registration_start >= now()
and (registration_end is null or registration_end < now())
;";

                var registrations = (await connection.QueryAsync<TournamentRegistration>(sql, new { request.UserId, request.TournamentName, GuildId = request.GuildId.ToString() })).ToList();


                if (registrations.Count != 1)
                {
                    var message = registrations.Count == 0
                        ? ($"No tournaments can have their registration {request.DesiredStatus}", HttpStatusCode.NotFound)
                        : ($"There are multiple tournaments that could be {request.DesiredStatus}, please specify a tournament name {string.Join(", ", registrations.Select(x => x.tournament_name).Distinct())}", HttpStatusCode.Conflict);

                    return new Response<ChangeRegistrationResponse>().SetError(message.Item1, message.Item2);
                }

                //var deleteResponse = "";
            }
            catch (Exception ex)
            {
                return new Response<ChangeRegistrationResponse>().SetError(ex.Message, HttpStatusCode.InternalServerError);
            }

            
        }

        public async Task<List<Tournament>> GetOpenTournamentsAsync(IDbConnection connection, string guildId = "", string tournamentName = "")
        {
            var tournamentSearchSql = TournamentRegistrationConstants.SearchTournamentsForRegistration;
            var searchObject = new
            {
                GuildId = guildId.ToString(),
                TournamentName = tournamentName
            };

            return (await connection.QueryAsync<Tournament>(tournamentSearchSql, searchObject)).ToList();
        }


        public async Task<Response<List<TournamentSummary>>> GetTournamentSummariesAsync()
        {
            using var connection = _connectionProvider.GetConnection();
            connection.Open();

            try
            {
                var summaries = await connection.QueryAsync<TournamentSummary>(TournamentRegistrationConstants.GetTournamentSummarySql);
                return new Response<List<TournamentSummary>>().SetSuccess(summaries.ToList());
            }
            catch (Exception ex)
            {
                return new Response<List<TournamentSummary>>().SetError(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        private async Task<int> UpsertEntrantAsync(IDbConnection connection, Entrant entrant)
        {
            var entrantEntity = await GetEntrantByUserIdAsync(connection, entrant.user_id);

            if (entrantEntity == null)
            {
                return await connection.ExecuteScalarAsync<int>(TournamentRegistrationConstants.InsertEntrantSql,
                    new { UserId = entrant.user_id, UserName = entrant.user_name, Pronouns = entrant.pronouns });
            }

            var updateSql = getUpdateSql(entrant, entrantEntity);

            if(!string.IsNullOrEmpty(updateSql))
            {
                await connection.ExecuteAsync(updateSql, new { entrant.user_name, entrant.pronouns, entrantEntity.id });
            }

            return entrantEntity.id;

            static string getUpdateSql(Entrant request, Entrant entity)
            {
                if (request.user_name.Equals(entity.user_name, StringComparison.InvariantCulture) &&
                    request.pronouns.Equals(entity.pronouns, StringComparison.InvariantCulture))
                {
                    return string.Empty;
                }

                return $@"update tournament.entrants set user_name = @user_name, pronouns = @pronouns where id = @id";
            }
        }

        #region Private Methods

        public async Task<Entrant?> GetEntrantByUserIdAsync(IDbConnection connection, string userId)
        {
            return await connection.QueryFirstOrDefaultAsync<Entrant>(TournamentRegistrationConstants.GetEntrantByUserId, new { userId });
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

        #endregion
    }
}
