using HW5.DAO;
using HW5.DTO;
using HW5.DTO.Requests;
using HW5.DTO.Responses;
using HW5.Models;
using System.Data.SqlClient;

namespace HW5.Services.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IOrderDao _orderDao;
        private readonly IAnalysisDao _analysisDao;

        public OrderService(IOrderDao orderDao, IAnalysisDao analysisDao)
        {
            _orderDao = orderDao;
            _analysisDao = analysisDao;
        }

        public async Task<DtoResult<OrderShortResponseDto>> CreateOrderAsync(OrderRequestDto order)
        {
            order.DateTime = DateTime.Now;
            if (await _analysisDao.CheckAnalysisAsync(order.AnalysisId))
            {
                return await _orderDao.CreateOrderAsync(order);
            }
            else
            {
                 return DtoResult<OrderShortResponseDto>.Error("There is no such analysis.");
            }
        }

        public async Task<DtoResult<bool>> DeleteOrderAsync(int orderId)
        {
            var result = await _orderDao.CheckOrderAsync(orderId);
            if (result.IsSuccessed && result.Data)
            {
                return await _orderDao.DeleteOrderAsync(orderId);
            }
            return DtoResult<bool>.Error("There is no such order.");
        }

        public async Task<DtoResult<OrderFullResponseDto>> GetOrderAsync(int orderId)
        {
            var result = await _orderDao.CheckOrderAsync(orderId);
            if(result.IsSuccessed && result.Data)
            {
                return await _orderDao.GetOrderAsync(orderId);
            }
            return DtoResult<OrderFullResponseDto>.Error("There is no such order.");
        }

        public async Task<DtoResult<IEnumerable<OrderShortResponseDto>>> GetOrdersAsync(bool sqlDataReaderMode)
        {
            return await _orderDao.GetOrdersAsync(sqlDataReaderMode);
        }

        public async Task<DtoResult<bool>> UpdateOrderAsync(int id, OrderRequestDto order)
        {
            var result = await _orderDao.CheckOrderAsync(id);
            if (result.IsSuccessed && result.Data)
            {
                return await _orderDao.UpdateOrderAsync(id, order);
            }
            return DtoResult<bool>.Error("There is no such order.");
        }
    }
}
