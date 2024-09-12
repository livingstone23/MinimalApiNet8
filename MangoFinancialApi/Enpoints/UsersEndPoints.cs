using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.RegularExpressions;
using MangoFinancialApi.Dto;
using MangoFinancialApi.Filters;
using MangoFinancialApi.Services;
using MangoFinancialApi.Utility;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;



namespace MangoFinancialApi.Enpoints;



public static class UsersEndPoints
{

    public static RouteGroupBuilder MapUsers(this RouteGroupBuilder endpoints)
    {
        
        endpoints.MapPost("/register", RegisterUser).AddEndpointFilter<FilterValidation<CredentialUserDTO>>();
        endpoints.MapPost("/login", Login).AddEndpointFilter<FilterValidation<CredentialUserDTO>>();

        endpoints.MapPost("/madeadmin", MadeAdmin)
            .AddEndpointFilter<FilterValidation<EditClaimDTO>>()
            .RequireAuthorization("isadmin");       //Enable the security in endpoints #SEC2

        endpoints.MapPost("/removeadmin", RemoveAdmin)
            .AddEndpointFilter<FilterValidation<EditClaimDTO>>()
            .RequireAuthorization("isadmin");       //Enable the security in endpoints #SEC2    
        
        endpoints.MapPost("/renewtoken", RenewToken).RequireAuthorization(); //Enable the security in endpoints #SEC2

        return endpoints;
    }


    static async Task<Results<Ok<AnswerAuthenticationDTO>, BadRequest<IEnumerable<IdentityError>>>> RegisterUser (CredentialUserDTO credentialUser, [FromServices] UserManager<IdentityUser> userManager, IConfiguration configuration) 
    {

       var user = new IdentityUser 
        { 
            UserName = credentialUser.Email, 
            Email = credentialUser.Email 
        };

        var result = await userManager.CreateAsync(user, credentialUser.Password);

        if (result.Succeeded)
        {

            var credentialAnswer = await BuildToken(credentialUser, configuration, userManager);

            return TypedResults.Ok(credentialAnswer);
           
        }
        else
        {
            return TypedResults.BadRequest(result.Errors);
        }


    }

    static async Task<Results<Ok<AnswerAuthenticationDTO>, BadRequest<string>>> Login (CredentialUserDTO credentialUser, 
                                                                                        [FromServices] SignInManager<IdentityUser> signInManager, 
                                                                                        [FromServices] UserManager<IdentityUser> userManager,
                                                                                        IConfiguration configuration
                                                                                        ) 
    {

        var user = await userManager.FindByEmailAsync(credentialUser.Email);    

        if (user is null)
        {
            return TypedResults.BadRequest("Login failed");
        }

        var result = await signInManager.CheckPasswordSignInAsync(user, credentialUser.Password, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            var credentialAnswer = await BuildToken(credentialUser, configuration, userManager);

            return TypedResults.Ok(credentialAnswer);
        }
        else
        {
            return TypedResults.BadRequest("Login failed");
        }
    }

    static async Task<Results<NoContent, NotFound>> MadeAdmin(  EditClaimDTO editClaimDTO, 
                                                                [FromServices] UserManager<IdentityUser> userManager)
    {

        var user = await userManager.FindByEmailAsync(editClaimDTO.Email);

        if (user is null)
        {
            return TypedResults.NotFound();
        }

        var result = await userManager.AddClaimAsync(user, new Claim("isadmin", "true"));

        return TypedResults.NoContent();
       
    } 

    static async Task<Results<NoContent, NotFound>> RemoveAdmin(  EditClaimDTO editClaimDTO, 
                                                                [FromServices] UserManager<IdentityUser> userManager)
    {

        var user = await userManager.FindByEmailAsync(editClaimDTO.Email);

        if (user is null)
        {
            return TypedResults.NotFound();
        }

        var result = await userManager.RemoveClaimAsync(user, new Claim("isadmin", "true"));

        return TypedResults.NoContent();
       
    } 


    private  async static Task<AnswerAuthenticationDTO> BuildToken(CredentialUserDTO credentialuser, 
                                                            IConfiguration configuration, 
                                                            UserManager<IdentityUser> userManager)
    {
        try
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, credentialuser.Email),
                new Claim("another value", "another value")
            };

            var user = await userManager.FindByEmailAsync(credentialuser.Email);
            var claimsDB = await userManager.GetClaimsAsync(user!);

            claims.AddRange(claimsDB);

            var Key = Keys.GetKey(configuration);
            var creds = new SigningCredentials(Key.First(), SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddYears(1);

            //Build the token 
            var tokenSecurity = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );

            var token = new JwtSecurityTokenHandler().WriteToken(tokenSecurity);

            return new AnswerAuthenticationDTO
            {
                Token = token,
                Expiration = expiration
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }

    //Method For Renew Token
    public async static Task<Results<Ok<AnswerAuthenticationDTO>, NotFound>> RenewToken(IUserServices userServices,
                                                                                        IConfiguration configuration,
                                                                                        [FromServices] UserManager<IdentityUser> userManager
                                                                                        )
    {

        var user = await userServices.GetUser();

        if(user is null)
        {
            return TypedResults.NotFound();
        } 
    
        var credentialUserDTO = new CredentialUserDTO { Email = user.Email! };

        var AnswerAuthenticationDTO = await BuildToken(credentialUserDTO, configuration, userManager);

        return TypedResults.Ok(AnswerAuthenticationDTO);

    }

}