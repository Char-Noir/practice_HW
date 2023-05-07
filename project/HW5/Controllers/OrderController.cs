using HW5.DTO.Requests;
using HW5.DTO.Responses;
using HW5.Helpers;
using HW5.Models;
using HW5.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HW5.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IAnalysisService _analysisService;
        private readonly string _dbMethode;

        public OrderController(IOrderService orderService, IAnalysisService analysisService, IConfiguration configuration)
        {
            _orderService = orderService;
            _analysisService = analysisService;
            _dbMethode = (configuration["PrefferedDataBaseConnectionMethode"] ?? "EF").ToUpper().Trim();
        }

        // GET: OrderController
        public async Task<ActionResult> Index()
        {
            ViewBag.Methode = _dbMethode;
            switch (_dbMethode)
            {
                case "EF":
                    {
                        
                        var orders = await _orderService.GetOrdersAsync(new Dictionary<string, string>());
                        if (!orders.IsSuccessed)
                        {
                            orders.Data = new List<OrderShortResponseDto>();
                            ViewBag.ErrorMessage = ErrorHelper.JoinListWithNewLine(orders.Messages);
                        }
                        return View((orders.Data, orders.Data));
                    }
                case "ADO":
                    {
                        var orders1 = await _orderService.GetOrdersAsync(new Dictionary<string, string> { { "sqlDataAccesser","reader" } });
                        if (!orders1.IsSuccessed)
                        {
                            orders1.Data = new List<OrderShortResponseDto>();
                            ViewBag.ErrorMessage = ErrorHelper.JoinListWithNewLine(orders1.Messages);
                        }
                        var orders2 = await _orderService.GetOrdersAsync(new Dictionary<string, string> { { "sqlDataAccesser", "adapter" } });
                        if (!orders2.IsSuccessed)
                        {
                            orders2.Data = new List<OrderShortResponseDto>();
                            ViewBag.ErrorMessage = ErrorHelper.JoinListWithNewLine(orders2.Messages);
                        }
                        return View((orders1.Data, orders2.Data));
                    }
                default:
                    throw new Exception("Invalid preferred database connection method");
            }
            
        }

        // GET: OrderController/Details/id
        public async Task<ActionResult> Details(int id)
        {
            var order = await _orderService.GetOrderAsync(id);
            if (!order.IsSuccessed)
            {
                ViewBag.ErrorMessage = ErrorHelper.JoinListWithNewLine(order.Messages);
            }
            return View(order.Data);
        }

        // GET: OrderController/Create
        public async Task<ActionResult> Create()
        {
            var analysis = await _analysisService.GetAnalysisAsync();
            if (!analysis.IsSuccessed)
            {
                analysis.Data = new List<AnalysisShortResponseDto>();
                ViewBag.ErrorMessage = ErrorHelper.JoinListWithNewLine(analysis.Messages);
            }
            ViewBag.Analysis = new SelectList(analysis.Data ?? new List<AnalysisShortResponseDto>(new AnalysisShortResponseDto[]{ new AnalysisShortResponseDto { Id = -1, Name = "" }}), "Id", "Name");
            return View(new OrderRequestDto());
        }

        // POST: OrderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(OrderRequestDto order)
        {
            var analysis = await _analysisService.GetAnalysisAsync();
            if (!analysis.IsSuccessed)
            {
                analysis.Data = new List<AnalysisShortResponseDto>();
                ViewBag.ErrorMessage = ErrorHelper.JoinListWithNewLine(analysis.Messages);
            }
            ViewBag.Analysis = new SelectList(analysis.Data ?? new List<AnalysisShortResponseDto>(new AnalysisShortResponseDto[] { new AnalysisShortResponseDto { Id = -1, Name = "" } }), "Id", "Name");
            if (order.AnalysisId<= 0)
            {
                ViewBag.ErrorMessage = "Invalid analysis.";
                return View(order);
            }
            if(order.OrderDateTime <= DateTime.Now)
            {
                ViewBag.ErrorMessage = "Invalid date. You need to choose future";
                return View(order);
            }
            var result = await _orderService.CreateOrderAsync(order);
            if (!result.IsSuccessed)
            {
                ViewBag.ErrorMessage = ErrorHelper.JoinListWithNewLine(result.Messages);
                return View(order);
            }
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(order);
            }
        }

        // GET: OrderController/Edit/id
        public async Task<ActionResult> Edit(int id)
        {
            var order = await _orderService.GetOrderAsync(id);
            if (!order.IsSuccessed)
            {
                ViewBag.ErrorMessage = ErrorHelper.JoinListWithNewLine(order.Messages);
                return View();
            }
            var analysis = await _analysisService.GetAnalysisAsync();
            if (!analysis.IsSuccessed)
            {
                analysis.Data = new List<AnalysisShortResponseDto>();
                ViewBag.ErrorMessage = ErrorHelper.JoinListWithNewLine(analysis.Messages);
            }
            ViewBag.Analysis = new SelectList(analysis.Data ?? new List<AnalysisShortResponseDto>(new AnalysisShortResponseDto[] { new AnalysisShortResponseDto { Id = -1, Name = "" } }), "Id", "Name", new AnalysisShortResponseDto { Id = order.Data.Analysis.Id, Name = order.Data.Analysis.Name});
            return View(new OrderRequestDto { AnalysisId = order.Data?.Analysis.Id ?? -1, OrderDateTime = order.Data?.DateTime ?? DateTime.MinValue});

        }

        // POST: OrderController/Edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, OrderRequestDto order)
        {
            var analysis = await _analysisService.GetAnalysisAsync();
            if (!analysis.IsSuccessed)
            {
                analysis.Data = new List<AnalysisShortResponseDto>();
                ViewBag.ErrorMessage = ErrorHelper.JoinListWithNewLine(analysis.Messages);
            }
            ViewBag.Analysis = new SelectList(analysis.Data ?? new List<AnalysisShortResponseDto>(new AnalysisShortResponseDto[] { new AnalysisShortResponseDto { Id = -1, Name = "" } }), "Id", "Name");
            if (order.AnalysisId <= 0)
            {
                ViewBag.ErrorMessage = "Invalid analysis.";
                return View(order);
            }
            var result = await _orderService.UpdateOrderAsync(id,order);
            if (!result.IsSuccessed)
            {
                ViewBag.ErrorMessage = ErrorHelper.JoinListWithNewLine(result.Messages);
                return View(order);
            }
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(order);
            }
        }

        // GET: OrderController/Delete/id
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _orderService.GetOrderAsync(id);
            if (!result.IsSuccessed)
            {
                ViewBag.ErrorMessage = ErrorHelper.JoinListWithNewLine(result.Messages);
                return View();
            }
            return View(result.Data);
        }

        // POST: OrderController/Delete/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            var result = await _orderService.DeleteOrderAsync(id);
            if (!result.IsSuccessed)
            {
                ViewBag.ErrorMessage = ErrorHelper.JoinListWithNewLine(result.Messages);
                var order = await _orderService.GetOrderAsync(id);
                return View(order.Data);
            }
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                var order = await _orderService.GetOrderAsync(id);
                return View(order.Data);
            }
        }
    }
}
