using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebUrlEditor.Models;

namespace WebUrlEditor.Filters;
public class UrlValidationFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ActionArguments.ContainsKey("request") &&
            context.ActionArguments["request"] is OriginalUrlRequest request)
        {
            if (!Uri.TryCreate(request.OriginalUrl, UriKind.Absolute, out Uri uriResult) ||
                uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps)
            {
                context.Result = new BadRequestObjectResult("Invalid URL format. Please provide a valid URL.");
            }
        }

        base.OnActionExecuting(context);
    }
}