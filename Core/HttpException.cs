namespace OmPlatform.Core
{
    public class HttpException : Exception
    {
        public int Status {  get; set; }

        public HttpException(int status, string message) : base(message)
        {
            Status = status;
        }
    }
}
