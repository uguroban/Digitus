using AutoMapper;
using Digitus.Dtos;
using Digitus.Models;
using MongoDB.Driver;

namespace Digitus.Mapping;

public class AllProfiles : Profile
{
    public AllProfiles()
    {
        #region User

        CreateMap<User, SignupDto>().ReverseMap();
        CreateMap<User, Login>().ReverseMap();
        CreateMap<Login, LoginDto>().ReverseMap();
        CreateMap<Login, ResetPasswordDto>().ReverseMap();
        
        #endregion
    }
}