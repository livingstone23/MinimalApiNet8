namespace MangoFinancialApi.Dto;

public class CreateActorDto
{

    public string Name { get; set; } = null!;

}


public class ActorDto
{
    
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime BirthDate { get; set; }

    public string? PictureRoute { get; set; }

}
