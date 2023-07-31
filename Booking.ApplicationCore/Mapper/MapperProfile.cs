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

            CreateMap<MentorBooking, BookingDto>()
                .ForMember(booking => booking.MentorId, options => options.MapFrom(booking => booking.MentorId))
                .ReverseMap();
        }
    }
}