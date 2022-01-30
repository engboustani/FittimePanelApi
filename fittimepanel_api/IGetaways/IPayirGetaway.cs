using FittimePanelApi.Models.Getaways;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FittimePanelApi.IGetaways
{
    public interface IPayirGetaway : IGetaway
    {
        public Task<IResponsePaymentDTO> Verify(string token);
    }
}
