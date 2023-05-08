using HW5.DTO;
using HW5.DTO.Requests;
using HW5.DTO.Responses;
using HW5.Models;
using System.Data;
using System.Data.SqlClient;

namespace HW5.DAO.Implementation
{
    public class OrderAdoDao : IOrderDao
    {
        private readonly string _connectionString;
        public OrderAdoDao(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new ArgumentException("Connection string is empty or null");
            }
        }

        public async Task<DtoResult<bool>> CheckOrderAsync(int orderId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string sql = "SELECT COUNT(*) FROM Orders WHERE ord_id = @orderId;";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@orderId", orderId);
                        int count = Convert.ToInt32(await command.ExecuteScalarAsync());
                        bool result = count > 0;
                        return DtoResult<bool>.Success(result);
                    }
                }
            }
            catch
            {
                return DtoResult<bool>.Error($"Order not found: {orderId}.");
            }
        }


        public async Task<DtoResult<int>> CreateOrderAsync(OrderRequestDto order)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string sql = "INSERT INTO Orders (ord_datetime, ord_an) VALUES (@datetime, @analysisId); SELECT SCOPE_IDENTITY();";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@datetime", order.OrderDateTime);
                        command.Parameters.AddWithValue("@analysisId", order.AnalysisId);
                        int orderId = Convert.ToInt32(await command.ExecuteScalarAsync());
                        return DtoResult<int>.Success(orderId);
                    }
                }
            }
            catch
            {
                return DtoResult<int>.Error($"Failed to create order.");
            }
        }


        public async Task<DtoResult<bool>> DeleteOrderAsync(int orderId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    string sql = "DELETE FROM Orders WHERE ord_id = @orderId";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@orderId", orderId);
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        if (rowsAffected > 0)
                        {
                            return DtoResult<bool>.Success(true);
                        }
                        else
                        {
                            return DtoResult<bool>.Error($"Failed to delete order with id {orderId}. Order not found.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return DtoResult<bool>.Error($"Failed to delete order with id {orderId}. Error message: {ex.Message}");
            }
        }


        public async Task<DtoResult<OrderFullResponseDto>> GetOrderAsync(int orderId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string sql = @"
                SELECT
                    o.ord_id AS OrderId,
                    o.ord_datetime AS OrderDateTime,
                    a.an_id AS AnalysisId,
                    a.an_name AS AnalysisName,
                    a.an_cost AS AnalysisCost,
                    a.an_price AS AnalysisPrice,
                    g.gr_id AS GroupId,
                    g.gr_name AS GroupName,
                    g.gr_temp AS GroupTemp
                FROM Orders o
                INNER JOIN Analysis a ON o.ord_an = a.an_id
                INNER JOIN Groups g ON a.an_group = g.gr_id
                WHERE o.ord_id = @orderId;
            ";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@orderId", orderId);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                OrderFullResponseDto order = new OrderFullResponseDto
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("OrderId")),
                                    DateTime = reader.GetDateTime(reader.GetOrdinal("OrderDateTime")),
                                    Analysis = new AnalysisFullResponseDto
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("AnalysisId")),
                                        Name = reader.GetString(reader.GetOrdinal("AnalysisName")),
                                        Cost = reader.GetDecimal(reader.GetOrdinal("AnalysisCost")),
                                        Price = reader.GetDecimal(reader.GetOrdinal("AnalysisPrice")),
                                        GroupId = reader.GetInt32(reader.GetOrdinal("GroupId")),
                                        Group = new GroupResponseDto
                                        {
                                            Id = reader.GetInt32(reader.GetOrdinal("GroupId")),
                                            Name = reader.GetString(reader.GetOrdinal("GroupName")),
                                            Temp = reader.GetString(reader.GetOrdinal("GroupTemp"))
                                        }
                                    }
                                };

                                return DtoResult<OrderFullResponseDto>.Success(order);
                            }
                            else
                            {
                                return DtoResult<OrderFullResponseDto>.Error($"Order with id {orderId} not found.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return DtoResult<OrderFullResponseDto>.Error($"Failed to get order. {ex.Message}");
            }
        }


        public async Task<DtoResult<IEnumerable<OrderShortResponseDto>>> GetOrdersAsync(Dictionary<string, string> parametrs)
        {
            if (parametrs != null && parametrs.ContainsKey("sqlDataAccesser") && parametrs["sqlDataAccesser"].ToLower() == "reader")
            {
                return await GetOrdersReaderModeAsync();
            }
            else if(parametrs != null && parametrs.ContainsKey("sqlDataAccesser") && parametrs["sqlDataAccesser"].ToLower() == "adapter")
            {
                return await GetOrdersAdapterModeAsync();
            }
            else
            {
                throw new ArgumentException("Parametrs needs to have 'sqlDataAccesser' key with options 'reader' or 'adapter'");
            }
        }

        private async Task<DtoResult<IEnumerable<OrderShortResponseDto>>> GetOrdersReaderModeAsync()
        {
            IEnumerable<OrderShortResponseDto> orders = new List<OrderShortResponseDto>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string sql = "SELECT ord_id, ord_datetime, an_name, gr_name " +
                             "FROM Orders " +
                             "JOIN Analysis ON Orders.ord_an = Analysis.an_id " +
                             "JOIN Groups ON Analysis.an_group = Groups.gr_id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        OrderShortResponseDto order = new OrderShortResponseDto();
                        order.Id = reader.GetInt32(0);
                        order.DateTime = reader.GetDateTime(1);
                        order.AnalysisName = reader.GetString(2);
                        order.GroupName = reader.GetString(3);
                        orders = orders.Append(order);
                    }
                }
            }
            return DtoResult<IEnumerable<OrderShortResponseDto>>.Success(orders);
        }

        private async Task<DtoResult<IEnumerable<OrderShortResponseDto>>> GetOrdersAdapterModeAsync()
        {
            IEnumerable<OrderShortResponseDto> orders = new List<OrderShortResponseDto>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string sql = "SELECT ord_id, ord_datetime, an_name, gr_name " +
                             "FROM Orders " +
                             "JOIN Analysis ON Orders.ord_an = Analysis.an_id " +
                             "JOIN Groups ON Analysis.an_group = Groups.gr_id";
                using (SqlDataAdapter adapter = new SqlDataAdapter(sql, connection))
                {
                    DataSet dataset = new DataSet();
                    adapter.Fill(dataset);
                    foreach (DataRow row in dataset.Tables[0].Rows)
                    {
                        OrderShortResponseDto order = new OrderShortResponseDto();
                        order.Id = (int)row["ord_id"];
                        order.DateTime = (DateTime)row["ord_datetime"];
                        order.AnalysisName = (string)row["an_name"];
                        order.GroupName = (string)row["gr_name"];
                        orders = orders.Append(order);
                    }
                }
            }
            return DtoResult<IEnumerable<OrderShortResponseDto>>.Success(orders);

        }

        public async Task<DtoResult<bool>> UpdateOrderAsync(int id, OrderRequestDto order)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = "UPDATE Orders SET ord_datetime = @datetime, ord_an = @analysis WHERE ord_id = @id";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@datetime", order.OrderDateTime);
                        command.Parameters.AddWithValue("@analysis", order.AnalysisId);
                        command.Parameters.AddWithValue("@id", id);

                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        return  DtoResult<bool>.Success(rowsAffected > 0);
                    }
                }
            }
            catch
            {
                return DtoResult<bool>.Error("Something went wrong.");
            }
        }

    }
}
