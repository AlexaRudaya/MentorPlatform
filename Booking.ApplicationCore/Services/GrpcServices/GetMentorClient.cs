namespace Booking.ApplicationCore.Services.GrpcServices
{
    public class GetMentorClient : IGetMentorClient
    {
        private readonly MentorByIdService.MentorByIdServiceClient _mentorClient;
        private readonly MentorApiOptions _options;

        public GetMentorClient(IOptions<MentorApiOptions> options)
        {
            _options = options.Value;
            var channel = GrpcChannel.ForAddress(_options.MentorApiUrl);
            _mentorClient = new MentorByIdService.MentorByIdServiceClient(channel);
        }

        public async Task<GetMentorByIdReply> GetMentorAsync(string mentorId,
            CancellationToken cancellationToken = default)
        {
            var request = new GetMentorByIdRequest { MentorId = mentorId };

            return await _mentorClient.GetMentorByIdAsync(request);
        }
    }
}