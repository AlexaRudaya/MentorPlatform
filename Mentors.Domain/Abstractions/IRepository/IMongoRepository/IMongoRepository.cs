namespace Mentors.Domain.Abstractions.IRepository.IMongoRepository
{
    public interface IMongoRepository<T> where T : MongoBaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<T> GetByIdAsync(string id, CancellationToken cancellationToken = default);

        Task CreateAsync(T entity, CancellationToken cancellationToken = default);

        Task DeleteAsync(string id, CancellationToken cancellationToken = default);

        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    }
}