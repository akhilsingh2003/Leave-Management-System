using Core;
using Core.BusinessObject;
using Datalayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer;
using IActionResult = Microsoft.AspNetCore.Mvc.IActionResult;

namespace Assingment.Pages.Departments
{
    public class DepartmentListModel : BaseModel
    {
        private const int PageSize = 5; // Number of items per page
        public List<Department> DepartmentList { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }


        public IActionResult OnGet(int? pageIndex)
        {
            DepartmentManager departmentManager = new DepartmentManager();
            log.Debug("Calling the GetAll from departmentList page");
            var allDepartment = departmentManager.GetAll();
            log.Debug("GetAll function gets executed on Department list page");
            // Calculate total pages
            TotalPages = (int)Math.Ceiling((double)departmentManager.CountAllDepartment() / PageSize);

            // Validate page index
            PageIndex = pageIndex ?? 1; // If pageIndex is null, default to page 1
            PageIndex = PageIndex < 1 ? 1 : PageIndex;
            PageIndex = PageIndex > TotalPages ? TotalPages : PageIndex;

            // Calculate the starting index of the current page
            int startIndex = (PageIndex - 1) * PageSize;

            // Select employees for the current page
            DepartmentList = allDepartment.Skip(startIndex).Take(PageSize).ToList();

            return Page();
        }

        public Microsoft.AspNetCore.Mvc.IActionResult OnGetDelete(int id)
        {
            DepartmentManager departmentManager = new DepartmentManager();
            log.Debug("Delete function calling on Department List");
            OperationResult operationResult = departmentManager.Delete(id);

            if (operationResult.Status == (int)Core.OperationStatus.Success)
            {
                Success(operationResult.Message);
                log.Debug("Delete function gets executed successfully for department");
            }
            else
            {
                Warning(operationResult.Message);
                log.Debug("Delete function gets failed  to delete the department");

                return Page();

            }
            return RedirectToPage("DepartmentList");

        }

    }
}
