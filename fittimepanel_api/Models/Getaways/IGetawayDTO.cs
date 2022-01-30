using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FittimePanelApi.Models.Getaways
{
    public interface IGetawayDTO
    {
    }

    public interface IResponsePaymentDTO
    {
    }

    public interface IResponsePaymentErrorDTO : IResponsePaymentDTO
    {
        public Exception Exception { get; set; }
        public string Error { get; set; }
    }
}
