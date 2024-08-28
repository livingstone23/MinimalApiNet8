using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace MangoDomain.EntititesTest;



public class Gender
{
    
    //El atributo Key indica que es la llave primaria, y debe ser autoincrementable de 1 a 1
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    public List<GenderMovie> GenderMovies { get; set; } = new List<GenderMovie>();


}
