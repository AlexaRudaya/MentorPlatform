namespace Mentors.Domain.Abstractions.IRepository
{
    public interface ICachedMentorRepository
    {
        Task<Mentor> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<IEnumerable<Mentor>> GetAllByAsync(CancellationToken cancellationToken = default);
    }
}