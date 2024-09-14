using Microsoft.IdentityModel.Tokens;



namespace MangoFinancialApi.Utility;


/// <summary>
/// Extension for HttpContext for using in the pagination to save code.
/// </summary>
public static class HttpContextExtensionsUtility
{

    public static T GetValueByDefault<T>(this HttpContext context, 
                                        string nameOfField, 
                                        T valueByDefect) where T: IParsable<T>
    {
        
        var value = context.Request.Query[nameOfField];

        if(value.IsNullOrEmpty())
        {
            return valueByDefect;
        }

        return T.Parse(value!, null);

    }

}
