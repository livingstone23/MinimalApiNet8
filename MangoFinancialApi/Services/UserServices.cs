using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace MangoFinancialApi.Services;



public class UserServices: IUserServices
{

    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly UserManager<IdentityUser> _userManager;

    public UserServices(IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager)
    {
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }


    /// <summary>
    /// Get the email of the user from the claims
    /// </summary>
    /// <returns></returns>
    public async Task<IdentityUser?> GetUser()
    {
        var emailClaim = _httpContextAccessor.HttpContext?.User.Claims.Where(c => c.Type == ClaimTypes.Email).FirstOrDefault();

        if (emailClaim == null)
        {
            return null;
        }

        var email = emailClaim.Value;
        return await _userManager.FindByEmailAsync(email);
    }

    

   
}
