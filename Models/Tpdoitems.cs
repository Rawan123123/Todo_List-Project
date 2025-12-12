using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using ToDo_list.Enums;

namespace ToDo_list.Models
{
    public class Tpdoitems
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        public string Item { get; set; }
        [MaxLength(500)]
        public string? Description { get; set; }
        public bool IsCompleted { get;private set; } = false;
        public void MarkCompleted()
        {
            IsCompleted = true;
        }
        public DateTime? CreatedAt { get;private set; } = DateTime.UtcNow;
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;

        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
