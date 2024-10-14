using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FreeEnterprise.Api.Classes
{
    /// <summary>
    /// <para>A class to encapsulate the status of an operation. Use when there are multiple possible failure modes that should be reported to the caller</para>
    /// <para>Should not be returned by a controller.</para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Response<T>
    {
        public T? Data { get; private set; }
        public string ErrorMessage { get; private set; } = string.Empty;
        public bool Success { get; private set; } = false;
        public HttpStatusCode? ErrorStatusCode { get; private set; }

        /// <summary>
        /// Basic constructor for the Response object. Generally prefer to use new Response<T>().SetSuccess() or SetError() to construct the response
        /// </summary>
        /// <param name="responseObject"></param>
        /// <param name="errorMessage"></param>
        /// <param name="success"></param>
        /// <param name="errorStatusCode">Should be not null when using this constructor for an error</param>
        public Response(T responseObject, string errorMessage = "", bool success = false, HttpStatusCode? errorStatusCode = null)
        {
            Data = responseObject;
            ErrorMessage = errorMessage;
            Success = success;
            ErrorStatusCode = errorStatusCode;
        }

        public Response() { }

        public Response<T> SetSuccess(T responseObject)
        {
            Data = responseObject;
            Success = true;
            ErrorStatusCode = null;
            ErrorMessage = string.Empty;
            return this;
        }

        private Response<T> SetError(string errorMessage, HttpStatusCode errorStatusCode = HttpStatusCode.InternalServerError)
        {
            ErrorMessage = errorMessage;
            Success = false;
            ErrorStatusCode = errorStatusCode;
            Data = default;
            return this;
        }

        public Response<T> BadRequest(string errorMessage) => SetError(errorMessage, HttpStatusCode.BadRequest);

        public Response<T> Unauthorized(string errorMessage) => SetError(errorMessage, HttpStatusCode.Unauthorized);

        public Response<T> NotFound(string errorMessage) => SetError(errorMessage, HttpStatusCode.NotFound);

        public Response<T> Conflict(string errorMessage) => SetError(errorMessage, HttpStatusCode.Conflict);

        public Response<T> InternalServerError(string errorMessage) => SetError(errorMessage, HttpStatusCode.InternalServerError);

        /// <summary>
        /// Controllers use this to return mostly reasonable status codes and data/messages back to external callers
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public ObjectResult GetRequestResponse() => (Success, ErrorStatusCode) switch
        {
            (true, _) => new OkObjectResult(Data),
            (false, HttpStatusCode.BadRequest) => new BadRequestObjectResult(ErrorMessage),
            (false, HttpStatusCode.Unauthorized) => new UnauthorizedObjectResult(ErrorMessage),
            (false, HttpStatusCode.NotFound) => new NotFoundObjectResult(ErrorMessage),
            (false, HttpStatusCode.Conflict) => new ConflictObjectResult(ErrorMessage),
            (false, HttpStatusCode.InternalServerError) => throw new InvalidOperationException(ErrorMessage),
            _ => new UnprocessableEntityObjectResult(ErrorMessage),
        };
    }
}
