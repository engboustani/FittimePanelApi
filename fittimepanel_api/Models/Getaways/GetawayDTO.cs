using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FittimePanelApi.Models.Getaways
{
    public class GetawayDTO : IGetawayDTO
    {

    }

    public class ResponsePaymentErrorDTO : IResponsePaymentErrorDTO
    {
        public ResponsePaymentErrorDTO(Exception exception, string error)
        {
            Exception = exception;
            Error = error;
        }

        public Exception Exception { get; set; }
        public string Error { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
