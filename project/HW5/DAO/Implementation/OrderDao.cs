using HW5.DTO;
using HW5.DTO.Requests;
using HW5.DTO.Responses;
using HW5.Models;
using System.Data;
using System.Data.SqlClient;

namespace HW5.DAO.Implementation
{
    public class OrderDao : IOrderDao
    {
        private readonly string _connectionString;
        public OrderDao(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new ArgumentException("Connection string is empty or null");
            }
        }

        public Task<DtoResult<bool>> CheckOrderAsync(int orderId)
        {
            throw new NotImplementedException();
        }

        public Task<DtoResult<OrderShortResponseDto>> CreateOrderAsync(OrderRequestDto order)
        {
            throw new NotImplementedException();
        }

        public Task<DtoResult<bool>> DeleteOrderAsync(int orderId)
        {
            throw new NotImplementedException();
        }

        public Task<DtoResult<OrderFullResponseDto>> GetOrderAsync(int orderId)
        {
            throw new NotImplementedException();
        }

        public async Task<DtoResult<IEnumerable<OrderShortResponseDto>>> GetOrdersAsync(bool sqlDataReaderMode)
        {
            if (sqlDataReaderMode)
            {
                return await GetOrdersReaderModeAsync();
            }
            else
            {
                return await GetOrdersAdapterModeAsync();
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

        public Task<DtoResult<bool>> UpdateOrderAsync(int id, OrderRequestDto order)
        {
            throw new NotImplementedException();
        }
    }
}
