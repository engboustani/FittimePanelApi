using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FittimePanelApi.Models.Getaways
{
    public class IDPayError
    {
        public int Error_Code { get; set; }
        public string Error_Message { get; set; }
    }

    public class RequestPaymentIDPayDTO
    {
        public string order_id { get; set; }
        public double amount { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string mail { get; set; }
        public string desc { get; set; }
        public string callback { get; set; }
    }

    public class ResponseLinkCreatedDTO : IDPayError, IResponsePaymentDTO
    {
        public string Id { get; set; }
        public string Link { get; set; }
    }

    public class ResponsePaymentIDPayDTO : IDPayError, IResponsePaymentDTO
    {
        public int Status { get; set; }
        public string Track_Id { get; set; }
        public string Id { get; set; }
        public string Order_Id { get; set; }
        public double Amount { get; set; }
        public string Card_No { get; set; }
        public string Hashed_Card_No { get; set; }
        public DateTime Date { get; set; }
    }

    public class VerifyPaymentIDPayDTO
    {
        public string id { get; set; }
        public string order_id { get; set; }
    }
    public class VerifyPaymentResponseIDPayDTO : IDPayError, IResponsePaymentDTO
    {
        public int Status { get; set; }
        public string Track_Id { get; set; }
        public string Id { get; set; }
        public string Order_Id { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }
        public PaymentProp Payment { get; set; }
        public VerifyProp Verify { get; set; }
        public class PaymentProp
        {
            public string Track_Id { get; set; }
            public int Amount { get; set; }
            public string Card_No { get; set; }
            public string Hashed_Card_No { get; set; }
            public DateTime Date { get; set; }
        }
        public class VerifyProp
        {
            public DateTime Date { get; set; }
        }
    }

    public class GetCallbackIDPay
    {
        public int Status { get; set; }
        public string Track_Id { get; set; }
        public string Id { get; set; }
        public string Order_Id { get; set; }
    }
}
