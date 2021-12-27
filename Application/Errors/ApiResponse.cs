using System;
using System.Net;

namespace Application.Errors
{
    public class ApiResponse : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }

        public ApiResponse(HttpStatusCode statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode((int)StatusCode);
        }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A Bad Request you have made",
                401 => "Authorized, you are not",
                404 => "Resource found, it was not",
                500 => "One more error",
                _ => null
            };
        }
    }
}