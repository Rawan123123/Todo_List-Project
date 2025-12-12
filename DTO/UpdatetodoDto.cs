using System.ComponentModel.DataAnnotations;
using ToDo_list.Enums;

namespace ToDo_list.DTO
{
    public class UpdatetodoDto
    {
        [MinLength(3)]
        public string Item { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    }
}
