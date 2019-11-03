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
    public class ResponsesController : ControllerBase
    {
        private readonly dentistdbContext _context;

        public ResponsesController(dentistdbContext context)
        {
            _context = context;
        }

        // GET: api/Responses
        [HttpGet]
        public IEnumerable<Response> GetResponse()
        {
            return _context.Response;
        }


        [Authorize(Roles = "Intern")]
        // GET: api/Responses
        [HttpGet("trainee")]
        public IEnumerable<Response> GetResponseTrainee()
        {

            var claims = User.Claims;
            var cla = claims.ToList();
            var idTrainee = int.Parse(cla[1].Value);
            return _context.Response.Where(l => l.FkTraineeidTrainee == idTrainee);
        }

        // GET: api/Responses/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetResponse([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _context.Response.FindAsync(id);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }


        [Authorize(Roles = "Client")]
        // PUT: api/Responses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutResponse([FromRoute] int id, [FromBody] Response response)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

          
            var claims = User.Claims;
            var cla = claims.ToList();
            var idClient = int.Parse(cla[1].Value);
          //  var responsegot = _context.Response.Where(l => (l.IdResponse == id && l.FkClientidClient == idClient)).Select(x => x.IdResponse);
          //  response.FkTraineeidTrainee = responsegot.FkTraineeidTrainee;
            response.IdResponse = id;
            response.FkClientidClient = idClient;
            var check = _context.Response.Where(l => (l.IdResponse == id && l.FkClientidClient == idClient)).Select(x => x.IdResponse).Count();
            if (check < 1)
            {
                return Unauthorized();
            }

            _context.Entry(response).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResponseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(response);
        }

        [Authorize(Roles = "Client")]
        // POST: api/Responses
        [HttpPost]
        public async Task<IActionResult> PostResponse([FromBody] Response response)
        {
            var claims = User.Claims;
            var cla = claims.ToList();
            var idClient = int.Parse(cla[1].Value);

            response.FkClientidClient = idClient;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Response.Add(response);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetResponse", new { id = response.IdResponse }, response);
        }

        [Authorize(Roles = "Client")]
        // DELETE: api/Responses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResponse([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var claims = User.Claims;
            var cla = claims.ToList();
            var idClient = int.Parse(cla[1].Value);


            var response = await _context.Response.FindAsync(id);
            if (response == null)
            {
                return NotFound();
            }
            if (response.FkClientidClient != idClient)
            {
                return Unauthorized();
            }
            _context.Response.Remove(response);
            await _context.SaveChangesAsync();

            return Ok(response);
        }

        private bool ResponseExists(int id)
        {
            return _context.Response.Any(e => e.IdResponse == id);
        }
    }
}