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
    public class CoursesController : ControllerBase
    {
        private readonly ShkollaContext _context;
        private IAuthService _authService;

        public CoursesController(ShkollaContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            return await _context.Courses.ToListAsync();
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

        // PUT: api/Courses/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, Course course)
        {
            if (id != course.CourseId)
            {
                return BadRequest();
            }

            _context.Entry(course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
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

        // POST: api/Courses
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCourse", new { id = course.CourseId }, course);
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Course>> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return course;
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
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

        //api/Universities/rate/doctor/' + courseId + "/star/+ starRate + '/comment/' + comment,
        [HttpGet("rate/course/{courseId}/star/{starRate}/comment/{comment}")]
        public async Task<ActionResult<bool>> reviewSent(int courseId, int starRate, string comment)
        {
            ReviewHelper review = new ReviewHelper();
            review.Comment = comment;
            review.CourseId = courseId;
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
