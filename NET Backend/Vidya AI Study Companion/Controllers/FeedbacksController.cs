using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Vidya_AI_Study_Companion.Models;

namespace Vidya_AI_Study_Companion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbacksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FeedbacksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Feedbacks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Feedback>> GetFeedback(int id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);

            if (feedback == null)
            {
                return NotFound();
            }

            return feedback;
        }

        /*
        // PUT: api/Feedbacks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFeedback(int id, Feedback feedback)
        {
            if (id != feedback.FId)
            {
                return BadRequest();
            }

            _context.Entry(feedback).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeedbackExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        */

        // POST: api/Feedbacks
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Feedback>> PostFeedback(Feedback feedback)
        {
            // Get user ID from JWT
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized("Invalid token. User not identified.");

            int userId = int.Parse(userIdClaim.Value);
            feedback.UserId = userId;

            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFeedback", new { id = feedback.FId }, feedback);
        }


        // DELETE: api/Feedbacks/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN,TEACHER")]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null)
            {
                return NotFound();
            }

            _context.Feedbacks.Remove(feedback);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FeedbackExists(int id)
        {
            return _context.Feedbacks.Any(e => e.FId == id);
        }

        //Get all student feedbacks (accessed by teachers)
        [HttpGet("student-feedbacks")]
        [Authorize(Roles = "TEACHER")]
        public async Task<ActionResult<IEnumerable<object>>> GetStudentFeedbacks()
        {
            var feedbacks = await _context.Feedbacks
                .Include(f => f.User)
                .Where(f => f.User.Role == UserRole.USER)
                .Select(f => new
                {
                    f.FId,
                    f.Title,
                    f.FeedbackText,
                    Username = f.User.Username,
                    Email = f.User.Email
                })
                .ToListAsync();

            return Ok(feedbacks);
        }

        //Get all teacher feedbacks (accessed by admin)
        [HttpGet("teacher-feedbacks")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<IEnumerable<object>>> GetTeacherFeedbacks()
        {
            var feedbacks = await _context.Feedbacks
                .Include(f => f.User)
                .Where(f => f.User.Role == UserRole.TEACHER)
                .Select(f => new
                {
                    f.FId,
                    f.Title,
                    f.FeedbackText,
                    Username = f.User.Username,
                    Email = f.User.Email
                })
                .ToListAsync();

            return Ok(feedbacks);
        }
    }
}
