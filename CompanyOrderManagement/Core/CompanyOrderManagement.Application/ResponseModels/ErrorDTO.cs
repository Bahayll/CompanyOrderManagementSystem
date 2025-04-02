using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyOrderManagement.Application.ResponseModels
{
    public class ErrorDTO
    {
        public List<ValidationError> ValidationErrors { get;  set; }
        public bool IsShow { get;  set; }

        public ErrorDTO(List<ValidationError> validationErrors, bool isShow)
        {
            ValidationErrors = validationErrors ?? new List<ValidationError>();
            IsShow = isShow;
        }

      
    }
}
