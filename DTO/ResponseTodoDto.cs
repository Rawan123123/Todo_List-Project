using System.ComponentModel.DataAnnotations;
using ToDo_list.Enums;

namespace ToDo_list.DTO
{
    public class ResponseTodoDto
    {
        public int Id { get; set; }
        public string Item { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get;set; }
        public DateTime? CreatedAt { get; set; }
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    }
}
