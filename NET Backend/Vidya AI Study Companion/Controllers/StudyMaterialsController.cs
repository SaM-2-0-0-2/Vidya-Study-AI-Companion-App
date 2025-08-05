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
    public class StudyMaterialsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StudyMaterialsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/StudyMaterials
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudyMaterial>>> GetStudyMaterials()
        {
            return await _context.StudyMaterials.ToListAsync();
        }

        // GET: api/StudyMaterials/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudyMaterial>> GetStudyMaterial(int id)
        {
            var studyMaterial = await _context.StudyMaterials.FindAsync(id);

            if (studyMaterial == null)
            {
                return NotFound();
            }

            return studyMaterial;
        }

        // PUT: api/StudyMaterials/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudyMaterial(int id, StudyMaterial studyMaterial)
        {
            if (id != studyMaterial.ResourceId)
            {
                return BadRequest();
            }

            _context.Entry(studyMaterial).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudyMaterialExists(id))
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

        // POST: api/StudyMaterials
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StudyMaterial>> PostStudyMaterial(StudyMaterial studyMaterial)
        {
            _context.StudyMaterials.Add(studyMaterial);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudyMaterial", new { id = studyMaterial.ResourceId }, studyMaterial);
        }

        // DELETE: api/StudyMaterials/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudyMaterial(int id)
        {
            var studyMaterial = await _context.StudyMaterials.FindAsync(id);
            if (studyMaterial == null)
            {
                return NotFound();
            }

            _context.StudyMaterials.Remove(studyMaterial);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudyMaterialExists(int id)
        {
            return _context.StudyMaterials.Any(e => e.ResourceId == id);
        }
    }
}
