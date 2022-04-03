using FittimePanelApi.Models.URLShortening;
using Microsoft.Extensions.Logging;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FittimePanelApi.Services
{
    public class URLShortening : IURLShortening
    {
        private IRestClient _apiClient;
        private ILogger<URLShortening> _logger;
        private string _key;

        public URLShortening(IRestClient apiClient,
                        ILogger<URLShortening> logger,
                        string key)
        {
            _apiClient = apiClient;
            _logger = logger;
            _key = key;
        }

        public async Task<IURLShorteningResponseDTO> ShortURL(string title, string url)
        {
            try
            {
                var restRequest = new RestRequest(new Uri("https://yun.ir/api/v1/urls"), Method.POST)
                                .AddJsonBody(new URLShorteningRequestDTO()
                                {
                                    Url = url,
                                    Title = title,
                                })
                                .AddHeader("X-API-Key", _key);

                var response = await _apiClient.PostAsync<URLShorteningResponseDTO>(restRequest);

                _logger.LogInformation("Short URL recived");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(ShortURL)}");
                return new URLShorteningErrorDTO() {Success = false };
            }

        }
    }
}
