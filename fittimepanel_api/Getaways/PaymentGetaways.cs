using FittimePanelApi.Data;
using FittimePanelApi.IGetaways;
using FittimePanelApi.IRepository;
using FittimePanelApi.Models.Getaways;
using Microsoft.Extensions.Logging;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FittimePanelApi.Getaways
{
    public class PaymentGetaways : IPaymentGetaways
    {
        private IUnitOfWork _unitOfWork;
        private IRestClient _apiClient;
        private ILogger<PaymentGetaways> _logger;
        private IIDPayGetaway _idpay_getaway;
        private IPayirGetaway _payir_getaway;

        public PaymentGetaways(IUnitOfWork unitOfWork, IRestClient apiClient, ILogger<PaymentGetaways> logger, IIDPayGetaway idpay_getaway, IPayirGetaway payir_getaway)
        {
            _unitOfWork = unitOfWork;
            _apiClient = apiClient;
            _logger = logger;
            _idpay_getaway = idpay_getaway;
            _payir_getaway = payir_getaway;
        }

        public async Task<string> GetLink(Payment payment)
        {
            switch (payment.PaymentGetwayId)
            {
                case 1:
                    return await GetPayirLink(payment);
                case 2:
                    return await GetIDPayLink(payment);
                default:
                    return await GetPayirLink(payment);
            }
        }

        private async Task<string> GetPayirLink(Payment payment)
        {
            ResponseLinkCreatedPayirDTO result = (ResponseLinkCreatedPayirDTO)await _payir_getaway.GetPayLink(payment);
            if (result.status == 1)
            {
                payment.Token = result.token;
                payment.Status = PaymentStatus.GoesToGetway;
                _unitOfWork.Payments.Update(payment);
                await _unitOfWork.Save();

                return String.Format("https://pay.ir/pg/{0}", payment.Token);
            }
            throw new Exception("Can't get link from payir");
        }

        private async Task<string> GetIDPayLink(Payment payment)
        {
            var result = (ResponseLinkCreatedDTO)await _idpay_getaway.GetPayLink(payment);
            if (result.Link != null)
            {
                payment.Token = result.Id;
                payment.Status = PaymentStatus.GoesToGetway;
                _unitOfWork.Payments.Update(payment);
                await _unitOfWork.Save();
                
                return result.Link;
            }
            _logger.LogError("Can't get link from idpay");
            throw new Exception("Can't get link from idpay");
        }

    }
}
