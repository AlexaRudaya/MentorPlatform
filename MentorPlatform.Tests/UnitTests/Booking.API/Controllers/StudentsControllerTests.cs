namespace MentorPlatform.Tests.UnitTests.Booking.API.Controllers
{
    public class StudentsControllerTests
    {
        private readonly Mock<IStudentService> _mockStudentService;
        private readonly StudentsController _controller;
        private readonly StudentGenerator _studentGenerator;
        private readonly StudentsControllerHelper _helper;
        private readonly CancellationToken _cancellationToken;

        public StudentsControllerTests()
        {
            _mockStudentService = new Mock<IStudentService>();
            _controller = new StudentsController(
                _mockStudentService.Object);
            _studentGenerator = new StudentGenerator();
            _helper = new StudentsControllerHelper(
                _mockStudentService);
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task GetStudents_WhenModelsAreFound_ShouldReturnOkWithStudents()
        {
            // Arrange
            var students = new List<StudentDto>
            {
                _studentGenerator.GenerateFakeDto(),
                _studentGenerator.GenerateFakeDto(),
                _studentGenerator.GenerateFakeDto(),
            };

            _helper.SetupGetAllAsync(students);

            // Act
            var result = await _controller.GetStudents(_cancellationToken);

            // Assert
            var okResult = result
                .Should().BeOfType<OkObjectResult>().Subject;

            var returnedCategories = okResult.Value
                .Should().BeOfType<List<StudentDto>>();
        }

        [Fact]
        public async Task GetStudent_WhenModelIsFound_ShouldReturnOkWithStudent()
        {
            // Arrange
            var student = _studentGenerator.GenerateFakeDto();

            _helper.SetupGetByIdAsync(student);

            // Act
            var result = await _controller.GetStudent(student.Id, _cancellationToken);

            // Assert
            var okResult = result
                .Should().BeOfType<OkObjectResult>().Subject;

            var returnedCategory = okResult.Value
                .Should().BeOfType<StudentDto>();
        }

        [Fact]
        public async Task CreateStudent_WhenModelIsValid_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var student = _studentGenerator.GenerateFakeStudentCreateDto();

            _helper.SetupCreateAsync(student);

            // Act
            var result = await _controller.CreateStudent(student, _cancellationToken);

            // Assert
            var createdAtResult = result
                .Should().BeOfType<CreatedAtActionResult>().Subject;

            var returnedCategory = createdAtResult.Value
                .Should().BeOfType<StudentCreateDto>();
        }

        [Fact]
        public async Task UpdateStudent_WhenModelIsFound_ShouldReturnNoContent()
        {
            // Arrange
            var student = _studentGenerator.GenerateFakeDto();

            _helper.SetupUpdateAsync(student);

            // Act
            var result = await _controller.UpdateStudent(student, _cancellationToken);

            // Assert
            result
                .Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteStudent_WhenModelIsFound_ShouldReturnNoContent()
        {
            // Arrange
            var student = _studentGenerator.GenerateFakeDto();

            _helper.SetupDeleteAsync(student);

            // Act
            var result = await _controller.DeleteStudent(student.Id, _cancellationToken);

            // Assert
            result
                .Should().BeOfType<NoContentResult>();
        }
    }
}