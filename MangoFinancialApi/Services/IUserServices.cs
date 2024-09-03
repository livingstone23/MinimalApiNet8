using Microsoft.AspNetCore.Identity;

namespace MangoFinancialApi.Services;

public interface IUserServices
{
    Task<IdentityUser?> GetUser();

}
