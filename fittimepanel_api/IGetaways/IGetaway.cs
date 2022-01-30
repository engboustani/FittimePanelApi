using FittimePanelApi.Data;
using FittimePanelApi.Models.Getaways;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FittimePanelApi.IGetaways
{
    public interface IGetaway
    {
        public Task<IResponsePaymentDTO> GetPayLink(Payment payment);
    }
}
