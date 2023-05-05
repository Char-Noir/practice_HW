namespace HW5.DTO.Responses
{
    public class AnalysisFullResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public decimal Price { get; set; }
        public int GroupId { get; set; }
        public GroupResponseDto Group { get; set; }
    }
}
