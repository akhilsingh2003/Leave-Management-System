using Core;
using Core.BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NLog.Fluent;
using ServiceLayer;

namespace Assingment.Pages.Leave
{
    public class AppliedListModel : BaseModel
    {
        private const int PageSize = 5; // Number of items per page

        public List<EmployeeLeave> EmployeeLeaveList { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public IActionResult OnGet(int? pageIndex)
        {
            EmployeeLeaveManager employeeleaveManager = new EmployeeLeaveManager();
            log.Debug("Calling the EmployeeLeavelist on get method");
            var allLeaves = employeeleaveManager.GetAll();
            log.Debug("List page on get method executed");
            // Calculate total pages
            log.Debug("Calling the CountAllEmployees on Get Method");
            TotalPages = (int)Math.Ceiling((double)employeeleaveManager.CountAllAppliedLeave() / PageSize);
            log.Debug(" CountAllEmployees on Get Method executed");

            // Validate page index
            PageIndex = pageIndex ?? 1; // If pageIndex is null, default to page 1
            PageIndex = PageIndex < 1 ? 1 : PageIndex;
            PageIndex = PageIndex > TotalPages ? TotalPages : PageIndex;

            // Calculate the starting index of the current page
            int startIndex = (PageIndex - 1) * PageSize;

            // Select employees for the current page
            EmployeeLeaveList = allLeaves.Skip(startIndex).Take(PageSize).ToList();

            return Page();
        }
        public Microsoft.AspNetCore.Mvc.IActionResult OnGetDelete(int id)
        {
            EmployeeLeaveManager employeeleaveManager = new EmployeeLeaveManager();
            log.Debug("Delete function calling on applied leave List");
            OperationResult operationResult = employeeleaveManager.Delete(id);

            if (operationResult.Status == (int)Core.OperationStatus.Success)
            {
                Success(operationResult.Message);
                log.Debug("Delete function gets executed successfully for leave");
            }
            else
            {
                Warning(operationResult.Message);
                log.Debug("Delete function gets failed  to delete the leave");

                return Page();

            }
            return RedirectToPage("AppliedList");

        }
    }
}
