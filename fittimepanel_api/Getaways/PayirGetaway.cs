using FittimePanelApi.Data;
using FittimePanelApi.IGetaways;
using FittimePanelApi.Models.Getaways;
using Microsoft.Extensions.Logging;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FittimePanelApi.Getaways
{
    public class PayirGetaway : IPayirGetaway
    {
        private IRestClient _apiClient;
        private ILogger _logger;
        private string _key;

        public PayirGetaway(IRestClient apiClient, ILogger logger, string key)
        {
            _apiClient = apiClient;
            _logger = logger;
            _key = key;
        }

        public async Task<IResponsePaymentDTO> GetPayLink(Payment payment)
        {
            try
            {
                var restRequest = new RestRequest(new Uri("https://pay.ir/pg/send"), Method.POST)
                    .AddJsonBody(new RequestPaymentPayirDTO()
                    {
                        factorNumber = payment.Id.ToString(),
                        amount = payment.Amount,
                        mobile = payment.User.PhoneNumber,
                        api = _key,
                        description = "Fittime Payment",
                        redirect = String.Format("https://panel.fittimeteam.com/api/Payment/callback/payir")
                    });

                var response = await _apiClient.PostAsync<ResponseLinkCreatedPayirDTO>(restRequest);

                _logger.LogInformation("Payment link recived");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetPayLink)}");
                return new ResponsePaymentErrorDTO(ex, $"Something Went Wrong in the {nameof(GetPayLink)}");
            }
        }

        public async Task<IResponsePaymentDTO> Verify(string token)
        {
            try
            {
                var restRequest = new RestRequest(new Uri("https://pay.ir/pg/verify"), Method.POST)
                    .AddJsonBody(new VerifyPaymentPayirDTO()
                    {
                        api = _key,
                        token = token,
                    });

                var response = await _apiClient.PostAsync<ResponseVerifyPayirDTO>(restRequest);

                _logger.LogInformation("Payment verified");
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
