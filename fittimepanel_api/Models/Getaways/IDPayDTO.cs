using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FittimePanelApi.Models.Getaways
{
    public class RequestPaymentIDPayDTO
    {
        public string OrderId { get; set; }
        public double Amount { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Mail { get; set; }
        public string Desc { get; set; }
        public string Callback { get; set; }
    }

    public class ResponseLinkCreatedDTO : IResponsePaymentDTO
    {
        public string Id { get; set; }
        public string Link { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class ResponsePaymentIDPayDTO : IResponsePaymentDTO
    {
        public int Status { get; set; }
        public int TrackId { get; set; }
        public string Id { get; set; }
        public string OrderId { get; set; }
        public double Amount { get; set; }
        public string CardNo { get; set; }
        public string HashedCardNo { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }

}
