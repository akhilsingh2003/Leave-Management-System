using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NLog;

namespace Assingment.Pages
{
    public class BaseModel : PageModel
    {
        public static Logger log = LogManager.GetLogger("Assingment");

        public void Warning(string message)
        {
            TempData["WarningMessage"] = message;
        }

        public void Success(string message)
        {
            TempData["SuccessMessage"] = message;
        }
    }
}
