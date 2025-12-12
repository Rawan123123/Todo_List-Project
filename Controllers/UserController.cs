using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDo_list.DTO;
using ToDo_list.Exceptions;
using ToDo_list.Helpers;
using ToDo_list.Models;

namespace ToDo_list.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly Context _context;
        private readonly JWTService _jwtService;

        public UserController(Context context , JWTService jWTService)
        {
            _context = context;
            _jwtService = jWTService;
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register(RigesterDto userFromRequestDto)
        {
            if (!ModelState.IsValid)
            {
                    var errors = ModelState
                        .Where(x => x.Value.Errors.Count > 0)
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                        );

                    throw new MyValidationException("Validation failed.", errors);
            }

            User existing = await _context.Users.FirstOrDefaultAsync(u => u.Email == userFromRequestDto.Email);
            if (existing != null)
            {
                throw new BadRequestException($"Email: {userFromRequestDto.Email} already exists");
            }
           //hash password
            string hashed = PasswordHasher.HashPassword(userFromRequestDto.Password);
            User user = new User()
            {
                Name = userFromRequestDto.Name,
                Email = userFromRequestDto.Email,
                PasswordHash = hashed,
                CreatedAt = DateTime.Now,
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(new { user.Id , user.Name , user.Email , user.Role , user.CreatedAt });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto userFromRequestDto)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userFromRequestDto.Email);

            if (user == null) return Unauthorized(new { Message = "Invalid email or password" });

            bool isValidPassword = PasswordHasher.VerifyPassword(userFromRequestDto.Password, user.PasswordHash);
            if(!isValidPassword) return Unauthorized(new { Message = "Invalid email or password" });

            string token = _jwtService.CreateToken(user);


            return Ok(new {user.Id, user.Name, user.Email, user.Role , token });
        }

    }
}
