using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DentistIntern.Models;

namespace DentistIntern.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LecturersController : ControllerBase
    {
        private readonly dentistdbContext _context;

        public LecturersController(dentistdbContext context)
        {
            _context = context;
        }

        // GET: api/Lecturers
        [HttpGet]
        public IEnumerable<Lecturer> GetLecturer()
        {
            return _context.Lecturer;
        }

        // GET: api/Lecturers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLecturer([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var lecturer = await _context.Lecturer.FindAsync(id);

            if (lecturer == null)
            {
                return NotFound();
            }

            return Ok(lecturer);
        }

        // PUT: api/Lecturers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLecturer([FromRoute] int id, [FromBody] Lecturer lecturer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != lecturer.IdLecturer)
            {
                return BadRequest();
            }

            _context.Entry(lecturer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LecturerExists(id))
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

        // POST: api/Lecturers
        [HttpPost]
        public async Task<IActionResult> PostLecturer([FromBody] Lecturer lecturer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Lecturer.Add(lecturer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLecturer", new { id = lecturer.IdLecturer }, lecturer);
        }

        // DELETE: api/Lecturers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLecturer([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var lecturer = await _context.Lecturer.FindAsync(id);
            if (lecturer == null)
            {
                return NotFound();
            }

            _context.Lecturer.Remove(lecturer);
            await _context.SaveChangesAsync();

            return Ok(lecturer);
        }

        private bool LecturerExists(int id)
        {
            return _context.Lecturer.Any(e => e.IdLecturer == id);
        }
    }
}