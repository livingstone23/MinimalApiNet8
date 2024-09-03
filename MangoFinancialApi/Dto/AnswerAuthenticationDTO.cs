


namespace MangoFinancialApi.Dto;



public class AnswerAuthenticationDTO
{
    public string Token { get; set; } = null!;
    public DateTime Expiration { get; set; }
}
