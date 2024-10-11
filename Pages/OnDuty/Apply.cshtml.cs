using Core.BusinessObject;
using Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceLayer;
using Assingment.AppCode;

namespace Assingment.Pages.OnDuty
{
    [BindProperties]
    public class ApplyModel : BaseModel
    {
        private readonly IWebHostEnvironment _environment;

        public ApplyModel(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public int Id { get; set; }
        public string Reason { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Duration { get; set; }
        public string Status {  get; set; }
        public IFormFile Document { get; set; }
        public List<string> StatusList { get; set; }


        public void OnGet()
        {
            log.Debug("Calling the GetStatusList from the common function on Add page");
            StatusList = CommonFunction.GetActionList();
            log.Debug("GetStatusList executed on Add Page");

        }

        public Microsoft.AspNetCore.Mvc.IActionResult OnPost()
        {

            Core.BusinessObject.OnDuty onduty = new Core.BusinessObject.OnDuty();
            onduty.StartDate = StartDate;
            onduty.EndDate = EndDate;
            onduty.Reason = Reason;
            onduty.Duration = Duration;
            onduty.Status = Status;
            onduty.CreatedOnUTC = DateTime.UtcNow;
            onduty.UpdatedOnUTC = DateTime.UtcNow;
            onduty.CreatedBy = "System";
            onduty.UpdatedBy = "System";
            onduty.IPAddress = HttpContext.Connection.RemoteIpAddress.ToString();

            if (Document != null && Document.Length > 0)
            {
                // Generate a unique file name for the uploaded file
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(Document.FileName);

                // Set the Document  to the file name
                onduty.Document = uniqueFileName; // Store only the file name in the database

                // Define the folder path where images will be saved
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "OnDutyuploads");

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




            OnDutyManager ondutyManager = new OnDutyManager();
            log.Debug("Calling the Add function on Apply Onduty page");
            OperationResult operationResult = ondutyManager.Add(onduty);

            if (operationResult.Status == (int)Core.OperationStatus.Success)
            {
                Success(operationResult.Message);
                log.Debug("Apply function gets executed");
            }
            else
            {
                Warning(operationResult.Message);
                log.Debug("Apply function gets failed to add Onduty request");


            }
            return RedirectToPage("List");
        }
    }
}
