﻿namespace Booking.Infrastructure.Mapper
{
    public class MapperInfrastructure : AutoMapper.Profile
    {
        public MapperInfrastructure()
        {
            CreateMap<GetMentorByIdReply, MentorDto>().ReverseMap();
            CreateMap<Availability, MentorDto>().ReverseMap();
            CreateMap<Timestamp, DateTime>().ConvertUsing(timestamp => timestamp.ToDateTime());
            CreateMap<DateTime, Timestamp>().ConvertUsing(dateTime => Timestamp.FromDateTime(dateTime));
        }
    }
}