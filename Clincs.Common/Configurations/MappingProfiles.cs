using Clincs.Common.Models.Database.API;

namespace Clincs.Common.Configurations
{
    public class MappingProfiles : AutoMapper.Profile
    {
        public MappingProfiles()
    {

        CreateMap<Models.Response.AuthenticateResponse, User>();
        CreateMap<Models.Request.CreateRequest, User>();

        }
    }
}
