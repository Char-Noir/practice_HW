using HW78.DTO;
using HW78.Dto.Request;
using HW78.Dto.Response;
using HW78.Models;
using Microsoft.EntityFrameworkCore;

namespace HW78.DAO.Implementation
{
    public class CategoryDao : ICategoryDao
    {
        private readonly ExpensesDbContext _expensesDbContext;

        public CategoryDao(ExpensesDbContext expensesDbContext)
        {
            _expensesDbContext = expensesDbContext;
        }

        public async Task<DtoResult<bool>> CheckCategoryAsync(int categoryId)
        {
            try
            {
                var category = await _expensesDbContext.Categories.AnyAsync(c => c.IdCategory == categoryId);
                if (!category)
                {
                    return DtoResult<bool>.Error($"Category with id {categoryId} not found");
                }
                return DtoResult<bool>.Success(true);
            }
            catch
            {
                return DtoResult<bool>.Error($"Error checking category with id {categoryId}");
            }
        }

        public async Task<DtoResult<int>> CreateCategoryAsync(CategoryDtoRequest category)
        {
            try
            {
                if(category.NameCategory.Length > 64)
                {
                    return DtoResult<int>.Error($"Name lenght must be lower than 64.");
                }
                Category newCategory = new Category
                {
                    NameCategory = category.NameCategory
                };

                _expensesDbContext.Categories.Add(newCategory);


                await _expensesDbContext.SaveChangesAsync();

                return DtoResult<int>.Success(newCategory.IdCategory);
            }
            catch
            {
                return DtoResult<int>.Error($"An error occurred while creating category");
            }
        }

        public async Task<DtoResult<bool>> DeleteCategoryAsync(int categoryId)
        {
            try
            {
                var category = await _expensesDbContext.Categories.FindAsync(categoryId);

                if (category == null)
                {
                    return DtoResult<bool>.Error($"Category with ID {categoryId} not found.");
                }

                _expensesDbContext.Categories.Remove(category);
                await _expensesDbContext.SaveChangesAsync();

                return DtoResult<bool>.Success(true);
            }
            catch
            {
                return DtoResult<bool>.Error($"An error occurred while deleting category");
            }
        }

        public async Task<DtoResult<CategoryDtoResponse>> GetCategoryAsync(int categoryId)
        {
            try
            {
                var category = await _expensesDbContext.Categories
                    .Include(c=>c.Expenses)
                    .Where(c=>c.IdCategory==categoryId)
                    .Select(c=>new CategoryDtoResponse { IdCategory = c.IdCategory, IsActive = c.IsActive, IsVisible = c.IsVisible, NameCategory = c.NameCategory, NumberOfExpenses = c.Expenses.Count()})
                    .FirstOrDefaultAsync();
                if(category == null)
                {
                    return DtoResult<CategoryDtoResponse>.Error($"Category not found");
                }
                return DtoResult<CategoryDtoResponse>.Success(category);
            }
            catch
            {
                return DtoResult<CategoryDtoResponse>.Error($"An error occurred while getting category");
            }
        }

        public async Task<DtoResult<IEnumerable<CategoryDtoResponse>>> GetCategoriesAsync(Dictionary<string, string> parametrs)
        {
            try
            {
                var unfiltered =  _expensesDbContext.Categories
                   .Include(c => c.Expenses);
                IQueryable<Category> filtered;
                if (parametrs.TryGetValue("IsAvtive", out string isActive))
                {
                     filtered = unfiltered.Where(c => c.IsActive == (isActive=="true"));
                }
                else
                {
                    filtered = unfiltered.Where(c=>true);
                }
                   var categories = await filtered.Select(c => new CategoryDtoResponse { IdCategory = c.IdCategory, IsActive = c.IsActive, IsVisible = c.IsVisible, NameCategory = c.NameCategory, NumberOfExpenses = c.Expenses.Count() })
                   .ToListAsync();
                return DtoResult<IEnumerable<CategoryDtoResponse>>.Success(categories);
            }
            catch
            {
                return DtoResult< IEnumerable<CategoryDtoResponse>>.Error($"An error occurred while getting categories");
            }
        }

        public async Task<DtoResult<bool>> UpdateCategoryAsync(int id, CategoryDtoExpandedRequest category)
        {
            try
            {
                var categoryItem = await _expensesDbContext.Categories.FindAsync(id);
                if (categoryItem == null)
                {
                    return DtoResult<bool>.Error($"Category with id {id} not found");
                }
                categoryItem.NameCategory = category.NameCategory;
                categoryItem.IsActive = category.IsActive;
                categoryItem.IsVisible = category.IsVisible;
                await _expensesDbContext.SaveChangesAsync();

                return DtoResult<bool>.Success(true);
            }
            catch
            {
                return DtoResult<bool>.Error($"An error occurred while updating category");
            }
        }
    }
}
