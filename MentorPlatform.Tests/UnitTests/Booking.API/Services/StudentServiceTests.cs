using Booking.ApplicationCore.DTO;
using Booking.ApplicationCore.Exceptions;
using Booking.ApplicationCore.Interfaces.IService;
using Booking.ApplicationCore.Services;
using Booking.Domain.Abstractions.IRepository;
using Booking.Domain.Entities;
using MentorPlatform.Tests.UnitTests.Booking.API.BogusData;
using MentorPlatform.Tests.UnitTests.Booking.API.Helpers.Students;
using Moq;

namespace MentorPlatform.Tests.UnitTests.Booking.API.Services
{
    public class StudentServiceTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IStudentRepository> _mockStudentRepository;
        private readonly Mock<ILogger<StudentService>> _mockLogger;
        private readonly IStudentService _studentService;
        private readonly StudentGenerator _studentGenerator;
        private readonly StudentServiceHelper _helper;
        private readonly CancellationToken _cancellationToken;
        public StudentServiceTests()
        {
            _mockMapper = new Mock<IMapper>();  
            _mockStudentRepository = new Mock<IStudentRepository>();
            _mockLogger = new Mock<ILogger<StudentService>>();
            _studentService = new StudentService(
                _mockStudentRepository.Object,
                _mockMapper.Object,
                _mockLogger.Object);
            _studentGenerator = new StudentGenerator();
            _helper = new StudentServiceHelper(_mockMapper, 
                _mockStudentRepository);
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task GetAllAsync_WhenStudentsAreFound_ShouldReturnListOfStudentsDto()
        {
            // Arrange
            var students = new List<Student>
            {
                _studentGenerator.GenerateFakeStudent(),
                _studentGenerator.GenerateFakeStudent(),
                _studentGenerator.GenerateFakeStudent(),
            };
            var studentsDto = _helper.GenerateDtoList(students);

            _helper.SetupGetAllAsync(students);
            _mockMapper
                .Setup(mapper => mapper.Map<IEnumerable<StudentDto>>(students))
                .Returns(studentsDto);

            // Act
            var result = await _studentService.GetAllAsync(_cancellationToken);

            // Assert
            result
                .Should().NotBeNull();
            result
                .Should().HaveCount(3);
        }

        [Fact]
        public async Task GetAllAsync_WhenStudentsAreNull_ShouldThrowStudentNotFoundException()
        {
            // Arrange
            _helper.SetupGetAllAsyncWhenNull();

            // Act
            var result = async() => await _studentService.GetAllAsync(_cancellationToken);

            // Assert
            await result
                .Should().ThrowAsync<StudentNotFoundException>();
        }

        [Fact]
        public async Task GetByIdAsync_WhenStudentIsFound_ShouldReturnStudentDto()
        {
            // Arrange
            var student = _studentGenerator.GenerateFakeStudent();
            var studentDto = _helper.GenerateDtoFromStudent(student);

            _helper.SetupGetByIdAsync(student);
            _helper.SetupMapperForStudentToDto(student, studentDto);

            // Act
            var result = await _studentService.GetByIdAsync(student.Id, _cancellationToken);

            // Assert
            result
                .Should().NotBeNull();
            result.Id
                .Should().Be(student.Id);
        }

        [Fact]
        public async Task GetByIdAsync_WhenCategoryIsNull_ShouldThrowCategoryNotFoundException()
        {
            // Arrange
            var studentId = _studentGenerator.GenerateFakeStudent().Id;

            _helper.SetupGetByIdAsyncWhenNull();

            // Act
            var result = async() => await _studentService.GetByIdAsync(studentId, _cancellationToken);

            // Assert
            await result
                .Should().ThrowAsync<StudentNotFoundException>();
        }

        [Fact]
        public async Task CreateAsync_WhenModelIsValid_ShouldReturnCreatedStudentDto()
        {
            // Arrange
            var student = _studentGenerator.GenerateFakeStudent();
            var studentDto = _helper.GenerateCreateDtoFromStudent(student);

            _helper.SetupMapperForCreateDtoToStudent(student, studentDto);
            _helper.SetupCreateAsync(student);

            // Act
            var result = await _studentService.CreateAsync(studentDto, _cancellationToken);

            // Assert
            result
                .Should().NotBeNull();
            result
                .Id.Should().Be(studentDto.Id);

            _mockStudentRepository
                .Verify(repository => repository
                    .CreateAsync(student, _cancellationToken),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WhenStudentIsFound_ShouldReturnUpdatedStudentDto()
        {
            // Arrange
            var student = _studentGenerator.GenerateFakeStudent();
            var studentDto = _helper.GenerateDtoFromStudent(student);

            _helper.SetupGetByIdAsync(student);
            _helper.SetupMapperForDtoToStudent(student, studentDto);

            // Act
            var result = await _studentService.UpdateAsync(studentDto, _cancellationToken);

            // Assert
            result
                .Should().NotBeNull();

            _mockStudentRepository
                .Verify(repository => repository
                    .UpdateAsync(student, _cancellationToken),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WhenStudentIsNull_ShouldThrowStudentNotFoundException()
        {
            // Arrange
            var student = _studentGenerator.GenerateFakeStudent();
            var studentDto = _helper.GenerateDtoFromStudent(student);

            _helper.SetupGetByIdAsyncWhenNull();

            // Act
            var result = async() => await _studentService.UpdateAsync(studentDto, _cancellationToken);

            // Assert
            await result
                .Should().ThrowAsync<StudentNotFoundException>();
        }

        [Fact]
        public async Task DeleteAsync_WhenStudentIsFound_ShouldDeleteStudent()
        {
            // Arrange
            var student = _studentGenerator.GenerateFakeStudent();
            var studentDto = _helper.GenerateDtoFromStudent(student);

            _helper.SetupGetByIdAsync(student);
            _helper.SetupMapperForStudentToDto(student, studentDto);

            // Act
            var result = await _studentService.DeleteAsync(student.Id);

            // Assert
            result
                .Should().NotBeNull();

            _mockStudentRepository
                .Verify(repository => repository
                    .DeleteAsync(student, _cancellationToken),
                Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WhenStudentIsNull_ShouldThrowStudentNotFoundException()
        {
            // Arrange
            var student = _studentGenerator.GenerateFakeStudent();
            var studentDto = _helper.GenerateDtoFromStudent(student);

            _helper.SetupGetByIdAsyncWhenNull();

            // Act
            var result = async() => await _studentService.DeleteAsync(student.Id, _cancellationToken);

            // Assert
            await result
                .Should().ThrowAsync<StudentNotFoundException>();
        }
    }
}