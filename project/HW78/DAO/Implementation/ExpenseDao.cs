using HW78.Dto.Request;
using HW78.Dto.Response;
using HW78.DTO;
using HW78.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace HW78.DAO.Implementation
{
    public class ExpenseDao : IExpenseDao
    {

        private readonly ExpensesDbContext _expensesDbContext;

        public ExpenseDao(ExpensesDbContext expensesDbContext)
        {
            _expensesDbContext = expensesDbContext;
        }

        public async Task<DtoResult<bool>> CheckExpenseAsync(int expenseId)
        {
            try
            {
                var expense = await _expensesDbContext.Expenses.AnyAsync(e => e.IdExpense == expenseId);
                if (!expense)
                {
                    return DtoResult<bool>.Error($"Expense with id {expenseId} not found");
                }
                return DtoResult<bool>.Success(true);
            }
            catch
            {
                return DtoResult<bool>.Error($"Error checking expense with id {expenseId}");
            }
        }

        public async Task<DtoResult<int>> CreateExpenseAsync(ExpenseDtoRequest expense)
        {
            try
            {
                if (!expense.Commentary.IsNullOrEmpty() && expense.Commentary.Length >255)
                {
                    return DtoResult<int>.Error($"Name lenght must be lower than 255.");
                }
                Expense expense1 = new Expense
                {
                    FkCategory = expense.FkCategory,
                    CostExpense = expense.CostExpense,
                    Commentary = expense.Commentary,
                    DateTime = DateTime.Now
                };
                

                _expensesDbContext.Expenses.Add(expense1);


                await _expensesDbContext.SaveChangesAsync();

                return DtoResult<int>.Success(expense1.IdExpense);
            }
            catch(Exception ex)
            {
                return DtoResult<int>.Error($"An error occurred while creating expense");
            }
        }

        public async Task<DtoResult<bool>> DeleteExpenseAsync(int expenseId)
        {
            try
            {
                var expense = await _expensesDbContext.Expenses.FindAsync(expenseId);

                if (expense == null)
                {
                    return DtoResult<bool>.Error($"Category with ID {expenseId} not found.");
                }

                _expensesDbContext.Expenses.Remove(expense);
                await _expensesDbContext.SaveChangesAsync();

                return DtoResult<bool>.Success(true);
            }
            catch
            {
                return DtoResult<bool>.Error($"An error occurred while deleting expense");
            }
        }

        public async Task<DtoResult<ExpenseDtoResponse>> GetExpenseAsync(int expenseId)
        {
            try
            {
                var expense = await _expensesDbContext.Expenses
                    .Include(e=>e.FkCategoryNavigation)
                    .Where(e => e.IdExpense== expenseId)
                    .Select(e => new ExpenseDtoResponse(e))
                    .FirstOrDefaultAsync();
                if (expense == null)
                {
                    return DtoResult<ExpenseDtoResponse>.Error($"Expense not found");
                }
                return DtoResult<ExpenseDtoResponse>.Success(expense);
            }
            catch
            {
                return DtoResult<ExpenseDtoResponse>.Error($"An error occurred while getting expense");
            }
        }

        public async Task<DtoResult<IEnumerable<ExpenseDtoResponse>>> GetExpensesAsync(Dictionary<string, string> parametrs)
        {
            try
            {
                var unfiltered =  _expensesDbContext.Expenses
                    .Include(e => e.FkCategoryNavigation);

                var filtered = await Filter(unfiltered, parametrs);
				var expense = await filtered.Select(e => new ExpenseDtoResponse(e))
					.ToListAsync();
				if (expense == null)
                {
                    return DtoResult< IEnumerable < ExpenseDtoResponse >>.Error($"Expense not found");
                }
                return DtoResult< IEnumerable < ExpenseDtoResponse >>.Success(expense);
            }
            catch
            {
                return DtoResult< IEnumerable < ExpenseDtoResponse >>.Error($"An error occurred while getting expenses");
            }
        }

		private async Task<IQueryable<Expense>> Filter(IQueryable<Expense> unfiltered, Dictionary<string, string> parametrs)
		{
			if(parametrs.Keys.Count == 0)
            {
                return unfiltered;
            }
            if (parametrs.ContainsKey("IsVisible"))
            {
                unfiltered = unfiltered.Where(e => e.IsVisible.Value);
                unfiltered = unfiltered.Where(e => e.FkCategoryNavigation.IsVisible);
            }
            if (parametrs.ContainsKey("first"))
            {
                unfiltered = unfiltered.Where(e => e.DateTime >=  DateTime.Parse(parametrs["first"]));
            }
			if (parametrs.ContainsKey("second"))
			{
				unfiltered = unfiltered.Where(e => e.DateTime <= DateTime.Parse(parametrs["second"]));
			}
            return unfiltered;
		}

		public async Task<DtoResult<bool>> UpdateExpenseAsync(int id, ExpenseDtoRequest expense)
        {
            try
            {
                var expenseItem = await _expensesDbContext.Expenses.FindAsync(id);
                if (expenseItem == null)
                {
                    return DtoResult<bool>.Error($"Expense with id {id} not found");
                }
                expenseItem.CostExpense = expense.CostExpense;
                expenseItem.Commentary = expense.Commentary;
                expenseItem.FkCategory = expense.FkCategory;
                expenseItem.IsVisible = expense.IsVisible;

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
