using CompanyOrderManagement.Application.ResponseModels.Common;
using CompanyOrderManagement.Application.ResponseModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyOrderManagement.Application.ResponseModels
{
    public class NoContentResponse : BaseResponse
    {
        public NoContentResponse(ResponseType responseType, bool isSuccess) : base(responseType, isSuccess)
        {
        }
        public static NoContentResponse Success()
        {
            return new NoContentResponse(ResponseType.Success, true);
        }
        public static NoContentResponse Fail(ErrorDTO error)
        {
            var response = new NoContentResponse(ResponseType.Fail, false);
            return Fail(response, error);
        }
    }
}
