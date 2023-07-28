namespace Booking.ApplicationCore.Mapper
{
    public class MapperProfile: AutoMapper.Profile
    {
        public MapperProfile()
        {
            CreateMap<Student, StudentDto>()
                .ForMember(studentDto => studentDto.BookingsIds, options => options.MapFrom(student => student.Bookings
                                                                                   .Select(booking => booking.Id)))
                .ReverseMap();

            CreateMap<Student, StudentCreateDto>().ReverseMap();

            CreateMap<Bookings, BookingsDto>()
                .ForMember(booking => booking.MentorId, options => options.MapFrom(booking => booking.MentorId))
                .ReverseMap();

            CreateMap<Student, StudentCreateDto>().ReverseMap();

            CreateMap<Availability, AvailabilityDto>().ReverseMap();

            CreateMap<Timestamp, DateTime>().ConvertUsing(timestamp => timestamp.ToDateTime());
            CreateMap<DateTime, Timestamp>().ConvertUsing(dateTime => Timestamp.FromDateTime(dateTime));
        }
    }
}