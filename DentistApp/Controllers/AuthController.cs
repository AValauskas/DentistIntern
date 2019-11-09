using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using DentistApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DentistApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly dentistdbContext _context;


        public AuthController(dentistdbContext context)
        {
            _context = context;
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(Lecturer lecturer)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            int exist = doExistLogin(lecturer);

            if (exist == 0)
            {
                return BadRequest("Here is no such user");
            }
            else if (exist - 3000 > 0) { int id = exist - 3000; return Ok(new JwtSecurityTokenHandler().WriteToken(ClientToken(id))); }
            else if(exist -2000 > 0) { int id = exist - 2000; return Ok(new JwtSecurityTokenHandler().WriteToken(InternToken(id))); }
            else { int id = exist - 1000; return Ok(new JwtSecurityTokenHandler().WriteToken(LecturerToken(id))); }

        }



        public int doExist(Lecturer lecturer)
        {
            var pass = _context.Lecturer.Where(l => l.Password == lecturer.Password).Select(x => x.IdLecturer).FirstOrDefault();
            var user = _context.Lecturer.Where(l => l.Username == lecturer.Username).Select(x => x.IdLecturer).FirstOrDefault();
            pass = pass + _context.Trainee.Where(l => l.Password == lecturer.Password).Select(x => x.IdTrainee).FirstOrDefault();
            user = user + _context.Trainee.Where(l => l.Username == lecturer.Username).Select(x => x.IdTrainee).FirstOrDefault();
            pass = pass + _context.Client.Where(l => l.Password == lecturer.Password).Select(x => x.IdClient).FirstOrDefault();
            user = user + _context.Client.Where(l => l.Username == lecturer.Username).Select(x => x.IdClient).FirstOrDefault();
            return pass + user;
        }


        public int doExistLogin(Lecturer lecturer)
        {
            int pass = 0;
            int user = 0;
            pass = _context.Lecturer.Where(l => l.Password == lecturer.Password).Select(x => x.IdLecturer).FirstOrDefault();
            user = _context.Lecturer.Where(l => l.Username == lecturer.Username).Select(x => x.IdLecturer).FirstOrDefault();
            if (pass>0 && user>0)
            {
                return pass + 1000;
            }
            pass = pass + _context.Trainee.Where(l => l.Password == lecturer.Password).Select(x => x.IdTrainee).FirstOrDefault();
            user = user + _context.Trainee.Where(l => l.Username == lecturer.Username).Select(x => x.IdTrainee).FirstOrDefault();
            if (pass > 0 && user > 0)
            {
                return pass + 2000;
            }
            pass = pass + _context.Client.Where(l => l.Password == lecturer.Password).Select(x => x.IdClient).FirstOrDefault();
            user = user + _context.Client.Where(l => l.Username == lecturer.Username).Select(x => x.IdClient).FirstOrDefault();
            if (pass > 0 && user > 0)
            {
                return pass + 3000;
            }
            else { return 0; }
        }

        public JwtSecurityToken LecturerToken(int id)
        {
            Environment.SetEnvironmentVariable("TestLecturer", "this_is_Lecturer_key");

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("TestLecturer")));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

           // add claims
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Role, "Lecturer"));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, id.ToString()));

            //create token
            var token = new JwtSecurityToken(
                issuer: "dentist.lt",
                audience: "readers",
                expires: DateTime.Now.AddHours(3),
                signingCredentials: signingCredentials,
                //Id: lecturer.IdLecturer,
                claims: claims
                       
                );
            return token;
           // return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }

        public JwtSecurityToken InternToken(int id)
        {
            Environment.SetEnvironmentVariable("TestIntern", "this_is_Intern_key");

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("TestIntern")));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            // add claims
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Role, "Intern"));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, id.ToString()));

            //create token
            var token = new JwtSecurityToken(
                issuer: "dentist.lt",
                audience: "readers",
                expires: DateTime.Now.AddHours(3),
                signingCredentials: signingCredentials,
                claims: claims
                );
            return token;
            // return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }

        public JwtSecurityToken ClientToken(int id)
        {
            Environment.SetEnvironmentVariable("TestClient", "this_is_Client_key");

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("TestClient")));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            // add claims
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Role, "Client"));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, id.ToString()));

            //create token
            var token = new JwtSecurityToken(
                issuer: "dentist.lt",
                audience: "readers",
                expires: DateTime.Now.AddHours(3),
                signingCredentials: signingCredentials,
                claims: claims
                );
            return token;
            // return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }


    }
}