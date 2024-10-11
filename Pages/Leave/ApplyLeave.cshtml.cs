using Core.BusinessObject;
using Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Org.BouncyCastle.Asn1.X509;
using ServiceLayer;
using System.Xml.Linq;
using Datalayer;
using Assingment.AppCode;

namespace Assingment.Pages.Leave
{
    [BindProperties]
    public class ApplyLeaveModel : BaseModel
    {
        private readonly IWebHostEnvironment _environment;

        public ApplyLeaveModel(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public int Id { get; set; }
        public string Reason {  get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Duration {  get; set; }
        public IFormFile Document {  get; set; }
        public string LeaveCategory { get; set; }
        public List<string> LeaveCategoryList { get; set; }
        public string Status {  get; set; }
        public List<string> StatusList { get; set; }

        public int LeaveCategoryId {  get; set; }
        public void OnGet()
        {
            LeaveCategoryManager leaveCategoryManager = new LeaveCategoryManager();
            LeaveCategoryList = leaveCategoryManager.GetCategoryName();
            log.Debug("Calling the GetActionList from the common function on Add page");
            StatusList = CommonFunction.GetActionList();
            log.Debug("GetActionList executed on Add Page");

        }

        public Microsoft.AspNetCore.Mvc.IActionResult OnPost()
        {
            LeaveCategoryManager leaveCategoryManager = new LeaveCategoryManager();

            EmployeeLeave employeeleave = new EmployeeLeave();
            employeeleave.StartDate = StartDate;
            employeeleave.EndDate = EndDate;
            employeeleave.Reason = Reason;
            employeeleave.Duration = Duration;
            employeeleave.Status = Status;
            employeeleave.LeaveCategoryId = leaveCategoryManager.GetCategoryId(LeaveCategory);
            employeeleave.CreatedOnUTC = DateTime.UtcNow;
            employeeleave.UpdatedOnUTC = DateTime.UtcNow;
            employeeleave.CreatedBy = "System";
            employeeleave.UpdatedBy = "System";
            employeeleave.IPAddress = HttpContext.Connection.RemoteIpAddress.ToString();

            if (Document != null && Document.Length > 0)
            {
                // Generate a unique file name for the uploaded file
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(Document.FileName);

                // Set the Document  to the file name
                employeeleave.Document = uniqueFileName; // Store only the file name in the database

                // Define the folder path where images will be saved
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "fileuploads");

                // Create the uploads folder if it doesn't exist
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Combine the uploads folder path with the unique file name
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Save the uploaded file  to the specified file path
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    Document.CopyTo(stream);
                }
            }




            EmployeeLeaveManager employeeleaveManager = new EmployeeLeaveManager();
            log.Debug("Calling the Add function on Add page");
            OperationResult operationResult = employeeleaveManager.Add(employeeleave);

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
            return RedirectToPage("AppliedList");
        }



    }
}
