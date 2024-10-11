using Core.BusinessObject;
using Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NLog.Fluent;
using ServiceLayer;
using Assingment.AppCode;

namespace Assingment.Pages.WFH
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
        public IFormFile Document { get; set; }
        public string Status {  get; set; }
        public List<string> StatusList { get; set; }


        public void OnGet()
        {
            log.Debug("Calling the GetActionList from the common function on Add page");
            StatusList = CommonFunction.GetActionList();
            log.Debug("GetActionList executed on Add Page");

        }

        public Microsoft.AspNetCore.Mvc.IActionResult OnPost()
        {

            Wfh wfh = new Wfh();
            wfh.StartDate = StartDate;
            wfh.EndDate = EndDate;
            wfh.Reason = Reason;
            wfh.Duration = Duration;
            wfh.Status = Status;
            wfh.CreatedOnUTC = DateTime.UtcNow;
            wfh.UpdatedOnUTC = DateTime.UtcNow;
            wfh.CreatedBy = "System";
            wfh.UpdatedBy = "System";
            wfh.IPAddress = HttpContext.Connection.RemoteIpAddress.ToString();

            if (Document != null && Document.Length > 0)
            {
                // Generate a unique file name for the uploaded file
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(Document.FileName);

                // Set the Document  to the file name
                wfh.Document = uniqueFileName; // Store only the file name in the database

                // Define the folder path where images will be saved
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "Wfhuploads");

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




            WfhMangaer wfhManager = new WfhMangaer();
            log.Debug("Calling the Add function on Apply wfh page");
            OperationResult operationResult = wfhManager.Add(wfh);

            if (operationResult.Status == (int)Core.OperationStatus.Success)
            {
                Success(operationResult.Message);
                log.Debug("Apply function gets executed");
            }
            else
            {
                Warning(operationResult.Message);
                log.Debug("Apply function gets failed to add employee");


            }
            return RedirectToPage("List");
        }
    }
}
