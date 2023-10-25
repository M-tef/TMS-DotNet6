using AutoMapper;
using TMSBlazorAPI.Data;
using TMSBlazorAPI.Models.Club;
using TMSBlazorAPI.Models.User;

namespace TMSBlazorAPI.Configuration
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<UserReadOnlyDto, User>().ReverseMap();
            CreateMap<UserCreateDto, User>().ReverseMap();
            CreateMap<UserUpdateDto, User>().ReverseMap();

            CreateMap<ClubCreateDto,Club > ().ReverseMap();
            CreateMap<ClubReadOnlyDto,Club > ().ReverseMap();
            CreateMap<ClubUpdateDto,Club > ().ReverseMap();
            CreateMap<Club, ClubReadOnlyDto> ()
                .ForMember(q=>q.CreatedBy, d=>d.MapFrom(map => $"{map.User.FirstName} {map.User.LastName}"))
                .ReverseMap();

            CreateMap<Club, ClubDetailDto>()
                .ForMember(q => q.Users, d => d.MapFrom(map => $"{map.User.FirstName} {map.User.LastName}"))
                .ReverseMap();
        }
    }
}
