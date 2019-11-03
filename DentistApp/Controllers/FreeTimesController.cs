using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DentistApp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace DentistApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Intern, Client", AuthenticationSchemes = "client, intern, lecturer")]
    public class FreeTimesController : ControllerBase
    {
        private readonly dentistdbContext _context;

        public FreeTimesController(dentistdbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Intern")]
        // GET: api/FreeTimes
        [HttpGet]
        public IEnumerable<FreeTime> GetFreeTime()
        {
            var claims = User.Claims;
            var cla = claims.ToList();
            var idTrainee = int.Parse(cla[1].Value);

            return _context.FreeTime.Where(l => l.FkTraineeidTrainee == idTrainee);
        }

        [Authorize(Roles = "Client")]
        // GET: api/FreeTimes
        [HttpGet("client")]
        public IEnumerable<FreeTime> GetFreeTimeClient()
        {
            return _context.FreeTime;
        }

        [Authorize(Roles = "Intern")]
        // GET: api/FreeTimes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFreeTime([FromRoute] int id)
        {
            var claims = User.Claims;
            var cla = claims.ToList();
            var idTrainee = int.Parse(cla[1].Value);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var freeTime = await _context.FreeTime.FindAsync(id);

            if (freeTime == null)
            {
                return NotFound();
            }
            if (freeTime.FkTraineeidTrainee!= idTrainee)
            {
                return Unauthorized();
            }
          

            return Ok(freeTime);
        }

        [Authorize(Roles = "Intern")]
        // PUT: api/FreeTimes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFreeTime([FromRoute] int id, [FromBody] FreeTime freeTime)
        {
            var claims = User.Claims;
            var cla = claims.ToList();
            var idTrainee = int.Parse(cla[1].Value);


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var check = _context.FreeTime.Where(l => (l.IdFreeTime == id && l.FkTraineeidTrainee==idTrainee)).Select(x => x.IdFreeTime).Count();
            if (check<1)
            {
                return Unauthorized();
            }
            


            freeTime.IdFreeTime = id;
            freeTime.FkTraineeidTrainee = idTrainee;
            DateTime localDate = DateTime.Now;
            if (localDate > freeTime.TimeStart || localDate > freeTime.TimeEnd)
            {
                return BadRequest("time is already over");
            }
           
            var alreadyis = _context.FreeTime.Where(l => ((l.TimeStart >= freeTime.TimeStart && l.TimeEnd >= freeTime.TimeEnd && freeTime.TimeEnd >= l.TimeStart)
              || (l.TimeStart <= freeTime.TimeStart && l.TimeEnd >= freeTime.TimeEnd)
              || (l.TimeStart <= freeTime.TimeStart && l.TimeEnd >= freeTime.TimeStart && l.TimeEnd <= freeTime.TimeEnd)
              || (l.TimeStart >= freeTime.TimeStart && l.TimeEnd <= freeTime.TimeEnd)
              || (l.TimeStart == freeTime.TimeStart && l.TimeEnd == freeTime.TimeEnd)) && l.FkTraineeidTrainee == idTrainee)
                  .Select(x => x.IdFreeTime).Count();

            if (alreadyis > 1)
            {
                return BadRequest("time is already busy");
            }
            if (freeTime.TimeStart >= freeTime.TimeEnd)
            {
                return BadRequest("end time is bigger then start time");
            }

            _context.Entry(freeTime).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FreeTimeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(freeTime);
        }

        [Authorize(Roles = "Intern")]
        // POST: api/FreeTimes
        [HttpPost]
        public async Task<IActionResult> PostFreeTime([FromBody] FreeTime freeTime)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DateTime localDate = DateTime.Now;
            if (localDate>freeTime.TimeStart || localDate > freeTime.TimeEnd)
            {
                return BadRequest("time is already over");
            }


            var claims = User.Claims;
            var cla = claims.ToList();
            var idTrainee = int.Parse(cla[1].Value);
            
            var alreadyis = _context.FreeTime.Where(l => ((l.TimeStart>= freeTime.TimeStart && l.TimeEnd>=freeTime.TimeEnd &&freeTime.TimeEnd>= l.TimeStart) 
            ||( l.TimeStart<= freeTime.TimeStart && l.TimeEnd>= freeTime.TimeEnd )
            ||(l.TimeStart <= freeTime.TimeStart && l.TimeEnd>=freeTime.TimeStart && l.TimeEnd <=freeTime.TimeEnd)
            ||(l.TimeStart>=freeTime.TimeStart && l.TimeEnd<=freeTime.TimeEnd)
            ||(l.TimeStart == freeTime.TimeStart && l.TimeEnd == freeTime.TimeEnd)) && l.FkTraineeidTrainee == idTrainee)
                .Select(x => x.IdFreeTime).Count();

            if (alreadyis > 0)
            {
                return BadRequest("time is already written");
            }
            if (freeTime.TimeStart>=freeTime.TimeEnd)
            {
                return BadRequest("end time is bigger then start time");
            }
            freeTime.FkTraineeidTrainee = idTrainee;
            _context.FreeTime.Add(freeTime);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFreeTime", new { id = freeTime.IdFreeTime }, freeTime);
        }

        [Authorize(Roles = "Intern")]
        // DELETE: api/FreeTimes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFreeTime([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var claims = User.Claims;
            var cla = claims.ToList();
            var idTrainee = int.Parse(cla[1].Value);

          
            var freeTime = await _context.FreeTime.FindAsync(id);
            if (freeTime == null)
            {
                return NotFound();
            }
       
            if (freeTime.FkTraineeidTrainee!= idTrainee)
            {
                return Unauthorized();
            }

            _context.FreeTime.Remove(freeTime);
            await _context.SaveChangesAsync();

            return Ok(freeTime);
        }

        private bool FreeTimeExists(int id)
        {
            return _context.FreeTime.Any(e => e.IdFreeTime == id);
        }
    }
}