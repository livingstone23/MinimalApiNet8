using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace MangoDomain.EntititesTest;



public class Movie
{

    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public bool InTheaters { get; set; }

    public DateTime DateRelease { get; set; }

    public string? PictureRoute { get; set; }

    public List<Comment> Comments { get; set; } = new List<Comment>();

    public List<GenderMovie> GenderMovies { get; set; } = new List<GenderMovie>();

    public List<ActorMovie> ActorMovies { get; set; } = new List<ActorMovie>();

}
