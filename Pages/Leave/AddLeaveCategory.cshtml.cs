using Core.BusinessObject;
using Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NLog.Fluent;
using ServiceLayer;
using System.Xml.Linq;

namespace Assingment.Pages.Leave
{
    [BindProperties]
    public class AddLeaveCategoryModel : BaseModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsDocumentRequired { get; set; }
        public void OnGet()
        {
        }

        public Microsoft.AspNetCore.Mvc.IActionResult OnPost()
        {
            LeaveCategory leaveCategory = new LeaveCategory();
            leaveCategory.Title = Title;
            leaveCategory.Description = Description;
            leaveCategory.IsDocumentRequired = IsDocumentRequired;
            leaveCategory.CreatedOnUTC = DateTime.UtcNow;
            leaveCategory.UpdatedOnUTC = DateTime.UtcNow;
            leaveCategory.CreatedBy = "System";
            leaveCategory.UpdatedBy = "System";
            leaveCategory.IPAddress = HttpContext.Connection.RemoteIpAddress.ToString();

            LeaveCategoryManager leaveCategoryManager = new LeaveCategoryManager();
            log.Debug("Calling the Add function for Department");
            OperationResult operationResult = leaveCategoryManager.Add(leaveCategory);

            if (operationResult.Status == (int)Core.OperationStatus.Success)
            {
                Success(operationResult.Message);
                log.Debug("Add function for LeaveCategory gets executed successfully");
            }
            else
            {
                Warning(operationResult.Message);
                log.Debug("Add function for Leave Category gets failed to add ");
                return Page();

            }
            return RedirectToPage("LeaveCategoryList");
        }
    }
}
