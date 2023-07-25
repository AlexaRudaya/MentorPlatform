namespace Mentors.ApplicationCore.Mapper
{
    public sealed class MapperProfile : AutoMapper.Profile
    {
        public MapperProfile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Availability, AvailabilityDto>().ReverseMap();
            CreateMap<Mentor, MentorDto>()
                .ForMember(mentorDto => mentorDto.AvailabilitiesIds,
                    options => options.MapFrom(mentor => mentor.Availabilities
                                                               .Select(availability => availability.Id)))
                .ReverseMap();
            CreateMap<Mentor, MentorCreateDto>().ReverseMap();
        }
    }
}