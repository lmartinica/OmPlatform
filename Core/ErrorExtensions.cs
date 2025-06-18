using Microsoft.AspNetCore.Mvc;

namespace OmPlatform.Core
{
    public static class ErrorExtensions
    {
        public static ActionResult Error<T>(this ControllerBase controller, Result<T> result)
        {
            return controller.StatusCode(result.Error.GetStatusCode(), result.Error);
        }
    }
}
