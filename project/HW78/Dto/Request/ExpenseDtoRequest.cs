namespace HW78.Dto.Request
{
    public class ExpenseDtoRequest
    {
        public double CostExpense { get; set; }


        public string? Commentary { get; set; } = null!;

        public int FkCategory { get; set; }
        public bool IsVisible { get; set; }
    }
}
