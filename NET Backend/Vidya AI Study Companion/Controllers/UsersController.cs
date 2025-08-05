using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Vidya_AI_Study_Companion.Models;
using Vidya_AI_Study_Companion.DTOs;

namespace Vidya_AI_Study_Companion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public UsersController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        //Get all users
        // Only ADMIN can access this endpoint
        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<IEnumerable<object>>> GetUsers()
        {
            var users = await _context.Users
                .Select(u => new
                {
                    u.Username,
                    u.Email,
                    Role = u.Role.ToString() // Convert enum to string
                })
                .ToListAsync();

            return Ok(users);
        }


        
        //User Login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<object>> Login(UserLoginDTO loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            {
                return Unauthorized("Invalid email or password.");
            }

            var token = GenerateJwtToken(user);

            return Ok(new
            {
                message = "Login successful",
                token,
                user = new
                {
                    user.UserId,
                    user.Username,
                    user.Email,
                    Role = user.Role.ToString()
                }
            });
        }

        //PUT for change password (Optional)
        [HttpPut("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Verify old password
            if (!BCrypt.Net.BCrypt.Verify(dto.OldPassword, user.Password))
            {
                return Unauthorized("Old password is incorrect.");
            }

            // Hash and set new password
            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok("Password changed successfully.");
        }


        

        //Delete User 
        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }


        //Registration
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<object>> Register(User user)
        {
            // Check if user with same email or username already exists
            if (await _context.Users.AnyAsync(u => u.Email == user.Email || u.Username == user.Username))
            {
                return BadRequest("User with this email or username already exists.");
            }

            // Hashing password using BCrypt
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Generate JWT
            var token = GenerateJwtToken(user);

            return Ok(new
            {
                message = "Registration successful",
                token,
                user = new
                {
                    user.UserId,
                    user.Username,
                    user.Email,
                    user.Role
                }
            });
        }

        // JWT Token Generator
        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }
    }
}
