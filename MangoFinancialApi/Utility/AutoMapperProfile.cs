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


        CreateMap<CreateActorDto, Actor>()
            .ForMember(x => x.PictureRoute, opt => opt.Ignore());    
        CreateMap<Actor, ActorDto>();   


        CreateMap<MovieCreateDto, Movie>()
            .ForMember(x => x.PictureRoute, opt => opt.Ignore());    
        CreateMap<Movie, MovieDto>()
            .ForMember(p => p.Genders, 
            entity => entity.MapFrom(x => 
            x.GenderMovies.Select(gp => 
                new GenderDto {Id = gp.GenderId, Name =gp.Gender.Name})))
            .ForMember(p => p.Actors, entity => entity.MapFrom(p =>
            p.ActorMovies.Select(am => 
                new ActorMovieDto {Id = am.ActorId, 
                    Name = am.Actor.Name, Character = am.Character})));  


        CreateMap<CommentCreateDto, Comment>();
        CreateMap<Comment, CommentDto>();  


        CreateMap<AsignActorMovieDto, ActorMovie>();

    }

}