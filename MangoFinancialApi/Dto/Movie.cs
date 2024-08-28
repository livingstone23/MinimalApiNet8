namespace MangoFinancialApi.Dto;




public class MovieCreateDto
{
    public string Title { get; set; } = null!;

    public bool InTheaters { get; set; }

    public DateTime DateRelease { get; set; }
    
    public IFormFile? Picture { get; set; }

}

public class MovieDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;

    public bool InTheaters { get; set; }

    public DateTime DateRelease { get; set; }
    
    public IFormFile? PictureRoute { get; set; }

    public List<CommentDto> Comments { get; set; } = new List<CommentDto>();

    public List<GenderDto> Genders { get;set;} = new List<GenderDto>();

    public List<ActorMovieDto> Actors { get;set;} = new List<ActorMovieDto>();


}