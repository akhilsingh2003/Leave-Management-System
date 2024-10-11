using Core.BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer;
using IActionResult = Microsoft.AspNetCore.Mvc.IActionResult;

namespace Assingment.Pages.Employees
{
    public class OverallDataModel : BaseModel
    {
        private const int PageSize = 5; // Number of items per page
        public List<Employee> OverallList { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }


        public IActionResult OnGet(int? pageIndex)
        {
            EmployeeManager employeeManager = new EmployeeManager();
            log.Debug("Calling the GetAll method on Overalldata Page");
            var allEmployees = employeeManager.GetAll();
            log.Debug("GetAll method gets executed on OverallData page");

            // Calculate total pages
            TotalPages = (int)Math.Ceiling((double)employeeManager.CountAllEmployees() / PageSize);

            // Validate page index
            PageIndex = pageIndex ?? 1; // If pageIndex is null, default to page 1
            PageIndex = PageIndex < 1 ? 1 : PageIndex;
            PageIndex = PageIndex > TotalPages ? TotalPages : PageIndex;

            // Calculate the starting index of the current page
            int startIndex = (PageIndex - 1) * PageSize;

            // Select employees for the current page
            OverallList = allEmployees.Skip(startIndex).Take(PageSize).ToList();

            return Page();
        }
    }
}
