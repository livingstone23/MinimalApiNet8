using AutoMapper;
using MangoFinancialApi.Repository;



namespace MangoFinancialApi.Dto;



/// <summary>
/// Class for Using the AsParameter 
/// </summary>
public class GetGenderByIdDTO
{

    public int Id { get; set; }

    public IRepositoryGender Repository { get; set; } = null!;

    public IMapper Mapper { get; set; } = null!;

}   