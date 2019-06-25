using Dictionary.API.Helpers;
using Dictionary.Data;
using Dictionary.Domain.Enums;
using Dictionary.Domain.Infrastructures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Dictionary.API.Base
{
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        private const string BadRequestMessage = "Bad Request.";
        private readonly ApiSettings appsettings;

        protected TurkishDictionary TurkishDictionaryDb { get; set; }

        public BaseApiController(IOptions<ApiSettings> appSettings)
        {
            TurkishDictionaryDb = new TurkishDictionary(null);
            appsettings = appSettings.Value;
        }


        protected ApiReturn<T> Error<T>(string message = BadRequestMessage, string internalMessage = default(string), ApiStatusCode code = ApiStatusCode.BadRequest)
        {
            return Error<T>(null, message, internalMessage, code);
        }

        protected ApiReturn<T> Error<T>(ApiErrorCollection errors, string message = BadRequestMessage, string internalMessage = default(string), ApiStatusCode code = ApiStatusCode.BadRequest)
        {
            return new ApiReturn<T>
            {
                Code = code,
                Message = message,
                Success = false,
                Errors = errors
            };
        }
        protected ApiReturn<T> InternalError<T>(ApiErrorCollection errors, string message = "There is something wrong.", string internalMessage = default(string), ApiStatusCode code = ApiStatusCode.InternalServerError)
        {
            return new ApiReturn<T>
            {
                Code = code,
                Message = message,
                Success = false,
                Errors = errors
            };
        }

        protected ApiReturn<T> Invalid<T>(string message = "Invalid action.", string internalMessage = default(string), ApiStatusCode code = ApiStatusCode.BadRequest)
        {
            return new ApiReturn<T>
            {
                Code = code,
                Message = message,
                InternalMessage = internalMessage,
                Success = false
            };
        }

        protected ApiReturn<T> NotFound<T>(ApiErrorCollection errors, string message = "Not Found.", string internalMessage = default(string), ApiStatusCode code = ApiStatusCode.NotFound)
        {
            return new ApiReturn<T>
            {
                Code = code,
                Message = message,
                Success = false,
                Errors = errors
            };
        }

        protected ApiReturn<T> Success<T>(T data, string message = "Success")
        {
            return new ApiReturn<T>
            {
                Code = ApiStatusCode.Success,
                Message = message,
                Success = true,
                Data = data
            };
        }
    }
}
