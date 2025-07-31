namespace CourseHubApi.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int InstructorId { get; set; }
        public User Instructor { get; set; }
    }
}
