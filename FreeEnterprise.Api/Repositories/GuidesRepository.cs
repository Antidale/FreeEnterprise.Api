using Dapper;
using FeInfo.Common.DTOs;
using FreeEnterprise.Api.Classes;
using FreeEnterprise.Api.Interfaces;

namespace FreeEnterprise.Api.Repositories
{
    public class GuidesRepository(IConnectionProvider connectionProvider) : IGuidesRepository
    {
        private readonly IConnectionProvider _connectionProvider = connectionProvider;

        public async Task<Response<IEnumerable<Guide>>> GetGuidesAsync(string searchText, int? limit = null)
        {
            using var connection = _connectionProvider.GetConnection();
            
            try
            {
                connection.Open();

                var sql = @"select id as id, title, summary as Description, tags, link as Url, link_type as LinkType,
    ts_rank(search, websearch_to_tsquery('english', @searchText)) +
    ts_rank(search, websearch_to_tsquery('simple', @searchText)) as Rank
from info.guides
where search @@ websearch_to_tsquery('english', @searchText)
    or search @@ websearch_to_tsquery('simple', @searchText)
order by rank desc";

                sql += limit is null
                    ? ";"
                    : " limit @limit";

                var guides = await connection.QueryAsync<Guide>(sql
, new { searchText, limit }
);
                return new Response<IEnumerable<Guide>>().SetSuccess(guides);
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<Guide>>().InternalServerError(ex.Message);
            }
             
        }
    }
}
