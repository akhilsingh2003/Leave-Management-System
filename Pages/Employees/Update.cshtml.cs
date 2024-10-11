using Assingment.AppCode;
using Core;
using Core.BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer;
using System.ComponentModel.DataAnnotations;

namespace Assingment.Pages.Employees
{
    [BindProperties]
    public class UpdateModel : BaseModel
    {
        
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone is required.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public string Status { get; set; }
        public string Department { get; set; }

        [Display(Name = "Profile Image")]
        public string ProfileImage {  get; set; }
        public List<string> DepartmentList { get; set; }
        public List<string> StatusList { get; set; }
        public string Designation { get; set; }
        public List<string> DesignationList { get; set; }

        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        public int DesignationId { get; set; }
        public void OnGet(int id)
        {
            StatusList = CommonFunction.GetStatusList();
            DepartmentManager departmnetManager = new DepartmentManager();
            DepartmentList = departmnetManager.GetDepartmentName();
            DesignationManager designationManager = new DesignationManager();
            DesignationList = designationManager.GetDesignationName();
            EmployeeManager employeeManager = new EmployeeManager();
            Employee employee = employeeManager.GetById(id);
            Name = employee.Name;
            Email = employee.Email;
            Phone = employee.Phone;
            DepartmentId = employee.DepartmentId;
            DesignationId = employee.DesignationId;
            Status = employee.Status;
            ProfileImage =employee.ProfileImage;
            Id = employee.ID;
        }
        public Microsoft.AspNetCore.Mvc.IActionResult OnPost()
        {
            DepartmentManager departmnetManager = new DepartmentManager();
            DesignationManager designationManager = new DesignationManager();

            Employee employee = new Employee();
            employee.ID = Id;
            employee.Name = Name;
            employee.Email = Email;
            employee.Phone = Phone;
            employee.Status = Status;
            employee.DepartmentName = departmnetManager.GetNameByID(DepartmentId);
            employee.DepartmentId = departmnetManager.GetDepartmentId(Department);
            employee.DesignationTitle = designationManager.GetNameByID(DesignationId);
            employee.DesignationId = designationManager.GetDesignationId(Designation);
            employee.CreatedBy = "System";
            employee.CreatedOnUTC = DateTime.UtcNow;
            employee.UpdatedBy = "System";
            employee.UpdatedOnUTC = DateTime.UtcNow;
            employee.IPAddress = HttpContext.Connection.RemoteIpAddress.ToString();

           

            EmployeeManager employeeManager = new EmployeeManager();
            log.Debug("Calling Update method from the employeeList");
            OperationResult operationResult = employeeManager.Update(employee);

            if (operationResult.Status == (int)Core.OperationStatus.Success)
            {
                Success(operationResult.Message);
                log.Debug("Update method get executed successfully for an employee");
            }
            else
            {
                Warning(operationResult.Message);
                log.Debug("Update method gets failed to executed successfully for an employee");

                return Page();

            };
            return RedirectToPage("EmployeeList");
        }


    }
}
