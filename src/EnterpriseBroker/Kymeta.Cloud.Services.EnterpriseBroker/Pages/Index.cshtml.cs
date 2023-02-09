using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics.CodeAnalysis;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Pages
{
    [Authorize]
    [ExcludeFromCodeCoverage]
    public class IndexModel : PageModel
    {
        private IActionsRepository _actionsRepository;
        public IEnumerable<SalesforceActionTransaction> ActionRecords = new List<SalesforceActionTransaction>();
        public int PageIndex;
        public int PageSize;
        public string TimePreference = "utc";

        public bool HasNextPage => PageIndex < (ActionRecords.Count() / PageSize);
        public bool HasPreviousPage => PageIndex > 1;
        public int LastPage => (ActionRecords.Count() / PageSize) + 1;
        public IEnumerable<SalesforceActionTransaction> PagedActionRecords
        {
            get
            {
                if (TimePreference == "local")
                {
                    foreach (var ar in ActionRecords)
                    {
                        ar.CreatedOn = ar.CreatedOn.ToLocalTime();
                        if (ar.LastUpdatedOn.HasValue) ar.LastUpdatedOn = ar.LastUpdatedOn.Value.ToLocalTime();
                    }
                }
                return ActionRecords.Skip((PageIndex - 1) * PageSize).Take(PageSize);
            }
        }

        public IndexModel(IActionsRepository actionsRepository)
        {
            _actionsRepository = actionsRepository;
        }

        public async Task OnGet([FromQuery]int page = 1, [FromQuery]int pageSize = 20)
        {
            if (page < 1) page = 1;

            PageIndex = page;
            PageSize = pageSize;
            TimePreference = HttpContext.Request.Cookies["timePreference"] ?? "utc";
            
            ActionRecords = await _actionsRepository.GetActionRecords();
        }
    }
}
