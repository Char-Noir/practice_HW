using HW78.Helper;
using HW78.Helpers;
using HW78.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HW78.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IExpenseService _expenseService;

		public ReportsController(IExpenseService expenseService)
		{
			_expenseService = expenseService;
		}

		public async Task<IActionResult> Index(DateTime? firstMonth, DateTime? secondMonth)
        {
            if(firstMonth == null)
            {
                firstMonth = DateTime.Now.AddDays(- DateTime.Now.Day + 1);
            }
            if(secondMonth == null)
            {
                secondMonth = DateTime.Now;
            }
            var result = await _expenseService.GetExpensesAsync(new Dictionary<string, string>
            {
                {"IsVisible","true" },
                {"first", firstMonth.ToString() },
                {"second", secondMonth.ToString() }
            });
            if(!result.IsSuccessed) {
                ViewBag.ErrorMessage = ErrorHelper.JoinListWithNewLine(result.Messages);
            }
			ViewBag.DataPoints = JsonConvert.SerializeObject(ExpencesHelper.GenerateDataPoints(result.Data));

			return View((firstMonth.Value, secondMonth.Value));
        }
    }
}
