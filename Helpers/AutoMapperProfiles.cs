using AuthDemoAPI.DTOs.Users;
using AuthDemoAPI.Entities.User;
using AutoMapper;

namespace AuthDemoAPI.Helpers
{
    public class CAutoMapperProfiles :Profile
    {
        public CAutoMapperProfiles()
        {
            CreateMap<CAppUser, CUserToReturnDto>();
        }
    }
}