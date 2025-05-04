using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using OmPlatform.DTOs.Order;
using OmPlatform.DTOs.OrderItems;

namespace OmPlatform.Core
{
    public class FormatErrorsAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var errorBody = new
            {
                error = new
                {
                    status = "test",
                    message = "test"
                }
            };
          
            // Modify Action results errors
            if (context.Result is BadRequestObjectResult)
                context.Result = new BadRequestObjectResult(errorBody);
            else if (context.Result is ObjectResult objectResult)
            {
                switch (objectResult.StatusCode)
                {
                    case 401: context.Result = new UnauthorizedObjectResult("TODO"); break;
                    case 404: context.Result = new NotFoundObjectResult("TODO"); break;
                }
            }
                
            base.OnResultExecuting(context);
        }
    }
}
