namespace OmPlatform.Core
{
    public class ErrorResponse
    {
        private int _statusCode;

        public ErrorDetail Error { get; set; }

        public ErrorResponse(int statusCode, string? message = null)
        {
            Error = GetErrorResponse(statusCode);
            _statusCode = statusCode;
            if(message != null) Error.Message = message;
        }

        public int GetStatusCode()
        {
            return _statusCode;
        }

        private ErrorDetail GetErrorResponse(int status)
        {
            switch (status)
            {
                case 400: return GetError("BadRequest", "The request was malformed or contained invalid parameters.");
                case 401: return GetError("Unauthorized", "The provided credentials are invalid.");
                case 403: return GetError("Forbidden", "You do not have permission to access this resource.");
                case 404: return GetError("NotFound", "The requested resource could not be found.");
                case 405: return GetError("MethodNotAllowed", "The HTTP method is not allowed for the requested resource.");
                case 415: return GetError("UnsupportedMediaType", "The media type of the request is not supported. Please ensure the Content-Type header is correct.");
                case 422: return GetError("UnprocessableEntity", "The server understands the content type of the request, but the request is semantically erroneous.");
                case 429: return GetError("TooManyRequests", "You have sent too many requests in a given amount of time.");
                case 500: return GetError("InternalServerError", "The server encountered an unexpected condition.");
                case 502: return GetError("BadGateway", "The server, while acting as a gateway or proxy, received an invalid response from the upstream server.");
                case 503: return GetError("ServiceUnavailable", "The server is currently unable to handle the request due to a temporary overload or maintenance.");
                case 504: return GetError("GatewayTimeout", "The server, while acting as a gateway or proxy, did not receive a timely response from the upstream server.");
                default: return GetError("UnknownError", "An unknown error occurred.");
            }
        }

        private ErrorDetail GetError(string code, string message)
        {
            return new ErrorDetail
            {
                Code = code,
                Message = message
            };
        }
    }
}
