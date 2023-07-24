namespace Mentors.ApplicationCore.Interfaces.IService
{
    public interface IMentorService
    {
        Task<IEnumerable<MentorDto>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<MentorDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<MentorCreateDto> CreateAsync(MentorCreateDto mentorCreateDto, CancellationToken cancellationToken = default);

        Task<MentorDto> UpdateAsync(Guid id, MentorDto mentorDto, CancellationToken cancellationToken = default);

        Task<MentorDto> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}