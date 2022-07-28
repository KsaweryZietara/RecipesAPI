using AutoMapper;
using RecipesAPI.Api.Dtos;
using RecipesAPI.Api.Models;

namespace RecipesAPI.Api.Profiles{

    public class UsersProfile : Profile{
        public UsersProfile(){
            CreateMap<RegisterDto, User>();
        }
    }
}