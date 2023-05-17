namespace HW78.Dto.Request
{
    public class CategoryDtoExpandedRequest:CategoryDtoRequest
    {
        public bool IsActive { get; set; }
        public bool IsVisible { get; set; }
    }
}
