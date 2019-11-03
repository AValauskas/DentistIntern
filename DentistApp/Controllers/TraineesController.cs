using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DentistApp.Models;

namespace DentistApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TraineesController : ControllerBase
    {
        private readonly dentistdbContext _context;

        public TraineesController(dentistdbContext context)
        {
            _context = context;
        }

        // GET: api/Trainees
        [HttpGet]
        public IEnumerable<Trainee> GetTrainee()
        {
            return _context.Trainee;
        }

        // GET: api/Trainees/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTrainee([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var trainee = await _context.Trainee.FindAsync(id);

            if (trainee == null)
            {
                return NotFound();
            }

            return Ok(trainee);
        }

        // PUT: api/Trainees/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrainee([FromRoute] int id, [FromBody] Trainee trainee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != trainee.IdTrainee)
            {
                return BadRequest();
            }

            _context.Entry(trainee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TraineeExists(id))
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







        [HttpPost("Register")]
        public async Task<IActionResult> Register(Trainee trainee)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // IEnumerable < Lecturer > = lecturers;

            int exist = doExist(trainee);

            if (exist != 0)
            {
                return BadRequest("somethink went wrong");
            }

            _context.Trainee.Add(trainee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTrainee", new { id = trainee.IdTrainee }, trainee);

        }

        public int doExist(Trainee trainee)
        {
            var pass = _context.Lecturer.Where(l => l.Password == trainee.Password).Select(x => x.IdLecturer).FirstOrDefault();
            var user = _context.Lecturer.Where(l => l.Username == trainee.Username).Select(x => x.IdLecturer).FirstOrDefault();
            pass = pass + _context.Trainee.Where(l => l.Password == trainee.Password).Select(x => x.IdTrainee).FirstOrDefault();
            user = user + _context.Trainee.Where(l => l.Username == trainee.Username).Select(x => x.IdTrainee).FirstOrDefault();
            pass = pass + _context.Client.Where(l => l.Password == trainee.Password).Select(x => x.IdClient).FirstOrDefault();
            user = user + _context.Client.Where(l => l.Username == trainee.Username).Select(x => x.IdClient).FirstOrDefault();

            return pass + user;
        }











        /*   // POST: api/Trainees
           [HttpPost]
           public async Task<IActionResult> PostTrainee([FromBody] Trainee trainee)
           {
               if (!ModelState.IsValid)
               {
                   return BadRequest(ModelState);
               }

               _context.Trainee.Add(trainee);
               await _context.SaveChangesAsync();

               return CreatedAtAction("GetTrainee", new { id = trainee.IdTrainee }, trainee);
           }
           */
        // DELETE: api/Trainees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrainee([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var trainee = await _context.Trainee.FindAsync(id);
            if (trainee == null)
            {
                return NotFound();
            }

            _context.Trainee.Remove(trainee);
            await _context.SaveChangesAsync();

            return Ok(trainee);
        }

        private bool TraineeExists(int id)
        {
            return _context.Trainee.Any(e => e.IdTrainee == id);
        }
    }
}