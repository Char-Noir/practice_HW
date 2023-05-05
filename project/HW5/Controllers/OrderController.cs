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

        public OrderController(IOrderService orderService, IAnalysisService analysisService)
        {
            _orderService = orderService;
            _analysisService = analysisService;
        }

        // GET: OrderController
        public async Task<ActionResult> Index()
        {
            var orders1 = await _orderService.GetOrdersAsync(true);
            if (!orders1.IsSuccessed)
            {
                orders1.Data = new List<OrderShortResponseDto>();
                ViewBag.ErrorMessage = ErrorHelper.JoinListWithNewLine(orders1.Messages);
            }
            var orders2 = await _orderService.GetOrdersAsync(false);
            if (!orders2.IsSuccessed)
            {
                orders2.Data = new List<OrderShortResponseDto>();
                ViewBag.ErrorMessage = ErrorHelper.JoinListWithNewLine(orders2.Messages);
            }
            return View((orders1.Data, orders2.Data));
        }

        // GET: OrderController/Details/5
        public ActionResult Details(int id)
        {
            return View();
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

        // GET: OrderController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OrderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: OrderController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OrderController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
