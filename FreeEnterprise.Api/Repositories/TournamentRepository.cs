using Dapper;
using FeInfo.Common.DTOs;
using FeInfo.Common.Enums;
using FeInfo.Common.Requests;
using FeInfo.Common.Responses;
using FreeEnterprise.Api.Classes;
using FreeEnterprise.Api.Constants;
using FreeEnterprise.Api.Interfaces;
using FreeEnterprise.Api.Models;
using System.Data;

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

                    return new Response<ChangeRegistrationPeriodResponse>().BadRequest(message);
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
                return new Response<ChangeRegistrationPeriodResponse>().InternalServerError(ex.Message);
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
                    createTournamentRequest.RulesLink,
                    createTournamentRequest.StandingsLink,
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
    {nameof(Tournament.rules_link)},
    {nameof(Tournament.standings_link)},
    {nameof(Tournament.role_id)},
    {nameof(Tournament.registration_start)},
    {nameof(Tournament.registration_end)}
)
Values(@GuildId, @GuildName, @ChannelId, @MessageId, @TournamentName, @RulesLink, @StandingsLink, @RoleId, @RegistrationStart, @RegistrationEnd);";
                var insertResponse = await connection.ExecuteAsync(sql, tournamentObject);
                return new Response<int>(insertResponse, success: true);
            }
            catch (Exception ex)
            {
                return new Response<int>().InternalServerError(ex.Message);
            }
        }

        public async Task<Response<ChangeRegistrationResponse>> RegisterPlayerAsync(ChangeRegistration request)
        {
            using var connection = _connectionProvider.GetConnection();
            connection.Open();
            try
            {
                var entrantId = await UpsertEntrantAsync(connection, new Entrant(request.UserId.ToString(), request.UserName, request.Pronouns, request.TwitchName));

                if (entrantId == 0)
                {
                    return new Response<ChangeRegistrationResponse>().InternalServerError("Error adding entrant");
                }

                var tournaments = await GetOpenTournamentsAsync(connection, request.GuildId.ToString(), request.TournamentName);

                if (tournaments == null || tournaments.Count == 0)
                {
                    return new Response<ChangeRegistrationResponse>().NotFound("No open tournaments to join were found");
                }

                var openTournamentIds = tournaments.Select(x => x.id).ToList();

                var registrations = (await connection.QueryAsync<Models.Registration>("select * from tournament.registrations where entrant_id = @entrantId and tournament_id = Any(@openTournamentIds);",
                    new { entrantId, openTournamentIds })).ToList();
                var registrationTournamentIds = registrations.Select(x => x.tournament_id).ToList();

                var unregisteredTournaments = tournaments.Where(x => !registrationTournamentIds.Contains(x.id)).ToList();

                var responseObject = unregisteredTournaments.Count switch
                {
                    0 => new Response<ChangeRegistrationResponse>().NotFound("No open tournaments to join were found"),
                    1 => new Response<ChangeRegistrationResponse>(),
                    > 1 => new Response<ChangeRegistrationResponse>().Conflict($"More than one possible tournament is open, please resubmit with a tournament name. Open tournaments in this server: {string.Join(", ", unregisteredTournaments.Select(x => x.tournament_name))}"),
                    _ => new Response<ChangeRegistrationResponse>().InternalServerError("Unexpected Result")
                };

                if (!string.IsNullOrEmpty(responseObject.ErrorMessage))
                {
                    return responseObject;
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
                    return responseObject.InternalServerError("registration failed");
                }

                var entrantCount = await connection.ExecuteScalarAsync<int>("select count(*) from tournament.registrations where tournament_id = @id", new { tournament.id });

                _ = ulong.TryParse(tournament.tracking_channel_id, out var channelId);
                _ = ulong.TryParse(tournament.tracking_message_id, out var messageId);
                _ = ulong.TryParse(tournament.role_id, out var roleId);

                return responseObject.SetSuccess(
                    new ChangeRegistrationResponse(
                        entrantCount,
                        channelId,
                        messageId,
                        roleId,
                        tournament.tournament_name));
            }
            catch (Exception ex)
            {
                return new Response<ChangeRegistrationResponse>().InternalServerError(ex.Message);
            }
        }

        public async Task<Response<ChangeRegistrationResponse>> DropPlayerAsync(ChangeRegistration request)
        {
            using var connection = _connectionProvider.GetConnection();
            connection.Open();
            try
            {
                var sql =
@"select 
      guild_id
    , guild_name
    , tournament_id
    , tournament_name
    , registration_start
    , registration_end
    , entrant_id
    , user_id
    , discord_name
    , pronouns
    , registered_on
from tournament.tournament_registrations 
where user_id = @UserId
and guild_id = @GuildId
and (tournament_name = @TournamentName or @TournamentName = '')
and registration_end > now()
;";

                var registrations = (await connection.QueryAsync<TournamentRegistration>(sql, new { UserId = request.UserId.ToString(), request.TournamentName, GuildId = request.GuildId.ToString() })).ToList();


                if (registrations.Count != 1)
                {
                    return registrations.Count == 0
                        ? new Response<ChangeRegistrationResponse>().NotFound($"No tournaments can have their registration {request.DesiredStatus}")
                        : new Response<ChangeRegistrationResponse>().Conflict($"There are multiple tournaments that could be {request.DesiredStatus}, please specify a tournament name {string.Join(", ", registrations.Select(x => x.tournament_name).Distinct())}");
                }

                var tournamentId = registrations.First().tournament_id;

                var deleteResponse = await connection.ExecuteAsync(TournamentRegistrationConstants.DropPlayerSql,
                                    new { registrations.First().entrant_id, tournament_id = tournamentId });

                if (deleteResponse != 1)
                {
                    return new Response<ChangeRegistrationResponse>().InternalServerError("Drop failed");
                }

                var tournament = await connection.QueryFirstAsync<Tournament>("select * from tournament.tournaments where id = @id", new { id = tournamentId });
                var tournamentSummary = await connection.QueryFirstAsync<TournamentSummary>(TournamentRegistrationConstants.GetTournamentSummaryByIdSql, new { tournament_id = tournamentId });

                _ = ulong.TryParse(tournament.tracking_channel_id, out var channelId);
                _ = ulong.TryParse(tournament.tracking_message_id, out var messageId);
                _ = ulong.TryParse(tournament.role_id, out var roleId);


                return new Response<ChangeRegistrationResponse>().SetSuccess(new ChangeRegistrationResponse(tournamentSummary.EntrantCount, channelId, messageId, roleId, tournament.tournament_name));
            }
            catch (Exception ex)
            {
                return new Response<ChangeRegistrationResponse>().InternalServerError(ex.Message);
            }

            throw new NotImplementedException();
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
                var summaries = await connection.QueryAsync<TournamentSummary>(TournamentRegistrationConstants.GetTournamentSummariesSql);
                return new Response<List<TournamentSummary>>().SetSuccess(summaries.ToList());
            }
            catch (Exception ex)
            {
                return new Response<List<TournamentSummary>>().InternalServerError(ex.Message);
            }
        }

        public async Task<Response<List<TournamentRegistrant>>> GetTournamentRegistrantsAsync(int id)
        {
            using var connection = _connectionProvider.GetConnection();
            connection.Open();

            try
            {
                var registrants = await connection.QueryAsync<TournamentRegistrant>(TournamentRegistrationConstants.GetTournamentRegistraionsSql, new { tournament_id = id });
                return new Response<List<TournamentRegistrant>>().SetSuccess(registrants.ToList());
            }
            catch (Exception ex)
            {
                return new Response<List<TournamentRegistrant>>().InternalServerError(ex.Message);
            }
        }

        private async Task<int> UpsertEntrantAsync(IDbConnection connection, Entrant entrant)
        {
            var entrantEntity = await GetEntrantByUserIdAsync(connection, entrant.user_id);

            if (entrantEntity == null)
            {
                return await connection.ExecuteScalarAsync<int>(TournamentRegistrationConstants.InsertEntrantSql,
                    new { UserId = entrant.user_id, UserName = entrant.user_name, Pronouns = entrant.pronouns, TwitchName = entrant.twitch_name });
            }

            var updateSql = getUpdateSql(entrant, entrantEntity);

            if (!string.IsNullOrEmpty(updateSql))
            {
                await connection.ExecuteAsync(updateSql, new { entrant.user_name, entrant.pronouns, entrant.twitch_name, entrantEntity.id });
            }

            return entrantEntity.id;

            static string getUpdateSql(Entrant request, Entrant entity)
            {
                if (request.user_name.Equals(entity.user_name, StringComparison.InvariantCulture) &&
                    request.pronouns.Equals(entity.pronouns, StringComparison.InvariantCulture) &&
                    request.twitch_name.Equals(entity.twitch_name, StringComparison.InvariantCulture))
                {
                    return string.Empty;
                }

                return $@"update tournament.entrants set user_name = @user_name, pronouns = @pronouns, twitch_name = @twitch_name where id = @id";
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
