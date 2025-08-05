using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vidya_AI_Study_Companion.Models;

namespace Vidya_AI_Study_Companion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SummariesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SummariesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Summaries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Summary>>> GetSummaries()
        {
            return await _context.Summaries.ToListAsync();
        }

        // GET: api/Summaries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Summary>> GetSummary(int id)
        {
            var summary = await _context.Summaries.FindAsync(id);

            if (summary == null)
            {
                return NotFound();
            }

            return summary;
        }

        // PUT: api/Summaries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSummary(int id, Summary summary)
        {
            if (id != summary.SummaryId)
            {
                return BadRequest();
            }

            _context.Entry(summary).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SummaryExists(id))
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

        // POST: api/Summaries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Summary>> PostSummary(Summary summary)
        {
            _context.Summaries.Add(summary);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSummary", new { id = summary.SummaryId }, summary);
        }

        // DELETE: api/Summaries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSummary(int id)
        {
            var summary = await _context.Summaries.FindAsync(id);
            if (summary == null)
            {
                return NotFound();
            }

            _context.Summaries.Remove(summary);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SummaryExists(int id)
        {
            return _context.Summaries.Any(e => e.SummaryId == id);
        }
    }
}
