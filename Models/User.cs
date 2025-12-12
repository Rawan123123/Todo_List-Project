using System.ComponentModel.DataAnnotations;

namespace ToDo_list.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; } = "User";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<Tpdoitems>? TodoItems { get; set; }

    }
}
