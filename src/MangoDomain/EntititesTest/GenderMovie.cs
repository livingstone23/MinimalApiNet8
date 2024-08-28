namespace MangoDomain.EntititesTest;

public class GenderMovie
{

    public int GenderId { get; set; }
    public Gender Gender { get; set; } = null!;

    public int MovieId { get; set; }
    public Movie Movie { get; set; } = null!;
    
}
