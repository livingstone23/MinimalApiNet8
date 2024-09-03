using Microsoft.IdentityModel.Tokens;



namespace MangoFinancialApi.Utility;



/// <summary>
/// Auxiliar class to get the keys of the secret.json
/// </summary>
public static class Keys
{

    public const string IssuerOwn = "own-app";
    private const string SectionKeys = "Authentication:Schemes:Bearer:SigninKeys";
    private const string SectionKey_Emisor = "Issuer";
    private const string SectionKey_Value = "Value";


    //Auxiliar Method to get the key of the secret.json of our app
    public static IEnumerable<SecurityKey> GetKey(IConfiguration configuration) => GetKey(configuration, IssuerOwn);


    //Method permit get the key of the secret.json
    public static IEnumerable<SecurityKey> GetKey(IConfiguration configuration, string issuer)
    {
        //apply a filter to get the key of the issuer
        var signinKey = configuration.GetSection(SectionKeys)
            .GetChildren()
            .SingleOrDefault(key => key[SectionKey_Emisor] == issuer);

        if (signinKey is not null && signinKey[SectionKey_Value] is string valueKey)
        {
            //Return the different keys that the app use
            yield return new SymmetricSecurityKey(Convert.FromBase64String(valueKey));
        }
    }


    public static IEnumerable<SecurityKey> GetAllKeys(IConfiguration configuration)
    {
        //Get all keys
        var signinKeys = configuration.GetSection(SectionKeys)
            .GetChildren();

        foreach (var signinKey in signinKeys)
        {
            if (signinKey[SectionKey_Value] is string valueKey)
            {
                //Return the different keys that the app use
                yield return new SymmetricSecurityKey(Convert.FromBase64String(valueKey));
            }
        }
    }
}