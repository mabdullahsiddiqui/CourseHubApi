using CourseHubApi.Data;
using CourseHubApi.DTOs;
using CourseHubApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CourseHubApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CourseController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ 1. Get All Courses (public)
        [HttpGet]
        public async Task<IActionResult> GetCourses()
        {
            var courses = await _context.Courses
                .Include(c => c.Instructor)
                .Select(c => new
                {
                    c.Id,
                    c.Title,
                    c.Description,
                    c.Price,
                    Instructor = c.Instructor.FullName
                })
                .ToListAsync();

            return Ok(courses);
        }

        // ✅ 2. Get Course by ID (public)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourse(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Instructor)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
                return NotFound();

            return Ok(new
            {
                course.Id,
                course.Title,
                course.Description,
                course.Price,
                Instructor = course.Instructor.FullName
            });
        }

        // 🔒 3. Create a Course (Instructors/Admin only)
        [Authorize(Roles = "Instructor,Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCourse(CourseDto dto)
        {
            var instructorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var course = new Course
            {
                Title = dto.Title,
                Description = dto.Description,
                Price = dto.Price,
                InstructorId = instructorId
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return Ok(course);
        }

        // 🔒 4. Update a Course (Instructors/Admin only)
        [Authorize(Roles = "Instructor,Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, CourseDto dto)
        {
            var instructorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return NotFound();

            if (course.InstructorId != instructorId)
                return Forbid("You can only update your own courses.");

            course.Title = dto.Title;
            course.Description = dto.Description;
            course.Price = dto.Price;

            await _context.SaveChangesAsync();
            return Ok(course);
        }

        // 🔒 5. Delete a Course (Instructors/Admin only)
        [Authorize(Roles = "Instructor,Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var instructorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return NotFound();

            if (course.InstructorId != instructorId)
                return Forbid("You can only delete your own courses.");

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return Ok("Course deleted.");
        }
    }
}
