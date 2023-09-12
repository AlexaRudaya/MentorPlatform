namespace MentorPlatform.Tests.UnitTests.Booking.API.Helpers.Students
{
    public class StudentsControllerHelper
    {
        private readonly Mock<IStudentService> _mockStudentService;

        public StudentsControllerHelper(
            Mock<IStudentService> mockStudentService)
        {
            _mockStudentService = mockStudentService;
        }

        public void SetupGetAllAsync(List<StudentDto> students)
        {
            _mockStudentService
                .Setup(service => service.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(students);
        }

        public void SetupGetByIdAsync(StudentDto student)
        {
            _mockStudentService
                .Setup(service => service.GetByIdAsync(
                    student.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(student);
        }

        public void SetupCreateAsync(StudentCreateDto student)
        {
            _mockStudentService
                .Setup(service => service.CreateAsync(
                    student, It.IsAny<CancellationToken>()))
                .ReturnsAsync(student);
        }

        public void SetupUpdateAsync(StudentDto student)
        {
            _mockStudentService
                .Setup(service => service.UpdateAsync(
                    student, It.IsAny<CancellationToken>()))
                .ReturnsAsync(student);
        }

        public void SetupDeleteAsync(StudentDto student)
        {
            _mockStudentService
                .Setup(service => service.DeleteAsync(
                    student.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(student);
        }
    }
}