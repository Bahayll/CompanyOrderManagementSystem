using CompanyOrderManagement.Application.ResponseModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CompanyOrderManagement.Application.ResponseModels.Common
{
    public abstract class BaseResponse
    {
        public bool IsSuccess { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ResponseType ResponseType { get; set; }
        public ErrorDTO errorDTO { get; set; }
        public List<ValidationError> ValidationErrors { get; set; }

        public BaseResponse(ResponseType responseType, bool isSuccess)
        {
            ResponseType = responseType;
            IsSuccess = isSuccess;
            ValidationErrors = new List<ValidationError>();
        }
        public static TFail Fail<TFail>(TFail response, ErrorDTO error)
          where TFail : BaseResponse
        {
            response.IsSuccess = false;
            response.ResponseType = ResponseType.Fail;
            response.errorDTO = error;
            return response;
        }
        public static TValidationError ValidationError<TValidationError>(TValidationError response, List<ValidationError> validationErrors)
            where TValidationError : BaseResponse
        {
            response.IsSuccess = false;
            response.ResponseType = ResponseType.ValidationError;
            response.ValidationErrors = validationErrors;
            return response;
        }
    }
}
