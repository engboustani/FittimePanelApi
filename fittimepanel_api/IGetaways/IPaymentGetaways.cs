using FittimePanelApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FittimePanelApi.IGetaways
{
    public interface IPaymentGetaways
    {
        Task<string> GetLink(Payment payment);
    }
}
