using AutoMapper;
using Microservice.Authentication.Data.Models.User;
using Microservice.Authentication.Dtos.Account;

namespace Microservice.Authentication.Services.AutoMapper
{
    public class AccountMapping : Profile
    {
        public AccountMapping()
        {
            CreateMap<RegisterDto, ApplicationUser>()
                .ForMember(user => user.UserName, map => map.MapFrom(dto => dto.Email));
        }
    }
}
