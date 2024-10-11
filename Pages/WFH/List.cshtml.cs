using Core;
using Core.BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NLog.Fluent;
using ServiceLayer;

namespace Assingment.Pages.WFH
{
    public class ListModel : BaseModel
    {
        private const int PageSize = 5; // Number of items per page

        public List<Wfh> WFHList { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public IActionResult OnGet(int? pageIndex)
        {
            WfhMangaer wfhManager = new WfhMangaer();
            log.Debug("Calling the WFHlist on get method");
            var AllWfhRequest = wfhManager.GetAll();
            log.Debug("List page on get method executed");
            // Calculate total pages
            log.Debug("Calling the CountAllWFH REQUEST on Get Method");
            TotalPages = (int)Math.Ceiling((double)wfhManager.CountAllWFH() / PageSize);
            log.Debug(" CountAllWFH on Get Method executed");

            // Validate page index
            PageIndex = pageIndex ?? 1; // If pageIndex is null, default to page 1
            PageIndex = PageIndex < 1 ? 1 : PageIndex;
            PageIndex = PageIndex > TotalPages ? TotalPages : PageIndex;

            // Calculate the starting index of the current page
            int startIndex = (PageIndex - 1) * PageSize;

            // Select employees for the current page
            WFHList = AllWfhRequest.Skip(startIndex).Take(PageSize).ToList();

            return Page();
        }
        public Microsoft.AspNetCore.Mvc.IActionResult OnGetDelete(int id)
        {
            WfhMangaer wfhManager = new WfhMangaer();
            log.Debug("Delete function calling on applied WFH List");
            OperationResult operationResult = wfhManager.Delete(id);

            if (operationResult.Status == (int)Core.OperationStatus.Success)
            {
                Success(operationResult.Message);
                log.Debug("Delete function gets executed successfully for wfh");
            }
            else
            {
                Warning(operationResult.Message);
                log.Debug("Delete function gets failed  to delete the wfh");

                return Page();

            }
            return RedirectToPage("List");

        }
    }
}
