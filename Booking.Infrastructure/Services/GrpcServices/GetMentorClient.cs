namespace Booking.Infrastructure.Services.GrpcServices
{
    public class GetMentorClient : IGetMentorClient
    {
        private readonly MentorByIdService.MentorByIdServiceClient _mentorClient;
        private readonly MentorApiOptions _options;
        private readonly IMapper _mapper;

        public GetMentorClient(
            IOptions<MentorApiOptions> options,
            IMapper mapper)
        {
            _options = options.Value;
            _mapper = mapper;
            var channel = GrpcChannel.ForAddress(_options.MentorApiUrl);
            _mentorClient = new MentorByIdService.MentorByIdServiceClient(channel);
        }

        public async Task<MentorDto> GetMentorAsync(string mentorId,
            CancellationToken cancellationToken = default)
        {
            var request = new GetMentorByIdRequest { MentorId = mentorId };

            var reply = await _mentorClient.GetMentorByIdAsync(request);

            var mentorDto = _mapper.Map<MentorDto>(reply);

            return mentorDto;
        }
    }
}