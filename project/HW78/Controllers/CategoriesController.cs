using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HW78.Models;
using HW78.Dto.Response;
using HW78.Dto.Request;
using HW78.Services;
using HW78.Helpers;

namespace HW78.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController( ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            if (TempData.TryGetValue("ErrorMessage", out object? value))
            {
                ViewBag.ErrorMessage = value as string;
                TempData.Remove("ErrorMessage");
            }


            var categories = await _categoryService.GetCategoriesAsync(new Dictionary<string, string>());
            if (!categories.IsSuccessed)
            {
                categories.Data = new List<CategoryDtoResponse>();
                ViewBag.ErrorMessage = ErrorHelper.JoinListWithNewLine(categories.Messages);
            }
            ViewBag.Categories = categories.Data;
            return View(new CategoryDtoRequest());
           
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "Category not found";
                return View(new CategoryDtoResponse { NameCategory="Not found"});
            }

            if (TempData.TryGetValue("ErrorMessage", out object? value))
            {
                ViewBag.ErrorMessage = value as string;
                TempData.Remove("ErrorMessage");
            }

            var result = await _categoryService.GetCategoryAsync(id.Value);
            if (!result.IsSuccessed)
            {
                ViewBag.ErrorMessage = ErrorHelper.JoinListWithNewLine(result.Messages);
                return View(new CategoryDtoResponse { NameCategory = "Not found" });
            }

            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NameCategory")] CategoryDtoRequest category)
        {
            if (ModelState.IsValid)
            {
                var result = await _categoryService.CreateCategoryAsync(category);
                if (!result.IsSuccessed)
                {
                    TempData["ErrorMessage"] = ErrorHelper.JoinListWithNewLine(result.Messages);
                }
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Invalid data.";
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "Category not found";
                return View(new CategoryDtoResponse { NameCategory = "Not found" });
            }

            var result = await _categoryService.GetCategoryAsync(id.Value);
            if (!result.IsSuccessed)
            {
                ViewBag.ErrorMessage = ErrorHelper.JoinListWithNewLine(result.Messages);
                return View(new CategoryDtoExpandedRequest { NameCategory = "Not found",IsActive = false, IsVisible = false });
            }
            return View(new CategoryDtoExpandedRequest { NameCategory = result.Data.NameCategory, IsVisible = result.Data.IsVisible, IsActive = result.Data.IsActive});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NameCategory","IsVisible","IsActive")] CategoryDtoExpandedRequest category)
        {
            if (ModelState.IsValid)
            {
                var result = await _categoryService.UpdateCategoryAsync(id,category);
                if (!result.IsSuccessed)
                {
                    TempData["ErrorMessage"] = ErrorHelper.JoinListWithNewLine(result.Messages);
                }
                return RedirectToAction(nameof(Details),new { id });
            }
            TempData["ErrorMessage"] = "Invalid data.";
            return View(category);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrorMessage = "Category not found";
                return View(new CategoryDtoResponse { NameCategory = "Not found" });
            }

            var result = await _categoryService.GetCategoryAsync(id.Value);
            if (!result.IsSuccessed)
            {
                ViewBag.ErrorMessage = ErrorHelper.JoinListWithNewLine(result.Messages);
                return View(new CategoryDtoResponse { NameCategory = "Not found" });
            }

            return View(result.Data);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (ModelState.IsValid)
            {
                var result = await _categoryService.DeleteCategoryAsync(id);
                if (!result.IsSuccessed)
                {
                    TempData["ErrorMessage"] = ErrorHelper.JoinListWithNewLine(result.Messages);
                }
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Delete));
        }
        [HttpPost, ActionName("Deactivate")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeactivateConfirmed(int id)
        {
            if (ModelState.IsValid)
            {
                var result = await _categoryService.DeactivateCategoryAsync(id);
                if (!result.IsSuccessed)
                {
                    TempData["ErrorMessage"] = ErrorHelper.JoinListWithNewLine(result.Messages);
                }
                return RedirectToAction(nameof(Details), new { id });
            }
            return RedirectToAction(nameof(Delete));
        }

    }
}
