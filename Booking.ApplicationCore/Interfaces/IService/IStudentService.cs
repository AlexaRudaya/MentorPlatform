namespace Booking.ApplicationCore.Interfaces.IService
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentDto>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<StudentDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<StudentCreateDto> CreateAsync(StudentCreateDto studentCreateDto, CancellationToken cancellationToken = default);

        Task<StudentDto> UpdateAsync(StudentDto studentDto, CancellationToken cancellationToken = default);

        Task<StudentDto> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}