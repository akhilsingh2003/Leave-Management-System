using Core;
using Core.BusinessObject;
using Datalayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NLog.Fluent;
using ServiceLayer;

namespace Assingment.Pages.Designations
{
    public class ListModel : BaseModel
    {
        private const int PageSize = 5; // Number of items per page
        public List<Designation> DesignationList { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }


        public IActionResult OnGet(int? pageIndex)
        {
            DesignationManager designationManager = new DesignationManager();
            log.Debug("Calling the GetAll from designationList page");
            var allDesignation = designationManager.GetAll();
            log.Debug("GetAll function gets executed on Designation list page");
            // Calculate total pages
            TotalPages = (int)Math.Ceiling((double)designationManager.CountAllDesignation() / PageSize);

            // Validate page index
            PageIndex = pageIndex ?? 1; // If pageIndex is null, default to page 1
            PageIndex = PageIndex < 1 ? 1 : PageIndex;
            PageIndex = PageIndex > TotalPages ? TotalPages : PageIndex;

            // Calculate the starting index of the current page
            int startIndex = (PageIndex - 1) * PageSize;

            DesignationList = allDesignation.Skip(startIndex).Take(PageSize).ToList();

            return Page();
        }
        public Microsoft.AspNetCore.Mvc.IActionResult OnGetDelete(int id)
        {
            DesignationManager designationManager = new DesignationManager();
            log.Debug("Delete function calling on Designation List");
            OperationResult operationResult = designationManager.Delete(id);

            if (operationResult.Status == (int)Core.OperationStatus.Success)
            {
                Success(operationResult.Message);
                log.Debug("Delete function gets executed successfully for designation");
            }
            else
            {
                Warning(operationResult.Message);
                log.Debug("Delete function gets failed  to delete the designation");

                return Page();

            }
            return RedirectToPage("List");

        }

    }
}
