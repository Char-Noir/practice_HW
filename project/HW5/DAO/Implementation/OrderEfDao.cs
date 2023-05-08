using HW5.DTO;
using HW5.DTO.Requests;
using HW5.DTO.Responses;
using HW5.Models;
using Microsoft.EntityFrameworkCore;

namespace HW5.DAO.Implementation
{
    public class OrderEfDao : IOrderDao
    {
        private readonly OrdersDBContext dbContext;

        public OrderEfDao(OrdersDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<DtoResult<bool>> CheckOrderAsync(int orderId)
        {
            try
            {
                var order = await dbContext.Orders.AnyAsync(o => o.OrdId == orderId);
                if (!order)
                {
                    return DtoResult<bool>.Error($"Order with id {orderId} not found");
                }
                return DtoResult<bool>.Success(true);
            }
            catch
            {
                return DtoResult<bool>.Error($"Error checking order with id {orderId}");
            }
        }

        public async Task<DtoResult<int>> CreateOrderAsync(OrderRequestDto order)
        {
            try
            {
                // Знаходимо аналіз з вказаним ідентифікатором
                Analysis analysis = await dbContext.Analyses.FindAsync(order.AnalysisId);
                if (analysis == null)
                {
                    return DtoResult<int>.Error($"Analysis with id {order.AnalysisId} not found");
                }

                // Створюємо новий об'єкт Order з властивостями з OrderRequestDto
                Order newOrder = new Order
                {
                    OrdDatetime = order.OrderDateTime,
                    OrdAnNavigation = analysis
                };

                // Додаємо створений об'єкт Order до контексту
                dbContext.Orders.Add(newOrder);

                // Зберігаємо зміни до бази даних
                await dbContext.SaveChangesAsync();

                return DtoResult<int>.Success(newOrder.OrdId);
            }
            catch
            {
                return DtoResult<int>.Error($"An error occurred while creating order");
            }
        }


        public async Task<DtoResult<bool>> DeleteOrderAsync(int orderId)
        {
            try
            {
                var order = await dbContext.Orders.FindAsync(orderId);

                if (order == null)
                {
                    return DtoResult<bool>.Error($"Order with ID {orderId} not found.");
                }

                dbContext.Orders.Remove(order);
                await dbContext.SaveChangesAsync();

                return DtoResult<bool>.Success(true);
            }
            catch
            {
                return DtoResult<bool>.Error($"An error occurred while deleting order");
            }
        }


        public async Task<DtoResult<OrderFullResponseDto>> GetOrderAsync(int orderId)
        {
            try
            {
                var order = await dbContext.Orders
                    .Include(o => o.OrdAnNavigation)
                        .ThenInclude(a => a.AnGroupNavigation)
                    .FirstOrDefaultAsync(o => o.OrdId == orderId);

                if (order == null)
                {
                    return  DtoResult<OrderFullResponseDto>.Error($"Order with id {orderId} not found");
                }

                var orderDto = new OrderFullResponseDto
                {
                    Id = order.OrdId,
                    DateTime = order.OrdDatetime,
                    Analysis = new AnalysisFullResponseDto
                    {
                        Id = order.OrdAnNavigation.AnId,
                        Name = order.OrdAnNavigation.AnName,
                        Cost = order.OrdAnNavigation.AnCost,
                        Price = order.OrdAnNavigation.AnPrice,
                        GroupId = order.OrdAnNavigation.AnGroupNavigation.GrId,
                        Group = new GroupResponseDto
                        {
                            Id = order.OrdAnNavigation.AnGroupNavigation.GrId,
                            Name = order.OrdAnNavigation.AnGroupNavigation.GrName,
                            Temp = order.OrdAnNavigation.AnGroupNavigation.GrTemp
                        }
                    }
                };

                return  DtoResult<OrderFullResponseDto>.Success(orderDto);
            }
            catch
            {
                // handle exception
                return  DtoResult<OrderFullResponseDto>.Error("An error occurred while getting order");
            }
        }


        public async Task<DtoResult<IEnumerable<OrderShortResponseDto>>> GetOrdersAsync(Dictionary<string, string> nevermind)
        {
            try
            {
                var orders = await dbContext.Orders
                    .Include(o => o.OrdAnNavigation)
                        .ThenInclude(a => a.AnGroupNavigation)
                    .Select(o => new OrderShortResponseDto
                    {
                        DateTime = o.OrdDatetime,
                        AnalysisName = o.OrdAnNavigation.AnName,
                        GroupName = o.OrdAnNavigation.AnGroupNavigation.GrName,
                        Id = o.OrdId
                    })
                    .ToListAsync();

                return DtoResult<IEnumerable<OrderShortResponseDto>>.Success(orders);
            }
            catch
            {
                return DtoResult<IEnumerable<OrderShortResponseDto>>.Error("An error occurred while getting orders");
            }
        }


        public async Task<DtoResult<bool>> UpdateOrderAsync(int id, OrderRequestDto order)
        {
            try
            {
                var orderEntity = await dbContext.Orders.FindAsync(id);
                if (orderEntity == null)
                {
                    return DtoResult<bool>.Error($"Order with id {id} not found");
                }

                // Update order properties
                orderEntity.OrdDatetime = order.OrderDateTime;
                orderEntity.OrdAn = order.AnalysisId;

                // Save changes to the database
                await dbContext.SaveChangesAsync();

                return DtoResult<bool>.Success(true);
            }
            catch {
                return DtoResult<bool>.Error("An error occurred while updating order");
            }
        }

    }
}
