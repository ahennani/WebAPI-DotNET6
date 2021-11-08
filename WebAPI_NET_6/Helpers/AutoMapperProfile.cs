namespace WebAPI_NET_6.Helpers;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<RegisterAppUserDTO, AppUser>()
            .ForMember(d => d.UserName, opt => opt.MapFrom(s => s.Email))
            .ForMember(d => d.Id, opt => opt.MapFrom(s => Guid.NewGuid()));
    }
}