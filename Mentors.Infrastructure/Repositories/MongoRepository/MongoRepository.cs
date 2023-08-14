using Mentors.Domain.Abstractions.IRepository.IMongoRepository;
using Mentors.Domain.Entities.MongoDb;
using Mentors.Infrastructure.MongoDb;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Mentors.Infrastructure.Repositories.MongoRepository
{
    public class MongoRepository<T> : IMongoRepository<T> where T : MongoBaseEntity
    {
        private readonly IMongoCollection<T> _mongoCollection;

        public MongoRepository(IOptions<MongoDbSettings> mongoSettings)
        {
            var client = new MongoClient(mongoSettings.Value.ConnectionURI);
            var database = client.GetDatabase(mongoSettings.Value.DatabaseName);
            _mongoCollection = database.GetCollection<T>(mongoSettings.Value.CollectionName);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _mongoCollection.Find(Builders<T>.Filter.Empty).ToListAsync();    
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            var entity = await _mongoCollection.FindAsync(Builders<T>.Filter.Eq(entity => entity.Id, id));

            return await entity.FirstOrDefaultAsync();
        }

        public async Task CreateAsync(T entity)
        {
            await _mongoCollection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            var filter = Builders<T>.Filter.Eq(existingEntity => existingEntity.Id, entity.Id);

            await _mongoCollection.ReplaceOneAsync(filter, entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var filter = Builders<T>.Filter.Eq(existingEntity => existingEntity.Id, id);

            await _mongoCollection.DeleteOneAsync(filter);
        }
    }
}