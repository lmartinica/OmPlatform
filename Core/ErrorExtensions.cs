using Microsoft.AspNetCore.Mvc;

namespace OmPlatform.Core
{
    public static class ErrorExtensions
    {
        public static ActionResult ErrorBadRequest(this ControllerBase controller, string? message = null)
        {
            return controller.BadRequest(new ErrorResponse(400, message));
        }

        public static ActionResult ErrorUnauthorized(this ControllerBase controller, string? message = null)
        {
            return controller.Unauthorized(new ErrorResponse(401, message));
        }

        public static ActionResult ErrorNotFound(this ControllerBase controller, string? message = null)
        {
            return controller.NotFound(new ErrorResponse(404, message));
        }
    }
}
