using FeInfo.Common.DTOs;
using FreeEnterprise.Api.Classes;

namespace FreeEnterprise.Api.Interfaces
{
    public interface IGuidesRepository
    {
        Task<Response<IEnumerable<Guide>>> GetGuidesAsync(string searchText, int? limit = null);
    }
}
