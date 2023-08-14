namespace Mentors.ApplicationCore.Interfaces.IMongoService
{
    public interface IMentorshipSubjectService
    {
        Task<IEnumerable<MentorshipSubjectDto>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<MentorshipSubjectDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<MentorshipSubjectDto> CreateAsync(MentorshipSubjectDto subjectDto, CancellationToken cancellationToken = default);

        Task<MentorshipSubjectDto> UpdateAsync(MentorshipSubjectDto subjectDto, CancellationToken cancellationToken = default);

        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}