using Core;
using Core.BusinessObject;
using Datalayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer;

namespace Assingment.Pages.Departments
{
    [BindProperties]
    public class DepartmentAddModel : BaseModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
      
        public void OnGet()
        {
        }
        public Microsoft.AspNetCore.Mvc.IActionResult OnPost()
        {
            Department department = new Department();
            department.Name = Name;
            department.Description = Description;
            department.CreatedOnUTC = DateTime.UtcNow;
            department.UpdatedOnUTC = DateTime.UtcNow;
            department.CreatedBy = "System";
            department.UpdatedBy = "System";
            department.IPAddress = HttpContext.Connection.RemoteIpAddress.ToString();

            DepartmentManager departmentManager = new DepartmentManager();
            log.Debug("Calling the Add function for Department");
            OperationResult operationResult = departmentManager.Add(department);

            if (operationResult.Status == (int)Core.OperationStatus.Success)
            {
                Success(operationResult.Message);
                log.Debug("Add function for Department gets executed successfully");
            }
            else
            {
                Warning(operationResult.Message);
                log.Debug("Add function for Department gets failed to add ");
                return Page();  

            }
            return RedirectToPage("DepartmentList");
        }
    }
}
