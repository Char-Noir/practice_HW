using HW5.DTO;
using HW5.DTO.Responses;
using System.Data;
using System.Data.SqlClient;

namespace HW5.DAO.Implementation
{
    public class AnalysisDao : IAnalysisDao
    {
        private readonly string _connectionString;
        public AnalysisDao(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new ArgumentException("Connection string is empty or null");
            }
        }
        public async Task<DtoResult<bool>> CheckAnalysisAsync(int analysisId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string sql = "SELECT COUNT(*) FROM Analysis WHERE an_id = @analysisId";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@analysisId", analysisId);
                        int count = (int)await command.ExecuteScalarAsync();
                        bool exists = (count > 0);
                        return DtoResult<bool>.Success(exists);
                    }
                }
            }
            catch
            {
                return DtoResult<bool>.Error("Something went wrong. Contact the administator.");
            }
        }

        public async Task<DtoResult<IEnumerable<AnalysisShortResponseDto>>> GetAnalysissAsync()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string sql = "SELECT an_id, an_name " +
                                 "FROM Analysis " +
                                 "JOIN Groups ON Analysis.an_group = Groups.gr_id";

                    DataSet dataSet = new DataSet();
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(sql, connection);

                    dataAdapter.Fill(dataSet);

                    List<AnalysisShortResponseDto> orders = new List<AnalysisShortResponseDto>();

                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        AnalysisShortResponseDto order = new AnalysisShortResponseDto();
                        order.Id = (int)row["an_id"];
                        order.Name = (string)row["an_name"];
                        orders.Add(order);
                    }

                    return DtoResult<IEnumerable<AnalysisShortResponseDto>>.Success(orders);
                }
            }
            catch
            {
                return DtoResult<IEnumerable<AnalysisShortResponseDto>>.Error("Something went wrong. Contact the administator.");
            }
        }
    }
}
