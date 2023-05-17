using HW78.Models;

namespace HW78.Dto.Response
{
    public class ExpenseDtoResponse
    {
        public int IdExpense { get; set; }

        public double CostExpense { get; set; }

        public string Commentary { get; set; } 


        public bool IsVisible { get; set; }

        public string CategoryName{ get; set; }
        public int CategoryId { get; set; }
        public DateTime DateTime { get; set; }
        public ExpenseDtoResponse() { }
        public ExpenseDtoResponse(Expense e)
        {
            IdExpense = e.IdExpense;
            Commentary = e.Commentary;
            CostExpense = e.CostExpense;
            IsVisible = e.IsVisible.Value;
            CategoryId = e.FkCategoryNavigation.IdCategory;
            CategoryName = e.FkCategoryNavigation.NameCategory;
            DateTime = e.DateTime;

		}
	}
}
