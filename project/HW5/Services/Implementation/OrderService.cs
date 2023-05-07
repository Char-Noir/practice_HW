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

        public async Task<DtoResult<int>> CreateOrderAsync(OrderRequestDto order)
        {
            if (order.OrderDateTime >= DateTime.Now.AddDays(365))
            {
                return DtoResult<int>.Error("Ordering too far in advance. You cannot create it that far in advance.");
            }
            var result = await _analysisDao.CheckAnalysisAsync(order.AnalysisId);
            if (result.IsSuccessed && result.Data)
            {
                return await _orderDao.CreateOrderAsync(order);
            }
            else
            {
                 return DtoResult<int>.Error("There is no such analysis.");
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

        public async Task<DtoResult<IEnumerable<OrderShortResponseDto>>> GetOrdersAsync(Dictionary<string, string> parametrs)
        {
            return await _orderDao.GetOrdersAsync(parametrs);
        }

        public async Task<DtoResult<bool>> UpdateOrderAsync(int id, OrderRequestDto order)
        {
            var result = await _orderDao.GetOrderAsync(id);
            if (result.IsSuccessed)
            {
                if(result.Data.DateTime > DateTime.Now && (order.OrderDateTime<= DateTime.Now ||order.OrderDateTime>= DateTime.Now.AddYears(1)))
                {
                    return DtoResult<bool>.Error("You need to chose future.");
                }
                 if (result.Data.DateTime <= DateTime.Now && result.Data.DateTime.ToString("yyyy-mm-dd hh-mm") !=( order.OrderDateTime.ToString("yyyy-mm-dd hh-mm")))
                {
                    return DtoResult<bool>.Error("You can not change old orders dates");
                }
                
                return await _orderDao.UpdateOrderAsync(id, order);
            }
            return DtoResult<bool>.Error("There is no such order.");
        }
    }
}
