using System.ComponentModel.DataAnnotations;

namespace HW78.Dto.Request
{
    public class CategoryDtoRequest
    {
        [Required]
        [StringLength(64, MinimumLength = 4)]
        public string NameCategory { get; set; }
    }
}
