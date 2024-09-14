using MangoFinancialApi.Utility;
using Microsoft.IdentityModel.Tokens;

namespace MangoFinancialApi.Dto;



/// <summary>
/// DTO for pagination
/// </summary>
public class PaginationDTO
{

    //Field using BidnAsing
    private const int pageInitialValue = 1;
    private const int recordsPerPageInitialValue = 10;

    public int Page { get; set; } = pageInitialValue;

    private int recordsPerPage = recordsPerPageInitialValue;

    private readonly int maxRecordsPerPage = 50;

    public int RecordsPerPage
    {
        get => recordsPerPage;
        set => recordsPerPage = (value > maxRecordsPerPage) ? maxRecordsPerPage : value;
    }


    //Method for using BindAsing
    public static ValueTask<PaginationDTO> BindAsync2(HttpContext context)
    {
        
        var page = context.Request.Query[nameof(Page)];
        var recordsPerPage = context.Request.Query[nameof(RecordsPerPage)];

        var pageInt = page.IsNullOrEmpty() ? pageInitialValue : int.Parse(page.ToString());
        var recordsPerPageInt = recordsPerPage.IsNullOrEmpty() ? recordsPerPageInitialValue : int.Parse(recordsPerPage.ToString());

        var result = new PaginationDTO { Page = pageInt, RecordsPerPage = recordsPerPageInt };

        return new ValueTask<PaginationDTO>(result);
    }

    //Reducing the code using generic
    public static ValueTask<PaginationDTO> BindAsync(HttpContext context)
    {
        
        var page = context.GetValueByDefault(nameof(Page), pageInitialValue);
        var recordsPerPage = context.GetValueByDefault(nameof(RecordsPerPage), recordsPerPageInitialValue);

        var result = new PaginationDTO { Page = page, RecordsPerPage = recordsPerPage };

        return new ValueTask<PaginationDTO>(result);
    }

}
