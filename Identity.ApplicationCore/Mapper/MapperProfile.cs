namespace Identity.ApplicationCore.Mapper
{
    public sealed class MapperProfile : AutoMapper.Profile
    {
        public MapperProfile()
        {
            CreateMap<ApplicationUser, RegisterDto>().ReverseMap()
                .ForMember(_ => _.UserName, _ => _.MapFrom(_ => _.Email));
            ;
        }
    }
}