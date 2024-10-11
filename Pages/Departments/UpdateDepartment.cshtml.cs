using Core;
using Core.BusinessObject;
using Datalayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer;

namespace Assingment.Pages.Departments
{
    [BindProperties]
    public class UpdateDepartmentModel : BaseModel
    {

        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public void OnGet(int id)
        {
            DepartmentManager departmentManager = new DepartmentManager();
            Department department = departmentManager.GetById(id);
            Name = department.Name;
            Description = department.Description;
            ID = department.ID;
            

        }
        public Microsoft.AspNetCore.Mvc.IActionResult OnPost()
        {
            DepartmentManager departmentManager = new DepartmentManager();
            Department department = new Department();
            department.ID = ID;
            department.Name = Name;
            department.Description = Description;
            department.UpdatedBy = "System";
            department.UpdatedOnUTC = DateTime.UtcNow;
            department.IPAddress = HttpContext.Connection.RemoteIpAddress.ToString();

            log.Debug("Calling Update method from the DepartmentList");
            OperationResult operationResult = departmentManager.Update(department);


            if (operationResult.Status == (int)Core.OperationStatus.Success)
            {
                Success(operationResult.Message);
                log.Debug("Update method get executed successfully for an department");
            }
            else
            {
                Warning(operationResult.Message);
                log.Debug("Update method gets failed to executed successfully for an department");
                return Page();

            };
            return RedirectToPage("DepartmentList");
        }
    }
}
