using MangoFinancialApi.Utility;

namespace MangoFinancialApi.Dto;



public class MovieFilterDTO
{

    public int Page { get; set; }
    public int RecordsPerPage { get; set; }
    public PaginationDTO Pagination 
    { 
        get
        {
            return new PaginationDTO() 
            { 
                Page = Page, 
                RecordsPerPage = RecordsPerPage 
            };
        }
    }

    public string? Title { get; set; }
    public int GenderId { get; set; }

    public bool InTheaters { get; set; }
    public bool UpcomingReleases { get; set; }
    public string? FieldOrder { get; set; }
    public bool OrderAscending { get; set; } = true;


    public static ValueTask<MovieFilterDTO> BindAsync(HttpContext context)
    {
        
        var page = context.GetValueByDefault(nameof(Page),1);
        var recordsPerPage = context.GetValueByDefault(nameof(RecordsPerPage),10);
        var genderId = context.GetValueByDefault(nameof(GenderId),0);
        var title = context.GetValueByDefault(nameof(Title),string.Empty);
        var inTheaters = context.GetValueByDefault(nameof(InTheaters),false);
        var upcomingReleases = context.GetValueByDefault(nameof(UpcomingReleases),false);
        var fieldOrder = context.GetValueByDefault(nameof(FieldOrder),string.Empty);
        var orderAscending = context.GetValueByDefault(nameof(OrderAscending),true);

        var result = new MovieFilterDTO
        {
            Page = page,
            RecordsPerPage = recordsPerPage,
            GenderId = genderId,
            Title = title,
            InTheaters = inTheaters,
            UpcomingReleases = upcomingReleases,
            FieldOrder = fieldOrder,
            OrderAscending = orderAscending
        };

        return ValueTask.FromResult(result);

    }

}