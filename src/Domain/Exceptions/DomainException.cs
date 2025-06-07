using System.Net;

namespace Domain.Exceptions
{
    public class DomainException : Exception
    {
        public string ErrorCode { get; }
        public HttpStatusCode StatusCode { get; }
        public DomainException(string message,
                               string errorCode = "DOMAIN_ERROR",
                               HttpStatusCode statusCode = HttpStatusCode.BadRequest)
            : base(message)
        {
            ErrorCode = errorCode;
            StatusCode = statusCode;
        }
    }
}
