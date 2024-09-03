namespace MangoFinancialApi.Validations;

public static class Utility
{
    public static string FieldRequiredMessage = "The property {PropertyName} is required";

    public static string MaxLenghtMessage = "The property {PropertyName} must have a maximum of {maxLenght} characters";

    public static string FirstLetterUpperCaseMessage = "The property {PropertyName} must have the first letter in uppercase";

    public static string EmailMessage = "The property {PropertyName} must be a valid email address";

    public static string GreaterThanOrEqualToMessage(DateTime minDate)
    {
        return  "The property {PropertyName} must be greater than or equal to " + minDate.ToString("yyyy-MM-dd");
    } 
    //Method of validation 
    public static bool FirstLetterUpperCase(string name)
    {
        if(string.IsNullOrWhiteSpace(name))
        {
            return true;
        }

        char firstLetter = name[0];

        return char.IsUpper(firstLetter);
    }


}
