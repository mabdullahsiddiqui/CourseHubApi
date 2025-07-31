// Controllers/EnrollmentController.cs
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
    public class EnrollmentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EnrollmentController(AppDbContext context)
        {
            _context = context;
        }

        // 🔒 1. Student enrolls in a course
        [Authorize(Roles = "Student")]
        [HttpPost]
        public async Task<IActionResult> Enroll(EnrollDto dto)
        {
            var studentId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            bool alreadyEnrolled = await _context.Enrollments
                .AnyAsync(e => e.UserId == studentId && e.CourseId == dto.CourseId);

            if (alreadyEnrolled)
                return BadRequest("Already enrolled.");

            var enrollment = new Enrollment
            {
                UserId = studentId,
                CourseId = dto.CourseId
            };

            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            return Ok("Enrolled successfully.");
        }

        // 🔒 2. Student sees their own enrollments
        [Authorize(Roles = "Student")]
        [HttpGet("my")]
        public async Task<IActionResult> GetMyEnrollments()
        {
            var studentId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var enrollments = await _context.Enrollments
                .Include(e => e.Course)
                .Where(e => e.UserId == studentId)
                .Select(e => new
                {
                    e.Id,
                    e.EnrolledAt,
                    CourseTitle = e.Course.Title,
                    CourseDescription = e.Course.Description
                })
                .ToListAsync();

            return Ok(enrollments);
        }

        // 🔒 3. Admin: get all enrollments
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllEnrollments()
        {
            var all = await _context.Enrollments
                .Include(e => e.User)
                .Include(e => e.Course)
                .Select(e => new
                {
                    e.Id,
                    Student = e.User.FullName,
                    Course = e.Course.Title,
                    e.EnrolledAt
                })
                .ToListAsync();

            return Ok(all);
        }
    }
}
