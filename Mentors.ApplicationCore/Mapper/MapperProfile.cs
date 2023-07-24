namespace Mentors.ApplicationCore.Mapper
{
    public sealed class MapperProfile : AutoMapper.Profile
    {
        public MapperProfile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Availability, AvailabilityDto>().ReverseMap();
            CreateMap<Mentor, MentorDto>().ReverseMap();
            CreateMap<Mentor, MentorCreateDto>().ReverseMap();
        }
    }
}