using System.ComponentModel.DataAnnotations;

namespace ToDo_list.DTO
{
    public class RigesterDto
    {
        [Required]
        [MinLength(3)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(8)]
        public string Password { get; set; }
    }
}
