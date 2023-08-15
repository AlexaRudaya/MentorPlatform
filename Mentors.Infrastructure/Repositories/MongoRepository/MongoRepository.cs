namespace Mentors.Infrastructure.Repositories.MongoRepository
{
    public class MongoRepository<T> : IMongoRepository<T> where T : MongoBaseEntity
    {
        private readonly IMongoCollection<T> _mongoCollection;

        public MongoRepository(
            IOptions<MongoDbSettings> mongoSettings)
        {
            var client = new MongoClient(mongoSettings.Value.ConnectionURI);
            var database = client.GetDatabase(mongoSettings.Value.DatabaseName);
            _mongoCollection = database.GetCollection<T>(mongoSettings.Value.CollectionName);
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _mongoCollection.Find(Builders<T>.Filter.Empty).ToListAsync(cancellationToken);    
        }

        public async Task<T> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var entity = await _mongoCollection.FindAsync(Builders<T>.Filter.Eq(entity => entity.Id, id));

            return await entity.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task CreateAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _mongoCollection.InsertOneAsync(entity, cancellationToken: cancellationToken);
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            var filter = Builders<T>.Filter.Eq(existingEntity => existingEntity.Id, entity.Id);

            await _mongoCollection.ReplaceOneAsync(filter, entity, cancellationToken: cancellationToken);
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var filter = Builders<T>.Filter.Eq(existingEntity => existingEntity.Id, id);

            await _mongoCollection.DeleteOneAsync(filter, cancellationToken);
        }
    }
}