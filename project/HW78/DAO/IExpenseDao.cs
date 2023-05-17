using HW78.Dto.Request;
using HW78.Dto.Response;
using HW78.DTO;

namespace HW78.DAO
{
    public interface IExpenseDao
    {
        Task<DtoResult<int>> CreateExpenseAsync(ExpenseDtoRequest expense);

        Task<DtoResult<ExpenseDtoResponse>> GetExpenseAsync(int expenseId);
        Task<DtoResult<IEnumerable<ExpenseDtoResponse>>> GetExpensesAsync(Dictionary<string, string> parametrs);

        Task<DtoResult<bool>> UpdateExpenseAsync(int id, ExpenseDtoRequest expense);

        Task<DtoResult<bool>> DeleteExpenseAsync(int expenseId);

        Task<DtoResult<bool>> CheckExpenseAsync(int expenseId);
    }
}
