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
            var order = new GetOrderDto();
            order.OrderItems = new List<GetOrderItemDto>();
            order.OrderItems.Add(new GetOrderItemDto());

            // Modify Action results errors
            if (context.Result is BadRequestObjectResult)
                context.Result = new BadRequestObjectResult(order);
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
