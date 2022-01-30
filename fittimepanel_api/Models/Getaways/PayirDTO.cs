using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FittimePanelApi.Models.Getaways
{
    public class RequestPaymentPayirDTO
    {
        public string api { get; set; }
        public double amount { get; set; }
        public string redirect { get; set; }
        public string mobile { get; set; }
        public string factorNumber { get; set; }
        public string description { get; set; }
    }

    public class VerifyPaymentPayirDTO
    {
        public string api { get; set; }
        public string token { get; set; }
    }


    public class ResponseLinkCreatedPayirDTO : IResponsePaymentDTO
    {
        public int status { get; set; }
        public string token { get; set; }
        public string errorCode { get; set; }
        public string errorMessage { get; set; }
    }

    public class ResponseVerifyPayirDTO : IResponsePaymentDTO
    {
        public int status { get; set; }
        public string amount { get; set; }
        public string transId { get; set; }
        public string factorNumber { get; set; }
        public string mobile { get; set; }
        public string description { get; set; }
        public string cardNumber { get; set; }
        public string message { get; set; }
        public string errorCode { get; set; }
        public string errorMessage { get; set; }
    }


}
