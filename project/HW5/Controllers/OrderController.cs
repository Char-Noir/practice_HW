using HW5.DTO.Requests;
using HW5.DTO.Responses;
using HW5.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HW5.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: OrderController
        public async Task<ActionResult> Index()
        {
            var orders1 = await _orderService.GetOrdersAsync(true);
            if (!orders1.IsSuccessed)
            {
                orders1.Data = new List<OrderShortResponseDto>();
                ViewBag.ErrorMessage = orders1.Messages.ToString();
            }
            var orders2 = await _orderService.GetOrdersAsync(false);
            if (!orders2.IsSuccessed)
            {
                orders2.Data = new List<OrderShortResponseDto>();
                ViewBag.ErrorMessage = orders2.Messages.ToString();
            }
            return View((orders1.Data, orders2.Data));
        }

        // GET: OrderController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OrderController/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: OrderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int analysisId)
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
