using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DentistApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace DentistApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Client, Intern, Lecturer", AuthenticationSchemes = "client, intern, lecturer")]
    public class SessionsController : ControllerBase
    {
        private readonly dentistdbContext _context;

        public SessionsController(dentistdbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Intern, Lecturer")]
        // GET: api/Sessions
        [HttpGet]
        public IEnumerable<Sessionn> GetSession()
        {
            var claims = User.Claims;
            var cla = claims.ToList();
            var idclient = int.Parse(cla[1].Value);

            return _context.Sessionn.Where(l => l.FkClientidClient == idclient); ;
        }




        [Authorize(Roles = "Client")]
        [HttpGet]
        public IEnumerable<Sessionn> GetSessionClient()
        {
            return _context.Sessionn;
        }



        // GET: api/Sessions/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSession([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var session = await _context.Sessionn.FindAsync(id);

            if (session == null)
            {
                return NotFound();
            }

            return Ok(session);
        }

        [Authorize(Roles = "Client")]
        // PUT: api/Sessions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSession([FromRoute] int id, [FromBody] Sessionn session)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var claims = User.Claims;
            var cla = claims.ToList();
            var idClient = int.Parse(cla[1].Value);
            var check = _context.Sessionn.Where(l => (l.IdSession == id && l.FkClientidClient == idClient)).Select(x => x.IdSession).Count();
            if (check < 1)
            {
                return Unauthorized();
            }

            var FreetimesExist = _context.FreeTime.Where(l => l.TimeStart <= session.TimeStart && l.TimeEnd >= session.TimeEnd && l.TimeEnd > session.TimeStart && l.IdFreeTime == session.FkFreeTimeidFreeTime)
             .Select(x => x.IdFreeTime).Count();
            if (FreetimesExist < 1)
            {
                return BadRequest("here is no time in freeTime");
            }


            var alreadyis = _context.Sessionn.Where(l => ((l.TimeStart >= session.TimeStart && l.TimeEnd >= session.TimeEnd && session.TimeEnd >= l.TimeStart)
             || (l.TimeStart <= session.TimeStart && l.TimeEnd >= session.TimeEnd)
             || (l.TimeStart <= session.TimeStart && l.TimeEnd >= session.TimeStart && l.TimeEnd <= session.TimeEnd)
             || (l.TimeStart >= session.TimeStart && l.TimeEnd <= session.TimeEnd)
             || (l.TimeStart == session.TimeStart && l.TimeEnd == session.TimeEnd)) && l.FkFreeTimeidFreeTime == session.FkFreeTimeidFreeTime)
                 .Select(x => x.IdSession).Count();


            if (alreadyis > 1)
            {
                return BadRequest("time is already busy");
            }





            if (id != session.IdSession)
            {
                return BadRequest();
            }

            _context.Entry(session).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SessionExists(id))
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

        [Authorize(Roles = "Client")]
        // POST: api/Sessions
        [HttpPost]
        public async Task<IActionResult> PostSession([FromBody] Sessionn session)
        {
          

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var claims = User.Claims;
            var cla = claims.ToList();
            var idClient = int.Parse(cla[1].Value);
            DateTime localDate = DateTime.Now;
            session.FkClientidClient = idClient;

            if (localDate > session.TimeStart || localDate > session.TimeEnd)
            {
                return BadRequest("time is already over");
            }
            if (session.TimeStart >= session.TimeEnd)
            {
                return BadRequest("end time is bigger then start time");
            }
            var FreetimesExist = _context.FreeTime.Where(l => l.TimeStart <= session.TimeStart && l.TimeEnd >= session.TimeEnd &&l.TimeEnd> session.TimeStart  && l.IdFreeTime == session.FkFreeTimeidFreeTime)
                .Select(x => x.IdFreeTime).Count();
            if (FreetimesExist<1)
            {
                return BadRequest("here is no time in freeTime");
            }


            var alreadyis = _context.Sessionn.Where(l => ((l.TimeStart >= session.TimeStart && l.TimeEnd >= session.TimeEnd && session.TimeEnd >= l.TimeStart)
               || (l.TimeStart <= session.TimeStart && l.TimeEnd >= session.TimeEnd)
               || (l.TimeStart <= session.TimeStart && l.TimeEnd >= session.TimeStart && l.TimeEnd <= session.TimeEnd)
               || (l.TimeStart >= session.TimeStart && l.TimeEnd <= session.TimeEnd)
               || (l.TimeStart == session.TimeStart && l.TimeEnd == session.TimeEnd)) && l.FkFreeTimeidFreeTime == session.FkFreeTimeidFreeTime)
                   .Select(x => x.IdSession).Count();


            if (alreadyis > 0)
            {
                return BadRequest("time is already busy");
            }

            _context.Sessionn.Add(session);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSession", new { id = session.IdSession }, session);
        }


        [Authorize(Roles = "Client")]
        // DELETE: api/Sessions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSession([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var claims = User.Claims;
            var cla = claims.ToList();
            var idClient= int.Parse(cla[1].Value);


            var session = await _context.Sessionn.FindAsync(id);
            if (session == null)
            {
                return NotFound();
            }
            if (session.FkClientidClient != idClient)
            {
                return Unauthorized();
            }


            _context.Sessionn.Remove(session);
            await _context.SaveChangesAsync();

            return Ok(session);
        }

        private bool SessionExists(int id)
        {
            return _context.Sessionn.Any(e => e.IdSession == id);
        }
    }
}