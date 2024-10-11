using Core;
using Core.BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NLog.Fluent;
using ServiceLayer;

namespace Assingment.Pages.OnDuty
{
    public class ListModel : BaseModel
    {
        private const int PageSize = 5; // Number of items per page

        public List<Core.BusinessObject.OnDuty> OnDutyList { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public IActionResult OnGet(int? pageIndex)
        {
            OnDutyManager ondutyManager = new OnDutyManager();
            log.Debug("Calling the Ondutylist on get method");
            var AllOndutyRequest = ondutyManager.GetAll();
            log.Debug("List page on get method executed");
            // Calculate total pages
            log.Debug("Calling the CountAllOnDuty REQUEST on Get Method");
            TotalPages = (int)Math.Ceiling((double)ondutyManager.CountAllOnduty() / PageSize);
            log.Debug(" CountAllOnduty on Get Method executed");

            // Validate page index
            PageIndex = pageIndex ?? 1; // If pageIndex is null, default to page 1
            PageIndex = PageIndex < 1 ? 1 : PageIndex;
            PageIndex = PageIndex > TotalPages ? TotalPages : PageIndex;

            // Calculate the starting index of the current page
            int startIndex = (PageIndex - 1) * PageSize;

            // Select employees for the current page
            OnDutyList = AllOndutyRequest.Skip(startIndex).Take(PageSize).ToList();

            return Page();
        }
        public Microsoft.AspNetCore.Mvc.IActionResult OnGetDelete(int id)
        {
            OnDutyManager ondutyManager = new OnDutyManager();
            log.Debug("Delete function calling on Onduty List");
            OperationResult operationResult = ondutyManager.Delete(id);

            if (operationResult.Status == (int)Core.OperationStatus.Success)
            {
                Success(operationResult.Message);
                log.Debug("Delete function gets executed successfully for Onduty");
            }
            else
            {
                Warning(operationResult.Message);
                log.Debug("Delete function gets failed  to delete the Onduty");

                return Page();

            }
            return RedirectToPage("List");

        }
    }
}
