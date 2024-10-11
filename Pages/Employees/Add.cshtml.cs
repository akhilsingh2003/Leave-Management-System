using Assingment.AppCode;
using Core;
using Core.BusinessObject;
using Datalayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceLayer;
using System.Buffers;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace Assingment.Pages.Employees
{
    [BindProperties]
    public class AddModel : BaseModel
    {
        private readonly IWebHostEnvironment _environment;

        public AddModel(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
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
        public IFormFile ProfileImage { get; set; }

        public List<string> DepartmentList { get; set; }
        public string Designation { get; set; }
        public List<string> DesignationList { get; set; }
        public List<string> StatusList { get; set; }


        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
    
    public void OnGet()
        {
            log.Debug("Calling the GetStatusList from the common function on Add page");
            StatusList = CommonFunction.GetStatusList();
            log.Debug("GetStatusList executed on Add Page");
            DepartmentManager departmnetManager = new DepartmentManager();
            DepartmentList = departmnetManager.GetDepartmentName();
            DesignationManager designationManager = new DesignationManager();
            DesignationList = designationManager.GetDesignationName();
        }

        public Microsoft.AspNetCore.Mvc.IActionResult OnPost()
        {
            DepartmentManager departmnetManager = new DepartmentManager();
            DesignationManager designationManager = new DesignationManager();


            Employee employee = new Employee();
            employee.Name = Name;
            employee.Email = Email;
            employee.Phone = Phone;
            employee.Status = Status;
            employee.DepartmentId = departmnetManager.GetDepartmentId(Department);
            employee.DesignationId = designationManager.GetDesignationId(Designation);
            employee.CreatedOnUTC = DateTime.UtcNow;
            employee.UpdatedOnUTC = DateTime.UtcNow;
            employee.CreatedBy = "System";
            employee.UpdatedBy = "System";
            employee.IPAddress = HttpContext.Connection.RemoteIpAddress.ToString();
            if (ProfileImage != null && ProfileImage.Length > 0)
            {
                // Generate a unique file name for the uploaded image
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(ProfileImage.FileName);

                // Set the ProfileImage  to the file name
                employee.ProfileImage = uniqueFileName; // Store only the file name in the database

                // Define the folder path where images will be saved
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");

                // Create the uploads folder if it doesn't exist
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Combine the uploads folder path with the unique file name
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Save the uploaded image to the specified file path
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ProfileImage.CopyTo(stream);
                }
            }



            EmployeeManager employeeManager = new EmployeeManager();
            log.Debug("Calling the Add function on Add page");
            OperationResult operationResult = employeeManager.Add(employee);

            if (operationResult.Status == (int)Core.OperationStatus.Success)
            {
                Success(operationResult.Message);
                log.Debug("Add function gets executed");
            }
            else
            {
                Warning(operationResult.Message);
                log.Debug("Add function gets failed to add employee");


            }
            return RedirectToPage("EmployeeList");
        }
    }
}
