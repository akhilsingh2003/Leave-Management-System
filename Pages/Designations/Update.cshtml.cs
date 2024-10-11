using Core.BusinessObject;
using Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NLog.Fluent;
using ServiceLayer;
using Datalayer;

namespace Assingment.Pages.Designations
{
    [BindProperties]
    public class UpdateModel : BaseModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public void OnGet(int id)
        {
            DesignationManager designationManager = new DesignationManager();
            Designation designation = designationManager.GetById(id);
            Title = designation.Title;
            ID = designation.ID;


        }
        public Microsoft.AspNetCore.Mvc.IActionResult OnPost()
        {
            DesignationManager designationManager = new DesignationManager();
            Designation designation = new Designation();
            designation.ID = ID;
            designation.Title = Title;
            designation.CreatedBy = "System";
            designation.CreatedOnUTC = DateTime.UtcNow;
            designation.UpdatedBy = "System";
            designation.UpdatedOnUTC = DateTime.UtcNow;
            designation.IPAddress = HttpContext.Connection.RemoteIpAddress.ToString();

            log.Debug("Calling Update method from the DesignationList");
            OperationResult operationResult = designationManager.Update(designation);


            if (operationResult.Status == (int)Core.OperationStatus.Success)
            {
                Success(operationResult.Message);
                log.Debug("Update method get executed successfully for an designation");
            }
            else
            {
                Warning(operationResult.Message);
                log.Debug("Update method gets failed to executed successfully for an designation");
                return Page();

            };
            return RedirectToPage("List");
        }
    }
}
