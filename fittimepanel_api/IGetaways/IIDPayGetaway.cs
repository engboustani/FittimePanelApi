using FittimePanelApi.Models.Getaways;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FittimePanelApi.IGetaways
{
    public interface IIDPayGetaway : IGetaway
    {
        Task<IResponsePaymentDTO> Verify(string id, string order_id);
    }
}
