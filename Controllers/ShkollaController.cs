using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shkolla.Helpers;
using Shkolla.Models;
using Shkolla.Services;

namespace Shkolla.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShkollaController : ControllerBase
    {
        private readonly ShkollaContext _context;
        private IAuthService _authService;

        public ShkollaController(ShkollaContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        // GET: api/Universities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<University>>> GetSpitals()
        {
            return await _context.Universities.ToListAsync();
        }

        // GET: api/Universities/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<University>> GetSpital(int id)
        {
            var spital = await _context.Universities.FindAsync(id);

            if (spital == null)
            {
                return NotFound();
            }

            return spital;
        }


        // PUT: api/Universities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSpital(int id, University spital)
        {
            if (id != spital.Shkolla)
            {
                return BadRequest();
            }

            _context.Entry(spital).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpitalExists(id))
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

        // POST: api/Universities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<University>> PostSpital(University university)
        {
            _context.Universities.Add(university);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSpital", new { id = university.Shkolla }, university);
        }

        // DELETE: api/Universities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSpital(int id)
        {
            var spital = await _context.Universities.FindAsync(id);
            if (spital == null)
            {
                return NotFound();
            }

            _context.Universities.Remove(spital);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SpitalExists(int id)
        {
            return _context.Universities.Any(e => e.Shkolla == id);
        }

        [HttpGet("login/{email}/{password}")]
        public async Task<ActionResult<bool>> login(string email, string password)
        {

            User user = new User();
            user.Email = email;
            user.Password = password;

            HttpClient client = new HttpClient();
            var myContent = JsonConvert.SerializeObject(user);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage userres = await client.PostAsync("http://localhost:26133/api/Users/login", byteContent);
            var res = _authService.generateJwtToken();

            // string responseBody = await response.Content.ReadAsStringAsync();
            // return responseBody.ToLower() == "true";
            return Ok(res);
        }

        //api/Universities/rate/doctor/' + doctorId + "/star/+ starRate + '/comment/' + comment,
        [HttpGet("rate/doctor/{doctorId}/star/{starRate}/comment/{comment}")]
        public async Task<ActionResult<bool>> reviewSent(int doctorId, int starRate, string comment)
        {
            ReviewHelper review = new ReviewHelper();
            review.Comment = comment;
            review.CourseId = doctorId;
            review.StarRate = starRate;
            HttpClient client = new HttpClient();
            var myContent = JsonConvert.SerializeObject(review);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await client.PostAsync("http://localhost:26133/api/StarReviews", byteContent);

            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody.ToLower() == "true";
        }

    }
}
