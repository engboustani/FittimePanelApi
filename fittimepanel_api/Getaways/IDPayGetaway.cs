using FittimePanelApi.Data;
using FittimePanelApi.IGetaways;
using FittimePanelApi.Models.Getaways;
using Microsoft.Extensions.Logging;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FittimePanelApi.Getaways
{
    public class IDPayGetaway : IIDPayGetaway
    {
        private IRestClient _apiClient;
        private ILogger _logger;
        private string _key;

        public IDPayGetaway(IRestClient apiClient, ILogger logger, string key)
        {
            _apiClient = apiClient;
            _logger = logger;
            _key = key;
        }

        public async Task<IResponsePaymentDTO> GetPayLink(Payment payment)
        {
            try
            {
                var restRequest = new RestRequest(new Uri("https://api.idpay.ir/v1.1/payment"), Method.POST)
                    .AddJsonBody(new RequestPaymentIDPayDTO()
                    {
                        OrderId = payment.Id.ToString(),
                        Amount = payment.Amount,
                        Name = payment.User.FullName, 
                        Phone = payment.User.PhoneNumber,
                        Mail = payment.User.Email,
                        Desc = "Fittime Payment",
                        Callback = String.Format("https://panel.fittimeteam.com/api/Payments/callback/idpay")
                    });
                restRequest.AddHeader("X-API-KEY", _key);

                var response = await _apiClient.PostAsync<ResponseLinkCreatedDTO>(restRequest);

                _logger.LogInformation("Payment link recived");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetPayLink)}");
                return new ResponsePaymentErrorDTO(ex, $"Something Went Wrong in the {nameof(GetPayLink)}");
            }
        }
    }
}
