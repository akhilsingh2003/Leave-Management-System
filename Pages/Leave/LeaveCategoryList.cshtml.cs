using Core;
using Core.BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NLog.Fluent;
using ServiceLayer;

namespace Assingment.Pages.Leave
{
    public class LeaveCategoryListcshtmlModel : BaseModel
    {
        private const int PageSize = 5; // Number of items per page
        public List<LeaveCategory> LeaveCategoryList { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }


        public IActionResult OnGet(int? pageIndex)
        {
            LeaveCategoryManager leaveCategoryManager = new LeaveCategoryManager();
            log.Debug("Calling the GetAll from LeaveCategoryList page");
            var AllLeaveCategory = leaveCategoryManager.GetAll();
            log.Debug("GetAll function gets executed on Department list page");
            // Calculate total pages
            TotalPages = (int)Math.Ceiling((double)leaveCategoryManager.CountAllDepartment() / PageSize);

            // Validate page index
            PageIndex = pageIndex ?? 1; // If pageIndex is null, default to page 1
            PageIndex = PageIndex < 1 ? 1 : PageIndex;
            PageIndex = PageIndex > TotalPages ? TotalPages : PageIndex;

            // Calculate the starting index of the current page
            int StartIndex = (PageIndex - 1) * PageSize;

            // Select employees for the current page
           LeaveCategoryList = AllLeaveCategory.Skip(StartIndex).Take(PageSize).ToList();

            return Page();
        }
        public Microsoft.AspNetCore.Mvc.IActionResult OnGetDelete(int id)
        {
           LeaveCategoryManager leaveCategoryManager = new LeaveCategoryManager();
            log.Debug("Delete function calling on leaveCategory List");
            OperationResult operationResult = leaveCategoryManager.Delete(id);

            if (operationResult.Status == (int)Core.OperationStatus.Success)
            {
                Success(operationResult.Message);
                log.Debug("Delete function gets executed successfully for leaveCategory");
            }
            else
            {
                Warning(operationResult.Message);
                log.Debug("Delete function gets failed  to delete the leaveCategory");

                return Page();

            }
            return RedirectToPage("LeaveCategoryList");

        }
    }
}
