namespace Mentors.ApplicationCore.Interfaces.IProducer
{
    public interface IProducer
    {
        Task PublishAsync<T>(T message, CancellationToken cancellationToken = default)
            where T : class;
    }
}