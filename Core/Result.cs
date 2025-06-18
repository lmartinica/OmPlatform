namespace OmPlatform.Core
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public T Data { get; set; }
        public ErrorResponse Error { get; set; }

        public static Result<T> Success(T data) 
        {
            return new Result<T> { IsSuccess = true, Data = data };
        }

        public static Result<T> Failure(int statusCode, string message = null)
        {
            return new Result<T> { IsSuccess = false, Error = new ErrorResponse(statusCode, message) };
        }
    }
}
