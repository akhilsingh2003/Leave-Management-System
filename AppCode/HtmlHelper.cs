using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Assingment.AppCode
{
    
        public static class IHtmlHelpers
        {

            public static HtmlString DisplayDate(this IHtmlHelper helper, DateTime date)
            {
                if (date == DateTime.MinValue)
                {
                    return new HtmlString(String.Empty);
                }
                else
                {
                    return new HtmlString(date.ToString("MMM dd,yyyy"));
                }
            }
        }
    
}
