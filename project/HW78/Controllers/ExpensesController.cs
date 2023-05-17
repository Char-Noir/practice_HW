using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HW78.Models;
using HW78.Services;
using HW78.Dto.Response;
using HW78.Helpers;
using HW78.Services.Implementation;
using HW78.Dto.Request;

namespace HW78.Controllers
{
    public class ExpensesController : Controller
    {
        private readonly IExpenseService _expenseService;
        private readonly ICategoryService _categoryService;

        public ExpensesController( IExpenseService expenseService, ICategoryService categoryService)
        {
            _expenseService = expenseService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            if (TempData.TryGetValue("ErrorMessage", out object? value))
            {
                ViewBag.ErrorMessage = value as string;
                TempData.Remove("ErrorMessage");
            }
            var expenses = await _expenseService.GetExpensesAsync(new Dictionary<string, string>());
            if (!expenses.IsSuccessed)
            {
                expenses.Data = new List<ExpenseDtoResponse>();
                ViewBag.ErrorMessage = ErrorHelper.JoinListWithNewLine(expenses.Messages);
            }
            return View(expenses.Data);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "Expense not found";
                return View(new ExpenseDtoResponse { Commentary = "Not found" });
            }

            if (TempData.TryGetValue("ErrorMessage", out object? value))
            {
                ViewBag.ErrorMessage = value as string;
                TempData.Remove("ErrorMessage");
            }

            var result = await _expenseService.GetExpenseAsync(id.Value);
            if (!result.IsSuccessed)
            {
                ViewBag.ErrorMessage = ErrorHelper.JoinListWithNewLine(result.Messages);
                return View(new ExpenseDtoResponse { Commentary = "Not found" });
            }

            return View(result.Data);
        }

        public async Task<IActionResult> Create()
        {
            var cats = await _categoryService.GetCategoriesAsync(new Dictionary<string, string>()
            {
                { "IsAvtive","true" }
            });
            if (!cats.IsSuccessed)
            {
                cats.Data = new List<CategoryDtoResponse>();
                ViewBag.ErrorMessage = ErrorHelper.JoinListWithNewLine(cats.Messages);
            }
            ViewBag.FkCategory = new SelectList(cats.Data, "IdCategory", "NameCategory");
            return View(new ExpenseDtoRequest());
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CostExpense,Commentary,FkCategory")] ExpenseDtoRequest expense)
        {
            if (ModelState.IsValid)
            {
                var result = await _expenseService.CreateExpenseAsync(expense);
                if (result.IsSuccessed)
                {
                    return RedirectToAction(nameof(Index));
                }
                ViewBag.ErrorMessage = ErrorHelper.JoinListWithNewLine(result.Messages);
            }
            var cats = await _categoryService.GetCategoriesAsync(new Dictionary<string, string>()
            {
                { "IsAvtive","true" }
            });
            if (!cats.IsSuccessed)
            {
                cats.Data = new List<CategoryDtoResponse>();
                ViewBag.ErrorMessage = ErrorHelper.JoinListWithNewLine(cats.Messages);
            }
            ViewBag.FkCategory = new SelectList(cats.Data, "IdCategory", "NameCategory");
            return View(expense);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "Expense not found";
                return View(new ExpenseDtoResponse { Commentary = "Not found" });
            }

            if (TempData.TryGetValue("ErrorMessage", out object? value))
            {
                ViewBag.ErrorMessage = value as string;
                TempData.Remove("ErrorMessage");
            }

            var result = await _expenseService.GetExpenseAsync(id.Value);
            if (!result.IsSuccessed)
            {
                ViewBag.ErrorMessage = ErrorHelper.JoinListWithNewLine(result.Messages);
                return View(new ExpenseDtoResponse { Commentary = "Not found" });
            }
            var cats = await _categoryService.GetCategoriesAsync(new Dictionary<string, string>()
            {
                { "IsAvtive","true" }
            });
            if (!cats.IsSuccessed)
            {
                cats.Data = new List<CategoryDtoResponse>();
                ViewBag.ErrorMessage = ErrorHelper.JoinListWithNewLine(cats.Messages);
            }
            if (cats.Data.Where(c => c.NameCategory == result.Data.CategoryName).Count() <= 0)
            {
                cats.Data.Append(new CategoryDtoResponse { IdCategory = result.Data.CategoryId, NameCategory = result.Data.CategoryName });
            }
            ViewBag.FkCategory = new SelectList(cats.Data, "IdCategory", "NameCategory",result.Data.CategoryId);
            ViewBag.ChoosenEdit = result.Data.CategoryId;
            return View(new ExpenseDtoRequest() { Commentary = result.Data.Commentary, CostExpense=result.Data.CostExpense, IsVisible = result.Data.IsVisible, FkCategory = result.Data.CategoryId});
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CostExpense,Commentary,FkCategory,IsVisible")] ExpenseDtoRequest expense)
        {
            if (ModelState.IsValid)
            {
                var result = await _expenseService.UpdateExpenseAsync(id,expense);
                if (result.IsSuccessed)
                {
                    return RedirectToAction(nameof(Index));
                }
                ViewBag.ErrorMessage = ErrorHelper.JoinListWithNewLine(result.Messages);
            }
            var cats = await _categoryService.GetCategoriesAsync(new Dictionary<string, string>()
            {
                { "IsAvtive","true" }
            });
            if (!cats.IsSuccessed)
            {
                cats.Data = new List<CategoryDtoResponse>();
                ViewBag.ErrorMessage = ErrorHelper.JoinListWithNewLine(cats.Messages);
            }
            ViewBag.FkCategory = new SelectList(cats.Data, "IdCategory", "NameCategory", ViewBag.ChoosenEdit??0);
            return View(expense);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "Expense not found";
                return View(new ExpenseDtoResponse { Commentary = "Not found" });
            }

            if (TempData.TryGetValue("ErrorMessage", out object? value))
            {
                ViewBag.ErrorMessage = value as string;
                TempData.Remove("ErrorMessage");
            }

            var result = await _expenseService.GetExpenseAsync(id.Value);
            if (!result.IsSuccessed)
            {
                ViewBag.ErrorMessage = ErrorHelper.JoinListWithNewLine(result.Messages);
                return View(new ExpenseDtoResponse { Commentary = "Not found" });
            }

            return View(result.Data);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (ModelState.IsValid)
            {
                var result = await _expenseService.DeleteExpenseAsync(id);
                if (!result.IsSuccessed)
                {
                    TempData["ErrorMessage"] = ErrorHelper.JoinListWithNewLine(result.Messages);
                }
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Delete));
        }

        
    }
}
