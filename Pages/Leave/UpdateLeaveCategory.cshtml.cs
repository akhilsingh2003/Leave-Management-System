using Core;
using Core.BusinessObject;
using Datalayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NLog.Fluent;
using ServiceLayer;

namespace Assingment.Pages.Leave
{
    [BindProperties]
    public class UpdateLeaveCategoryModel : BaseModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsDocumentRequired { get; set; }
        public void OnGet(int id)
        {
            LeaveCategoryManager leaveCategoryManager = new LeaveCategoryManager();
            LeaveCategory leaveCategory = leaveCategoryManager.GetById(id);
            Title = leaveCategory.Title;
            Id = leaveCategory.ID;


        }
        public Microsoft.AspNetCore.Mvc.IActionResult OnPost()
        {
            LeaveCategoryManager leaveCategoryManager = new LeaveCategoryManager();
            LeaveCategory leaveCategory = new LeaveCategory();
            leaveCategory.ID = Id;
            leaveCategory.Title = Title;
            leaveCategory.Description = Description;
            leaveCategory.IsDocumentRequired = IsDocumentRequired;
            leaveCategory.CreatedOnUTC = DateTime.UtcNow;
            leaveCategory.UpdatedOnUTC = DateTime.UtcNow;
            leaveCategory.CreatedBy = "System";
            leaveCategory.UpdatedBy = "System";
            leaveCategory.IPAddress = HttpContext.Connection.RemoteIpAddress.ToString();

            log.Debug("Calling Update method from the LeaveCategoryList");
            OperationResult operationResult = leaveCategoryManager.Update(leaveCategory);


            if (operationResult.Status == (int)Core.OperationStatus.Success)
            {
                Success(operationResult.Message);
                log.Debug("Update method get executed successfully for an LeaveCategory");
            }
            else
            {
                Warning(operationResult.Message);
                log.Debug("Update method gets failed to executed successfully for an LeaveCategory");
                return Page();

            };
            return RedirectToPage("LeaveCategoryList");
        }
    }
}
