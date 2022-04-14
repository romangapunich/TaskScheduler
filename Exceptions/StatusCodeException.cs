using System;
using System.Net;

namespace TaskScheduler.Exceptions
{
    /// <summary>
    /// Custom http status exception
    /// </summary>
    public class StatusCodeException : Exception
    {
        public StatusCodeException(HttpStatusCode statusCode, string description)
        {
            StatusCode = statusCode;
            Description = description;
        }
        /// <summary>
        /// HTTP status code
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }
        /// <summary>
        /// exception description
        /// </summary>
        public string Description { get; set; }
    }
}
