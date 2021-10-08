using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fittimepanel_api.Models
{
    public class BaseEntity
    {
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public class ErrorResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }
}
