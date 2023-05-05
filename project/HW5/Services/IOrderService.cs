using HW5.DTO;
using HW5.DTO.Requests;
using HW5.DTO.Responses;
using HW5.Models;

namespace HW5.Services
{
    public interface IOrderService
    {
            // Create
            Task<DtoResult<OrderShortResponseDto>> CreateOrderAsync(OrderRequestDto order);

            // Read
            Task<DtoResult<OrderFullResponseDto>> GetOrderAsync(int orderId);
            Task<DtoResult<IEnumerable<OrderShortResponseDto>>> GetOrdersAsync(bool sqlDataReaderMode);

            // Update
            Task<DtoResult<bool>> UpdateOrderAsync(int id, OrderRequestDto order);

            // Delete
            Task<DtoResult<bool>> DeleteOrderAsync(int orderId);
    }
}
