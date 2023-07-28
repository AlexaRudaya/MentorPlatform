namespace Mentors.ApplicationCore.Mapper
{
    public sealed class MapperProfile : AutoMapper.Profile
    {
        public MapperProfile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Domain.Entities.Availability, AvailabilityDto>().ReverseMap();
            CreateMap<Mentor, GetMentorByIdReply>()
                .ForMember(mentor => mentor.MentorId,
                    options => options.MapFrom(mentor => mentor.Id))
                .ReverseMap();

            CreateMap<Domain.Entities.Availability, API.Protos.Availability>()
                .ForMember(availability => availability.Date,
                    options => options.MapFrom(availability => ToUtcTimestamp(availability.Date)))
                .ForMember(availability => availability.StartTime,
                    options => options.MapFrom(availability => ToUtcTimestamp(availability.StartTime)))
                .ForMember(availability => availability.EndTime,
                    options => options.MapFrom(availability => ToUtcTimestamp(availability.EndTime)))
                .ForMember(availability => availability.MentorId, options => options.MapFrom(availability => availability.MentorId));

            CreateMap<Mentor, MentorDto>()
                .ForMember(mentorDto => mentorDto.AvailabilitiesIds,
                    options => options.MapFrom(mentor => mentor.Availabilities.Select(availability => availability.Id)))
                .ReverseMap();
            CreateMap<Mentor, MentorCreateDto>().ReverseMap();
        }

        private static Timestamp ToUtcTimestamp(DateTime dateTime)
        {
            return Timestamp.FromDateTime(DateTime.SpecifyKind(dateTime, DateTimeKind.Utc));
        }
    }
}