using System.ComponentModel.DataAnnotations;

namespace HW5.DTO.Requests
{
    public class OrderRequestDto
    {
        [DataType(DataType.DateTime)]
        public DateTime OrderDateTime { get; set; }
        public int AnalysisId { get; set; }
    }
}
