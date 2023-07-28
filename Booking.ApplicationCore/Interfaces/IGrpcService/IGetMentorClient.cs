namespace Booking.ApplicationCore.Interfaces.IGrpcService
{
    public interface IGetMentorClient
    {
        Task<GetMentorByIdReply> GetMentorAsync(string mentorId,
            CancellationToken cancellationToken = default);
    }
}