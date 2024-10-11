using Core.BusinessObject;
using Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NLog.Fluent;
using ServiceLayer;
using System.Xml.Linq;
using Datalayer;

namespace Assingment.Pages.Designations
{
    [BindProperties]
    public class AddModel : BaseModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public void OnGet()
        {
        }
        public Microsoft.AspNetCore.Mvc.IActionResult OnPost()
        {
            Designation designation = new Designation();
            designation.Title = Title;
            designation.CreatedOnUTC = DateTime.UtcNow;
            designation.UpdatedOnUTC = DateTime.UtcNow;
            designation.CreatedBy = "System";
            designation.UpdatedBy = "System";
            designation.IPAddress = HttpContext.Connection.RemoteIpAddress.ToString();

            DesignationManager designationManager = new DesignationManager();
            log.Debug("Calling the Add function for Designation");
            OperationResult operationResult = designationManager.Add(designation);

            if (operationResult.Status == (int)Core.OperationStatus.Success)
            {
                Success(operationResult.Message);
                log.Debug("Add function for Designation gets executed successfully");
            }
            else
            {
                Warning(operationResult.Message);
                log.Debug("Add function for Designation gets failed tlo add ");
                return Page();

            }
            return RedirectToPage("List");
        }
    }
}
