using AutoMapper;
using MangoDomain.EntititesTest;
using MangoFinancialApi.Dto;



namespace MangoFinancialApi.Utility;



public class AutoMapperProfiles : Profile
{

    public AutoMapperProfiles()
    {

        CreateMap<CreateGenderDto, Gender>();    
        CreateMap<Gender, GenderDto>();    

        CreateMap<CreateActorDto, Actor>();    
        CreateMap<Actor, ActorDto>();    

    }

}