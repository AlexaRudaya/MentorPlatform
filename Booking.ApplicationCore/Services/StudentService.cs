namespace Booking.ApplicationCore.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<StudentService> _logger;

        public StudentService(
            IStudentRepository studentRepository,
            IMapper mapper,
            ILogger<StudentService> logger)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<StudentDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var allStudents = await _studentRepository.GetAllByAsync(
                include: query => query
                    .Include(student => student.Bookings),
                    cancellationToken: cancellationToken);

            if (allStudents is null)
            {
                throw new StudentNotFoundException();
            }

            var studentsDto = _mapper.Map<IEnumerable<StudentDto>>(allStudents);

            _logger.LogInformation("Students list is loaded");

            return studentsDto;
        }

        public async Task<StudentDto> GetByIdAsync(Guid studentId, CancellationToken cancellationToken = default)
        {
            var student = await _studentRepository.GetOneByAsync(
               include: query => query
                   .Include(student => student.Bookings),
               expression: student => student.Id.Equals(studentId),
               cancellationToken: cancellationToken);

            if (student is null)
            {
                throw new StudentNotFoundException(studentId);
            }

            var studentDto = _mapper.Map<StudentDto>(student);

            return studentDto;
        }

        public async Task<StudentCreateDto> CreateAsync(StudentCreateDto studentCreateDto,
            CancellationToken cancellationToken = default)
        {
            var studentToCreate = _mapper.Map<Student>(studentCreateDto);

            await _studentRepository.CreateAsync(studentToCreate, cancellationToken);

            _logger.LogInformation($"Student with Id: {studentToCreate.Id} is created.");

            return studentCreateDto;
        }

        public async Task<StudentDto> UpdateAsync(StudentDto studentDto,
            CancellationToken cancellationToken = default)
        {
            var existingStudent = await _studentRepository.GetOneByAsync(expression: student => student.Id.Equals(studentDto.Id));

            if (existingStudent is null)
            {
                _logger.LogError($"Failed finding student with Id:{studentDto.Id} while updating entity.");
                throw new StudentNotFoundException(studentDto.Id);
            }

            var studentToUpdate = _mapper.Map<Student>(studentDto);

            await _studentRepository.UpdateAsync(studentToUpdate, cancellationToken);

            _logger.LogInformation($"Data for student with Id: {existingStudent.Id} has been successfully updated.");

            return studentDto;
        }

        public async Task<StudentDto> DeleteAsync(Guid studentId, CancellationToken cancellationToken = default)
        {
            var studentToDelete = await _studentRepository.GetOneByAsync(expression: student => student.Id.Equals(studentId));

            if (studentToDelete is null)
            {
                _logger.LogError($"Failed finding student with Id:{studentId} while deleting entity.");
                throw new StudentNotFoundException(studentId);
            }

            var studentDeleted = _mapper.Map<StudentDto>(studentToDelete);

            await _studentRepository.DeleteAsync(studentToDelete, cancellationToken);

            _logger.LogInformation($"Student with Id: {studentId} is removed");

            return studentDeleted;
        }
    }
}