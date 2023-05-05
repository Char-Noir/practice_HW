using System.ComponentModel;

namespace HW5.DTO.Responses
{
    public class OrderShortResponseDto
    {
        [DisplayName("Date and Time")]
        public DateTime DateTime { get; set; }
        [DisplayName("Analysis")]
        public string AnalysisName { get; set; }
        [DisplayName("Analysis Group")]
        public string GroupName { get; set; }
        [DisplayName("Number")]
        public int Id { get; set; }
    }
}
