using CompanyOrderManagement.Application.ResponseModels.Common;
using CompanyOrderManagement.Application.ResponseModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyOrderManagement.Application.ResponseModels
{
    public class ApiResponse<T> : BaseResponse
    {
        public T Data { get; set; }
        public ApiResponse(ResponseType responseType, bool isSuccess, T data) : base(responseType, isSuccess)
        {
            Data = data;
        }
        public static ApiResponse<T> Success(T data)
        {
            return new ApiResponse<T>(ResponseType.Success, true, data);
        }
        public static ApiResponse<T> Fail(ErrorDTO error)
        {
            var response = new ApiResponse<T>(ResponseType.Fail, false, default);
            return Fail(response, error);
        }
        public static ApiResponse<T> ValidationError(List<ValidationError> validationErrors)
        {
            var response = new ApiResponse<T>(ResponseType.ValidationError, false, default);
            return ValidationError(response, validationErrors);
        }
    }
}
