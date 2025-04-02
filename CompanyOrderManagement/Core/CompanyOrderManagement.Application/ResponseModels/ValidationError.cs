using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyOrderManagement.Application.ResponseModels
{
    public class ValidationError
    {
        public string Property { get;  set; }
        public string Message { get;  set; }
        public ValidationError(string property, string message)
        {
            Property = property;
            Message = message;
        }

    }
}
