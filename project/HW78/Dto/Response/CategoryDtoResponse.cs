namespace HW78.Dto.Response
{
    public class CategoryDtoResponse
    {
        public int IdCategory { get; set; }

        public string NameCategory { get; set; }

        public bool IsVisible { get; set; }

        public bool IsActive { get; set; }

        public int NumberOfExpenses { get; set; }
    }
}
