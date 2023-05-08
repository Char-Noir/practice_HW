using HW5.DTO;
using HW5.DTO.Responses;
using HW5.Models;
using Microsoft.EntityFrameworkCore;

namespace HW5.DAO.Implementation
{
    public class AnalysisEfDao : IAnalysisDao
    {
        private readonly OrdersDBContext dbContext;

        public AnalysisEfDao(OrdersDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<DtoResult<bool>> CheckAnalysisAsync(int analysisId)
        {
            try
            {
                var analysis = await dbContext.Analyses.AnyAsync(a => a.AnId == analysisId);
                if (!analysis)
                {
                    return DtoResult<bool>.Error($"Analysis with id {analysisId} not found");
                }
                return DtoResult<bool>.Success(true);
            }
            catch
            {
                return DtoResult<bool>.Error($"Error checking analysis with id {analysisId}");
            }
        }

        public async Task<DtoResult<IEnumerable<AnalysisShortResponseDto>>> GetAnalysissAsync()
        {
            try
            {
                var analyses = await dbContext.Analyses
                    .Select(a => new AnalysisShortResponseDto
                    {
                        Id = a.AnId,
                        Name = a.AnName,
                        
                    })
                    .ToListAsync();
                return DtoResult<IEnumerable<AnalysisShortResponseDto>>.Success(analyses);
            }
            catch
            {
                return DtoResult<IEnumerable<AnalysisShortResponseDto>>.Error($"Error getting analyses");
            }
        }
    }
}
