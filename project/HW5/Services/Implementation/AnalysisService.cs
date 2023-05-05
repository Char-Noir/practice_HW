using HW5.DAO;
using HW5.DTO;
using HW5.DTO.Responses;

namespace HW5.Services.Implementation
{
    public class AnalysisService : IAnalysisService
    {
        private readonly IAnalysisDao _analysisDao;

        public AnalysisService(IAnalysisDao analysisDao)
        {
            _analysisDao = analysisDao;
        }

        public async Task<DtoResult<IEnumerable<AnalysisShortResponseDto>>> GetAnalysisAsync()
        {
            return await _analysisDao.GetAnalysissAsync();
        }
    }
}
