using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ToDo_list.DTO;
using ToDo_list.Enums;
using ToDo_list.Exceptions;
using ToDo_list.Models;
namespace ToDo_list.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [DisableRequestSizeLimit]
    [Authorize]
    public class TodoController : ControllerBase
    {
        private Context _context;

        public TodoController(Context context)
        {
            _context = context;
        }
        //create method to get Id of logged in user from JWT token
        private int GetCurrentUserId()
        {
            // جرب كل الطرق الممكنة لقراءة الـ User ID
            var subClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                   ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub)
                   ?? User.FindFirstValue("sub");

            if (string.IsNullOrEmpty(subClaim))
            {
                throw new UnauthorizedException("User ID not found in token claims");
            }

            return int.Parse(subClaim);
        }

        [HttpGet]
        public IActionResult GetAllTodos(
            [FromQuery] int page = 1 ,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sortBy = "CreatedAt",
            [FromQuery] string? order = "desc")
        {
            int userId = GetCurrentUserId();

            var query = _context.Tpdoitems.Where(t => t.UserId == userId);

            //sorting logic
            query = sortBy?.ToLower() switch
            {
                "priority" => order.ToLower() == "asc" 
                    ?query.OrderBy(t => t.Priority)
                    :query.OrderByDescending(t => t.Priority),

                "item" => order.ToLower() == "asc" 
                    ? query.OrderBy(t => t.Item)
                    :query.OrderByDescending(t => t.Item),

                "iscompleted" => order.ToLower() == "asc" 
                    ? query.OrderBy(t => t.IsCompleted)  
                    :query.OrderByDescending(t => t.IsCompleted),

                _ => order.ToLower() == "asc" 
                    ? query.OrderBy(t => t.CreatedAt) 
                    :query.OrderByDescending(t => t.CreatedAt)
            };

            int totalItems = query.Count();

            var todos = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(item => new ResponseTodoDto
                {
                    Id = item.Id,
                    Item = item.Item,
                    Description = item.Description,
                    IsCompleted = item.IsCompleted,
                    CreatedAt = item.CreatedAt,
                    Priority = item.Priority

                }).ToList();
            return Ok(new
            {
                data = todos,
                currentPage = page,
                totalPages = (int)Math.Ceiling((double)totalItems / pageSize),
                totalItems = totalItems
            });
        }

        [HttpGet("{id}")]
        public IActionResult GetTodoById(int id)
        {
            int userId = GetCurrentUserId();

            var todo = _context.Tpdoitems.Where(t => t.Id == id && t.UserId == userId)
                .Select(item => new ResponseTodoDto
                {
                    Id = item.Id,
                    Item = item.Item,
                    Description = item.Description,
                    IsCompleted = item.IsCompleted,
                    CreatedAt = item.CreatedAt,
                    Priority = item.Priority
                }).FirstOrDefault();

            if (todo == null)
            {
                throw new NotFoundException($"To-do item with ID {id} not found.");
            }
            return Ok(todo);

        }

        [HttpGet("stats")]
        public IActionResult GetStatistics()
        {
            int userId = GetCurrentUserId();
            var todo = _context.Tpdoitems.Where(t => t.UserId == userId).ToList();

            var stats = new
            {
                totalTask = todo.Count,
                completedTask = todo.Count(t => t.IsCompleted),
                pendingTask = todo.Count(t => !t.IsCompleted),

                //according to priority
                highPriorityTask = todo.Count(t => t.Priority == TaskPriority.High),
                mediumPriorityTask = todo.Count(t => t.Priority == TaskPriority.Medium),
                lowPriorityTask = todo.Count(t => t.Priority == TaskPriority.Low),

                completionRatio = Math.Round((double)todo.Count(t => t.IsCompleted) / todo.Count * 100, 2)
            };
            return Ok(stats);
        }

        [HttpGet("search")]
        public IActionResult SearchTodos([FromQuery] string? item , [FromQuery] bool? isCompleted ,[FromQuery] TaskPriority? priority)
        {
            int userId = GetCurrentUserId();
            /*
             // كود غلط ❌
            var todos = _context.Tpdoitems
                .Where(t => t.UserId == userId)
                .ToList(); // ← جاب كل المهام من الـ Database (مثلاً 10,000 مهمة!)

            // دلوقتي هنفلتر في الذاكرة
            if (!string.IsNullOrEmpty(item))
            {
                todos = todos.Where(t => t.Item.Contains(item)).ToList();
            }
             */

            IQueryable<Tpdoitems> query = _context.Tpdoitems.Where(t => t.UserId == userId);

            if (!string.IsNullOrEmpty(item))
            {
                query = query.Where(t=>t.Item.Contains(item));
            }
            if(isCompleted.HasValue)
            {
                query = query.Where(t => t.IsCompleted == isCompleted.Value);
            }
            if(priority.HasValue)
            {
                query = query.Where(t => t.Priority == priority.Value);
            }

            var todos = query.Select(todo => new ResponseTodoDto
            {
                Id = todo.Id,
                Item = todo.Item,
                Description = todo.Description,
                IsCompleted = todo.IsCompleted,
                CreatedAt = todo.CreatedAt,
                Priority = todo.Priority
            }).ToList();

            return Ok(todos);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTodoAsync(CreatetodoDto dtoFromRequest)
        {
            int userId = GetCurrentUserId();

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                .ToDictionary(
                   kvp => kvp.Key,
                   kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );

                throw new MyValidationException("Validation failed", errors);
            }

            Tpdoitems todo = new Tpdoitems
            {
                Item = dtoFromRequest.Item,
                Description = dtoFromRequest.Description,
                Priority = dtoFromRequest.Priority,
                UserId = userId,
            };

            await _context.AddAsync(todo);
            await _context.SaveChangesAsync();

            var response = new ResponseTodoDto
            {
                Id = todo.Id,
                Item = todo.Item,
                Description = todo.Description,
                IsCompleted = todo.IsCompleted,
                CreatedAt = todo.CreatedAt,
                Priority = todo.Priority,
            };
            return CreatedAtAction(nameof(GetTodoById), new { id = todo.Id }, response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTodo(int id, UpdatetodoDto dtoFromRequest)
        {
            int userId = GetCurrentUserId();

            Tpdoitems todoFromDb = _context.Tpdoitems.FirstOrDefault(t => t.Id == id && t.UserId == userId);

            if (todoFromDb == null) throw new NotFoundException($"To-do item with ID {id} not found.");
            

            if (dtoFromRequest.Item.Contains("forbidden")) throw new BadRequestException("Item contains forbidden word.");

            todoFromDb.Item = dtoFromRequest.Item;
            todoFromDb.Description = dtoFromRequest.Description;
            todoFromDb.Priority = dtoFromRequest.Priority;

            _context.SaveChanges();

            return Ok(new ResponseTodoDto
            {
                Id = todoFromDb.Id,
                Item = todoFromDb.Item,
                Description = todoFromDb.Description,
                IsCompleted = todoFromDb.IsCompleted,
                CreatedAt = todoFromDb.CreatedAt,
                Priority = todoFromDb.Priority

            });
        
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTodo(int id)
        {
            int userId = GetCurrentUserId();

            var todoFromDb = _context.Tpdoitems.FirstOrDefault(t => t.Id == id && t.UserId == userId);
            if (todoFromDb == null)
            {
                throw new NotFoundException($"To-do item with ID {id} not found.");
            }
            _context.Tpdoitems.Remove(todoFromDb);
            _context.SaveChanges();

            return Ok(new ResponseTodoDto
            {
                Id = todoFromDb.Id,
                Item = todoFromDb.Item,
                Description = todoFromDb.Description,
                IsCompleted = todoFromDb.IsCompleted,
                CreatedAt = todoFromDb.CreatedAt,
                Priority = todoFromDb.Priority

            });
        }

        [HttpPatch("{id}/complete")]
        public IActionResult MarkAsCompleted(int id)
        {
            int userId = GetCurrentUserId();
            var todoFromDb = _context.Tpdoitems.FirstOrDefault(t => t.Id == id && t.UserId == userId);
            if (todoFromDb == null)
            {
                throw new NotFoundException($"To-do item with ID {id} not found.");
            }
            todoFromDb.MarkCompleted();
            _context.SaveChanges();

            return Ok(new ResponseTodoDto
            {
                Id = todoFromDb.Id,
                Item = todoFromDb.Item,
                Description = todoFromDb.Description,
                IsCompleted = todoFromDb.IsCompleted,
                CreatedAt = todoFromDb.CreatedAt,
                Priority = todoFromDb.Priority
            });
            
        }

    }
}
