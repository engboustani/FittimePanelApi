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
                        order_id = payment.Id.ToString(),
                        amount = payment.Amount,
                        name = payment.User.FullName, 
                        phone = payment.User.PhoneNumber,
                        mail = payment.User.Email,
                        desc = "Fittime Payment",
                        callback = String.Format("https://panel.fittimeteam.com/v1/api/Callback/idpay")
                    });
                restRequest.AddHeader("X-API-KEY", _key);
                //restRequest.AddHeader("X-SANDBOX", "1");

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

        public async Task<IResponsePaymentDTO> Verify(string id, string order_id)
        {
            try
            {
                var restRequest = new RestRequest(new Uri("https://api.idpay.ir/v1.1/payment/verify"), Method.POST)
                    .AddJsonBody(new VerifyPaymentIDPayDTO()
                    {
                        id = id,
                        order_id = order_id,
                    });
                restRequest.AddHeader("X-API-KEY", _key);
                //restRequest.AddHeader("X-SANDBOX", "1");

                var response = await _apiClient.PostAsync<VerifyPaymentResponseIDPayDTO>(restRequest);

                _logger.LogInformation("Payment verified");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(Verify)}");
                return new ResponsePaymentErrorDTO(ex, $"Something Went Wrong in the {nameof(Verify)}");
            }
        }

    }
}
