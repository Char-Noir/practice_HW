using HW78.Dto.Request;
using HW78.Dto.Response;
using HW78.DTO;

namespace HW78.Services
{
    public interface IExpenseService
    {
        Task<DtoResult<int>> CreateExpenseAsync(ExpenseDtoRequest expense);

        Task<DtoResult<ExpenseDtoResponse>> GetExpenseAsync(int expenseId);
        Task<DtoResult<IEnumerable<ExpenseDtoResponse>>> GetExpensesAsync(Dictionary<string, string> parametrs);

        Task<DtoResult<bool>> UpdateExpenseAsync(int id, ExpenseDtoRequest expense);
       
        Task<DtoResult<bool>> ShowExpenseAsync(int id);
        Task<DtoResult<bool>> HideExpenseAsync(int id);

        Task<DtoResult<bool>> DeleteExpenseAsync(int expenseId);
    }
}
