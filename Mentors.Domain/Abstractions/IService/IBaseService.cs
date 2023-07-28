namespace Mentors.Domain.Abstractions.IService
{
    public interface IBaseService<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);

        Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);

        Task<T> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}