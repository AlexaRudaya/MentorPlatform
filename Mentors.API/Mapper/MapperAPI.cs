namespace Mentors.API.Mapper
{
    public sealed class MapperAPI : AutoMapper.Profile
    {
        public MapperAPI()
        {
            CreateMap<Mentor, GetMentorByIdReply>()
                .ForMember(reply => reply.MentorId, options => options.MapFrom(reply => reply.Id))
                .ReverseMap();

            CreateMap<Domain.Entities.Availability, Protos.Availability>()
                .ForMember(availability => availability.Date,
                    options => options.MapFrom(availability => ToUtcTimestamp(availability.Date)))
                .ForMember(availability => availability.StartTime,
                    options => options.MapFrom(availability => ToUtcTimestamp(availability.StartTime)))
                .ForMember(availability => availability.EndTime,
                    options => options.MapFrom(availability => ToUtcTimestamp(availability.EndTime)))
                .ForMember(availability => availability.MentorId, options => options.MapFrom(availability => availability.MentorId));
        }

        private static Timestamp ToUtcTimestamp(DateTime dateTime)
        {
            return Timestamp.FromDateTime(DateTime.SpecifyKind(dateTime, DateTimeKind.Utc));
        }
    }
}