namespace Booking.ApplicationCore.Interfaces.IGrpcService
{
    public interface IGetMentorClient
    {
        Task<MentorDto> GetMentorAsync(string mentorId,
            CancellationToken cancellationToken = default);
    }
}