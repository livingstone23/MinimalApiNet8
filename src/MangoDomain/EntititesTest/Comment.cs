using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace MangoDomain.EntititesTest;



public class Comment
{

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public string Content { get; set; } = null!;
    
    public int MovieId { get; set; }
    
}
