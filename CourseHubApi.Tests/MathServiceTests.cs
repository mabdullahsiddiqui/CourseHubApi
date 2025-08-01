// File: CourseHubApi.Tests/MathServiceTests.cs
using CourseHubApi.Services;
using Xunit;

namespace CourseHubApi.Tests
{
    public class MathServiceTests
    {
        [Fact]
        public void Add_ReturnsCorrectSum()
        {
            var service = new MathService();

            var result = service.Add(2, 3);

            Assert.Equal(5, result);
        }
    }
}
