using Mentors.Domain.Entities.MongoDb;

namespace Mentors.Domain.Abstractions.IRepository.IMongoRepository
{
    public interface IMongoRepository<T> where T : MongoBaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(Guid id);

        Task CreateAsync(T entity);

        Task DeleteAsync(Guid id);

        Task UpdateAsync(T entity);
    }
}