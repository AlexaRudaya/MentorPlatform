namespace Mentors.API.Services.GrpcService
{
    public class GetMentorService : MentorByIdService.MentorByIdServiceBase
    {
        private readonly IMentorRepository _mentorRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetMentorService> _logger;

        public GetMentorService(
            IMentorRepository mentorRepository,
            IMapper mapper,
            ILogger<GetMentorService> logger)
        {
            _logger = logger;
            _mapper = mapper;
            _mentorRepository = mentorRepository;
        }

        public override async Task<GetMentorByIdReply> GetMentorById(GetMentorByIdRequest mentorRequest,
            ServerCallContext serverCallContext)
        {
            var mentorId = Guid.Parse(mentorRequest.MentorId);

            var mentor = await _mentorRepository.GetOneByAsync(
               include: query => query
                   .Include(mentor => mentor.Category)
                   .Include(mentor => mentor.Availabilities),
               expression: mentor => mentor.Id.Equals(mentorId));

            if (mentor is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Mentor not found"));
            }

            _logger.LogInformation($"Mentor with ID {mentorId} found");

            var reply = _mapper.Map<GetMentorByIdReply>(mentor);

            return reply;
        }
    }
}