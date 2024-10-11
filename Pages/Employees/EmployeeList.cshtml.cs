using Core;
using Core.BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NLog;
using ServiceLayer;
using IActionResult = Microsoft.AspNetCore.Mvc.IActionResult;

namespace Assingment.Pages.Employees
{
    public class EmployeeListModel : BaseModel
    {
        private const int PageSize = 5; // Number of items per page

        public List<Employee> EmployeesList { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public IActionResult OnGet(int? pageIndex)
        {
            EmployeeManager employeeManager = new EmployeeManager();
            log.Debug("Calling the Employeelist on get method");
            var allEmployees = employeeManager.GetAll();
            log.Debug("List page on get method executed");
            // Calculate total pages
            log.Debug("Calling the CountAllEmployees on Get Method");
            TotalPages = (int)Math.Ceiling((double)employeeManager.CountAllEmployees() / PageSize);
            log.Debug(" CountAllEmployees on Get Method executed");

            // Validate page index
            PageIndex = pageIndex ?? 1; // If pageIndex is null, default to page 1
            PageIndex = PageIndex < 1 ? 1 : PageIndex;
            PageIndex = PageIndex > TotalPages ? TotalPages : PageIndex;

            // Calculate the starting index of the current page
            int startIndex = (PageIndex - 1) * PageSize;

            // Select employees for the current page
            EmployeesList = allEmployees.Skip(startIndex).Take(PageSize).ToList();

            return Page();
        }

        public IActionResult OnGetDelete(int id)
        {
            EmployeeManager employeeManager = new EmployeeManager();
            log.Debug("Calling the Delete method in employeelist");
            OperationResult operationResult = employeeManager.Delete(id);

            if (operationResult.Status == (int)Core.OperationStatus.Success)
            {
                Success(operationResult.Message);
                log.Debug("Delete method gets executed for employee");
            }
            else
            {
                Warning(operationResult.Message);
                log.Debug("Delete method gets failed to execute for employee");
                return Page();

            }
            return RedirectToPage("EmployeeList");

        }
    }
}
