using AutoMapper;
using HotChocolate.Authorization;
using MangoDomain.EntititesTest;
using MangoFinancialApi.Dto;
using MangoFinancialApi.Repository;

namespace MangoFinancialApi.Graphql;

public class Mutation
{


    [Serial]
    public async Task<GenderDto> CreateGender([Service] IRepositoryGender repository, 
                                                [Service] IMapper mapper,
                                                CreateGenderDto createGenderDto)
    {
        var gender = mapper.Map<Gender>(createGenderDto);

        await repository.Create(gender);

        var genderDto = mapper.Map<GenderDto>(gender);

        return genderDto;

    }



    [Serial]
    [Authorize(Policy = "isadmin")]
    public async Task<GenderDto?> UpdateGender([Service] IRepositoryGender repository, 
                                                [Service] IMapper mapper,
                                                UpdateGenderDTO updateGenderDTO)
    {
        var genderExist = await repository.Exist(updateGenderDTO.Id);

        if (!genderExist)
        {
            return null;
        }


        var gender = mapper.Map<Gender>(updateGenderDTO);

        await repository.Update(gender);

        var genderDto = mapper.Map<GenderDto>(gender);

        return genderDto;
       

    }

    [Serial]
    public async Task<bool> DeleteGender([Service] IRepositoryGender repository, 
                                                int id)
    {
        var genderExist = await repository.Exist(id);

        if (!genderExist)
        {
            return false;
        }

        await repository.Delete(id);

        return true;

    }

}
