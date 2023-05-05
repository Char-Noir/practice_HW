using HW5.DTO.Responses;
using HW5.DTO;

namespace HW5.DAO
{
    public interface IAnalysisDao
    {
        Task<DtoResult<bool>> CheckAnalysisAsync(int orderId);
        // Read
        Task<DtoResult<IEnumerable<AnalysisShortResponseDto>>> GetAnalysissAsync();
    }
}
