namespace HW5.DTO.Responses
{
    public class OrderFullResponseDto
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public AnalysisFullResponseDto Analysis { get; set; }
    }
}
