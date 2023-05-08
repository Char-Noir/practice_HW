using HW5.DTO;
using HW5.DTO.Requests;
using HW5.DTO.Responses;

namespace HW5.DAO
{
    public interface IOrderDao
    {
        // Create
        Task<DtoResult<int>> CreateOrderAsync(OrderRequestDto order);

        // Read
        Task<DtoResult<OrderFullResponseDto>> GetOrderAsync(int orderId);
        Task<DtoResult<IEnumerable<OrderShortResponseDto>>> GetOrdersAsync(Dictionary<string,string> parametrs);

        // Update
        Task<DtoResult<bool>> UpdateOrderAsync(int id, OrderRequestDto order);

        // Delete
        Task<DtoResult<bool>> DeleteOrderAsync(int orderId);

        //Check
        Task<DtoResult<bool>> CheckOrderAsync(int orderId);
    }
}
