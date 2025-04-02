using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyOrderManagement.Application.ResponseModels.Enums
{
    public enum ResponseType
    {
        Success=0,
        Fail=1,
        ValidationError=2,
        NotFound=3
    }
}
