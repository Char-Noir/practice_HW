using HW78.Dto.Response;

namespace HW78.Helper
{
	public static class ExpencesHelper
	{
		public static List<DataPoint> GenerateDataPoints(IEnumerable<ExpenseDtoResponse> expenses)
		{
			return expenses.GroupBy(e => e.CategoryName).Select(g => new DataPoint(g.Key, g.Sum(e => e.CostExpense))).ToList();
		}
	}
}
