namespace MangoFinancialApi.Dto;


public class CommentDto
{

    public int Id { get; set; }
    public string content { get; set; } = null!;

    public int MovieId { get; set; }

}


public class CommentCreateDto
{
    public string Content { get; set; } = null!;

}
