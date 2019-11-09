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
    public class ClientsController : ControllerBase
    {
        private readonly dentistdbContext _context;

        public ClientsController(dentistdbContext context)
        {
            _context = context;
        }

        // GET: api/Clients
        [HttpGet]
        public IEnumerable<Client> GetClient()
        {
            return _context.Client;
        }

        // GET: api/Clients/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClient([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var client = await _context.Client.FindAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            return Ok(client);
        }

        // PUT: api/Clients/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClient([FromRoute] int id, [FromBody] Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != client.IdClient)
            {
                return BadRequest();
            }

            _context.Entry(client).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
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
        public async Task<IActionResult> Register(Client client)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // IEnumerable < Lecturer > = lecturers;

            int exist = doExist(client);

            if (exist != 0)
            {
                return BadRequest("somethink went wrong");
            }

            _context.Client.Add(client);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClient", new { id = client.IdClient }, client);

        }

        public int doExist(Client client)
        {
            var pass = _context.Lecturer.Where(l => l.Password == client.Password).Select(x => x.IdLecturer).FirstOrDefault();
            var user = _context.Lecturer.Where(l => l.Username == client.Username).Select(x => x.IdLecturer).FirstOrDefault();
            pass = pass + _context.Trainee.Where(l => l.Password == client.Password).Select(x => x.IdTrainee).FirstOrDefault();
            user = user + _context.Trainee.Where(l => l.Username == client.Username).Select(x => x.IdTrainee).FirstOrDefault();
            pass = pass + _context.Client.Where(l => l.Password == client.Password).Select(x => x.IdClient).FirstOrDefault();
            user = user + _context.Client.Where(l => l.Username == client.Username).Select(x => x.IdClient).FirstOrDefault();

            return pass + user;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var client = await _context.Client.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            _context.Client.Remove(client);
            await _context.SaveChangesAsync();

            return Ok(client);
        }

        private bool ClientExists(int id)
        {
            return _context.Client.Any(e => e.IdClient == id);
        }
    }
}