using HW78.DAO;
using HW78.Dto.Request;
using HW78.Dto.Response;
using HW78.DTO;
using HW78.Models;
using Microsoft.IdentityModel.Tokens;

namespace HW78.Services.Implementation
{
    public class ExpenseService : IExpenseService
    {
        public readonly ICategoryDao _categoryDao;

        public ExpenseService(ICategoryDao categoryDao, IExpenseDao expenseDao)
        {
            _categoryDao = categoryDao;
            _expenseDao = expenseDao;
        }

        public readonly IExpenseDao _expenseDao;
        public async Task<DtoResult<int>> CreateExpenseAsync(ExpenseDtoRequest expense)
        {
            if(!expense.Commentary.IsNullOrEmpty() && expense.Commentary.Length > 255)
            {
                return DtoResult<int>.Error("Comment lenght must be below 255 characters.");
            }
            if (expense.CostExpense < 0)
            {
                return DtoResult<int>.Error("Cost must be bigger or equal to 0.");
            }
            var cat = await _categoryDao.CheckCategoryAsync(expense.FkCategory);
            if(!cat.IsSuccessed || !cat.Data)
            {
                return DtoResult<int>.Error("Category not found!");
            }
            return await _expenseDao.CreateExpenseAsync(expense);
        }

        public async Task<DtoResult<bool>> DeleteExpenseAsync(int expenseId)
        {
            var category = await _expenseDao.CheckExpenseAsync(expenseId);
            if (!category.IsSuccessed || !category.Data)
            {
                return DtoResult<bool>.Error(category.Messages[0]);
            }
           
            return await _expenseDao.DeleteExpenseAsync(expenseId);
        }

        public async Task<DtoResult<ExpenseDtoResponse>> GetExpenseAsync(int expenseId)
        {
            var result = await _expenseDao.CheckExpenseAsync(expenseId);
            if (result.IsSuccessed && result.Data)
            {
                return await _expenseDao.GetExpenseAsync(expenseId);
            }
            return DtoResult<ExpenseDtoResponse>.Error("Expense not found.");
        }

        public async Task<DtoResult<IEnumerable<ExpenseDtoResponse>>> GetExpensesAsync(Dictionary<string, string> parametrs)
        {
            return await _expenseDao.GetExpensesAsync(parametrs);
        }

        public async Task<DtoResult<bool>> HideExpenseAsync(int id)
        {
            var expense = await _expenseDao.GetExpenseAsync(id);
            if (!expense.IsSuccessed)
            {
                return DtoResult<bool>.Error(expense.Messages[0]);
            }
            if (!expense.Data.IsVisible)
            {
                return DtoResult<bool>.Error("Expense hided already");
            }
            return await _expenseDao.UpdateExpenseAsync(id, new ExpenseDtoRequest { Commentary = expense.Data.Commentary, CostExpense = expense.Data.CostExpense, FkCategory = expense.Data.CategoryId, IsVisible = false});
        }

        public async Task<DtoResult<bool>> ShowExpenseAsync(int id)
        {
            var expense = await _expenseDao.GetExpenseAsync(id);
            if (!expense.IsSuccessed)
            {
                return DtoResult<bool>.Error(expense.Messages[0]);
            }
            if (expense.Data.IsVisible)
            {
                return DtoResult<bool>.Error("Expense showed already");
            }
            return await _expenseDao.UpdateExpenseAsync(id, new ExpenseDtoRequest { Commentary = expense.Data.Commentary, CostExpense = expense.Data.CostExpense, FkCategory = expense.Data.CategoryId, IsVisible = true });
        }

        public async Task<DtoResult<bool>> UpdateExpenseAsync(int id, ExpenseDtoRequest expense)
        {
            var cat = await _categoryDao.CheckCategoryAsync(expense.FkCategory);
            if(!cat.IsSuccessed || !cat.Data)
            {
                return DtoResult<bool>.Error("Category not found!");
            }
            var expense1 = await _expenseDao.CheckExpenseAsync(id);
            if (!expense1.IsSuccessed || !expense1.Data)
            {
                return DtoResult<bool>.Error(expense1.Messages[0]);
            }
            return await _expenseDao.UpdateExpenseAsync(id, expense);
        }
    }
}
