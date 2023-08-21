using Chat.ApplicationCore.DTO;
using Chat.Domain.Entities;

namespace Chat.ApplicationCore.Mapper
{
    public sealed class MapperProfile : AutoMapper.Profile
    {
        public MapperProfile()
        {
            CreateMap<Message, MessageDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}