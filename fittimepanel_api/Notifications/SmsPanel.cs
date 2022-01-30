using FittimePanelApi.INotifications;
using FittimePanelApi.Models.Notifications;
using Microsoft.Extensions.Logging;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FittimePanelApi.Notifications
{
    public class SmsPanel : ISmsPanel
    {
        private IRestClient _apiClient;
        private ILogger<SmsPanel> _logger;
        private string _username;
        private string _password;
        private string _from;

        public SmsPanel(IRestClient apiClient,
                        ILogger<SmsPanel> logger,
                        string username,
                        string password,
                        string from)
        {
            _apiClient = apiClient;
            _logger = logger;
            _username = username;
            _password = password;
            _from = from;
        }

        public async Task<IResponseSmsDTO> SendSimpleSMS(SendSimpleSmsDTO sendSimpleSmsDTO)
        {
            try
            {
                var restRequest = new RestRequest(new Uri("http://api.payamak-panel.com/post/Send.asmx/SendSimpleSMS"), Method.GET);

                restRequest.AddQueryParameter("username", sendSimpleSmsDTO.Username);
                restRequest.AddQueryParameter("password", sendSimpleSmsDTO.Password);
                restRequest.AddQueryParameter("from", sendSimpleSmsDTO.From);
                foreach (var To in sendSimpleSmsDTO.To)
                {
                    restRequest.AddQueryParameter("to", To);
                }
                //restRequest.AddQueryParameter("to", sendSimpleSmsDTO.To.FirstOrDefault());
                restRequest.AddQueryParameter("text", sendSimpleSmsDTO.Text);
                restRequest.AddQueryParameter("isflash", sendSimpleSmsDTO.IsFlush ? "true" : "false");

                var response = await _apiClient.GetAsync<ResponseSmsDTO>(restRequest);

                _logger.LogInformation("Sms deliverd");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(SendSimpleSMS)}");
                return new ResponseErrorSmsDTO(ex, $"Something Went Wrong in the {nameof(SendSimpleSMS)}");
            }
        }

        public async Task<IResponseSmsDTO> SendSMS(SendSmsDTO sendSmsDTO)
        {
            try
            {
                return await SendSimpleSMS(new SendSimpleSmsDTO()
                {
                    Username = _username,
                    Password = _password,
                    From = _from,
                    To = sendSmsDTO.To,
                    Text = sendSmsDTO.Text,
                    IsFlush = false,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(SendSMS)}");
                return new ResponseErrorSmsDTO(ex, $"Something Went Wrong in the {nameof(SendSMS)}");
            }
        }

    }
}
