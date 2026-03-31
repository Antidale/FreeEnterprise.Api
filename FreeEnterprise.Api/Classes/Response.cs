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
        /// Basic constructor for the Response object.
        /// </summary>
        /// <param name="responseObject"></param>
        /// <param name="errorMessage"></param>
        /// <param name="success"></param>
        /// <param name="errorStatusCode">Should be not null when using this constructor for an error</param>
        public Response(T? responseObject, string errorMessage = "", bool success = false, HttpStatusCode? errorStatusCode = null)
        {
            Data = responseObject;
            ErrorMessage = errorMessage;
            Success = success;
            ErrorStatusCode = errorStatusCode;
        }

        public Response() { }

        public static Response<T> SetSuccess(T responseObject)
        {
            return new Response<T>
            {
                Data = responseObject,
                Success = true,
                ErrorStatusCode = null,
                ErrorMessage = string.Empty
            };
        }

        private static Response<T> SetError(string errorMessage, HttpStatusCode errorStatusCode = HttpStatusCode.InternalServerError)
        {
            return new Response<T>
            {
                ErrorMessage = errorMessage,
                Success = false,
                ErrorStatusCode = errorStatusCode,
                Data = default
            };
        }

        public static Response<T> BadRequest(string errorMessage) => SetError(errorMessage, HttpStatusCode.BadRequest);

        public static Response<T> Unauthorized(string errorMessage) => SetError(errorMessage, HttpStatusCode.Unauthorized);

        public static Response<T> NotFound(string errorMessage) => SetError(errorMessage, HttpStatusCode.NotFound);

        public static Response<T> Conflict(string errorMessage) => SetError(errorMessage, HttpStatusCode.Conflict);

        public static Response<T> InternalServerError(string errorMessage) => SetError(errorMessage, HttpStatusCode.InternalServerError);

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

    public class Response
    {
        private HttpStatusCode? _errorStatusCode;
        public bool Success { get; private set; }
        public string ErrorMessage { get; private set; } = string.Empty;

        public static Response SetSuccess()
        {
            return new Response
            {
                Success = true,
                _errorStatusCode = null,
                ErrorMessage = string.Empty
            };
        }

        public static Response<T> SetSuccess<T>(T data)
        {
            return Response<T>.SetSuccess(data);
        }

        public static Response<T> BadRequest<T>(string errorMessage) => Response<T>.BadRequest(errorMessage);
        public static Response<T> Unauthorized<T>(string errorMessage) => Response<T>.Unauthorized(errorMessage);
        public static Response<T> NotFound<T>(string errorMessage) => Response<T>.NotFound(errorMessage);
        public static Response<T> Conflict<T>(string errorMessage) => Response<T>.Conflict(errorMessage);
        public static Response<T> InternalServerError<T>(string errorMessage) => Response<T>.InternalServerError(errorMessage);

        public Response BadRequest(string errorMessage) => SetError(errorMessage, HttpStatusCode.BadRequest);
        public Response Unauthorized(string errorMessage) => SetError(errorMessage, HttpStatusCode.Unauthorized);
        public Response NotFound(string errorMessage) => SetError(errorMessage, HttpStatusCode.NotFound);
        public Response Conflict(string errorMessage) => SetError(errorMessage, HttpStatusCode.Conflict);
        public Response InternalServerError(string errorMessage) => SetError(errorMessage, HttpStatusCode.InternalServerError);

        private static Response SetError(string errorMessage, HttpStatusCode errorStatusCode = HttpStatusCode.InternalServerError)
        {
            return new Response
            {
                ErrorMessage = errorMessage,
                Success = false,
                _errorStatusCode = errorStatusCode
            };
        }

        public ActionResult GetRequestResponse() => (Success, _errorStatusCode) switch
        {
            (true, _) => new NoContentResult(),
            (false, HttpStatusCode.BadRequest) => new BadRequestObjectResult(ErrorMessage),
            (false, HttpStatusCode.Unauthorized) => new UnauthorizedObjectResult(ErrorMessage),
            (false, HttpStatusCode.NotFound) => new NotFoundObjectResult(ErrorMessage),
            (false, HttpStatusCode.Conflict) => new ConflictObjectResult(ErrorMessage),
            (false, HttpStatusCode.InternalServerError) => throw new InvalidOperationException(ErrorMessage),
            _ => new UnprocessableEntityObjectResult(ErrorMessage),
        };
    }
}
