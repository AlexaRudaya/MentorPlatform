namespace Mentors.Domain.Abstractions.IService
{
    public interface IBaseService<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);

        Task<T> UpdateAsync(int id, T entity, CancellationToken cancellationToken = default);

        Task<T> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}