using HW5.DTO.Responses;
using HW5.DTO;

namespace HW5.Services
{
    public interface IAnalysisService
    {
        // Read
        Task<DtoResult<IEnumerable<AnalysisShortResponseDto>>> GetAnalysisAsync();
    }
}
