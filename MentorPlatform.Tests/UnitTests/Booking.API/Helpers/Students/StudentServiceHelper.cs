using Booking.ApplicationCore.DTO;
using Booking.Domain.Abstractions.IRepository;
using Booking.Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System.Linq.Expressions;

namespace MentorPlatform.Tests.UnitTests.Booking.API.Helpers.Students
{
    public class StudentServiceHelper
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IStudentRepository> _mockStudentRepository;

        public StudentServiceHelper(
            Mock<IMapper> mockMapper,
            Mock<IStudentRepository> mockStudentRepository)
        {
            _mockMapper = mockMapper;
            _mockStudentRepository = mockStudentRepository;
        }

        public void SetupGetAllAsync(List<Student> students)
        {
            _mockStudentRepository
                .Setup(repository => repository.GetAllByAsync(
                    It.IsAny<Func<IQueryable<Student>, 
                    IIncludableQueryable<Student, object>>>(),
                    null,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(students);
        }

        public void SetupGetAllAsyncWhenNull()
        {
            _mockStudentRepository
                .Setup(repository => repository.GetAllByAsync(
                    It.IsAny<Func<IQueryable<Student>, 
                    IIncludableQueryable<Student, object>>>(),
                    null,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<Student>)null);
        }

        public void SetupGetByIdAsync(Student student)
        {
            _mockStudentRepository
                .Setup(repository => repository.GetOneByAsync(
                    It.IsAny<Func<IQueryable<Student>, 
                    IIncludableQueryable<Student, object>>>(),
                    It.IsAny<Expression<Func<Student, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(student);
        }

        public void SetupGetByIdAsyncWhenNull()
        {
            _mockStudentRepository
                .Setup(repository => repository.GetOneByAsync(
                    null,
                    It.IsAny<Expression<Func<Student, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Student)null);
        }

        public void SetupCreateAsync(Student student)
        {
            _mockStudentRepository
                .Setup(repository => repository.CreateAsync(
                    student, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
        }

        public void SetupMapperForStudentToDto(Student student,
            StudentDto studentDto)
        {
            _mockMapper
                .Setup(mapper => mapper.Map<StudentDto>(student))
                .Returns(studentDto);
        }

        public void SetupMapperForDtoToStudent(Student student,
            StudentDto studentDto)
        {
            _mockMapper
                .Setup(mapper => mapper.Map<Student>(studentDto))
                .Returns(student);
        }

        public void SetupMapperForStudentToCreateDto(Student student,
            StudentCreateDto studentDto)
        {
            _mockMapper
                .Setup(mapper => mapper.Map<StudentCreateDto>(student))
                .Returns(studentDto);
        }

        public void SetupMapperForCreateDtoToStudent(Student student,
            StudentCreateDto studentDto)
        {
            _mockMapper
                .Setup(mapper => mapper.Map<Student>(studentDto))
                .Returns(student);
        }

        public List<StudentDto> GenerateDtoList(IEnumerable<Student> students)
        {
            return students.Select(student => new StudentDto
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email
            }).ToList();
        }

        public StudentDto GenerateDtoFromStudent(Student student)
        {
            return new StudentDto
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email
            };
        }

        public StudentCreateDto GenerateCreateDtoFromStudent(Student student)
        {
            return new StudentCreateDto
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email
            };
        }
    }
}