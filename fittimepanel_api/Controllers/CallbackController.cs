using AutoMapper;
using FittimePanelApi.Data;
using FittimePanelApi.IGetaways;
using FittimePanelApi.INotifications;
using FittimePanelApi.IRepository;
using FittimePanelApi.Models.Getaways;
using FittimePanelApi.Models.Notifications;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FittimePanelApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallbackController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CallbackController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IPayirGetaway _payir_getaway;
        private readonly IIDPayGetaway _idpay_getaway;
        private readonly ISmsPanel _smsPanel;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly AppDb _context;

        public CallbackController(UserManager<User> userManager
                                , IUnitOfWork unitOfWork
                                , ILogger<CallbackController> logger
                                , IMapper mapper
                                , IPayirGetaway payir_getaway
                                , IIDPayGetaway idpay_getaway
                                , ISmsPanel smsPanel
                                , IHostingEnvironment hostingEnvironment
                                , AppDb context)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _payir_getaway = payir_getaway;
            _idpay_getaway = idpay_getaway;
            _smsPanel = smsPanel;
            _hostingEnvironment = hostingEnvironment;
            _context = context;
        }

        // GET: api/Callback/payir
        [HttpGet("payir")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CallbackPayir([FromQuery] int status, [FromQuery] string token)
        {
            var url = "https://panel.fittimeteam.com/";
            if (_hostingEnvironment.IsDevelopment())
                url = "http://localhost:8080/";
            try
            {
                Payment payment;
                if (status == 1)
                {
                    ResponseVerifyPayirDTO responseVerifyPayir = (ResponseVerifyPayirDTO)await _payir_getaway.Verify(token);
                    if (responseVerifyPayir.status == 1)
                    {
                        string paymentId = responseVerifyPayir.factorNumber;
                        payment = await _unitOfWork.Payments.Get(p => p.Id == Guid.Parse(paymentId), new List<string> { "Exercise", "User" });
                        payment.Exercise.Status = Exercise.ExerciseStatus.Paid;
                        payment.Status = PaymentStatus.Successful;
                        _unitOfWork.Payments.Update(payment);
                        _unitOfWork.Exercises.Update(payment.Exercise);
                        await _unitOfWork.Save();

                        try
                        {
                            await _smsPanel.SendSMS(new SendSmsDTO()
                            {
                                To = new string[] { payment.User.PhoneNumber },
                                Text = String.Format("{0} عزیز! شما با موفقیت برنامه خود را پرداخت کردید. فیت تایم", payment.User.FullName)
                            });
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Cant send sms");
                        }

                        return Redirect(String.Format("{0}#/payment/{1}", url, payment.Id));
                    }
                    else
                    {
                        payment = await _unitOfWork.Payments.Get(p => p.Token == token);
                        payment.Status = PaymentStatus.Failed;
                        _unitOfWork.Payments.Update(payment);
                        await _unitOfWork.Save();

                        return Redirect(String.Format("{0}#/payment/{1}", url, payment.Id));
                    }
                }
                payment = await _unitOfWork.Payments.Get(p => p.Token == token);
                payment.Status = PaymentStatus.Failed;
                _unitOfWork.Payments.Update(payment);
                await _unitOfWork.Save();

                return Redirect(String.Format("{0}#/payment/{1}", url, payment.Id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(CallbackPayir)}");
                return StatusCode(500, "Internal Server Error, Please try again later.");
            }
        }

        // GET: api/Callback/idpay
        [HttpGet("idpay")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CallbackIDPay([FromQuery] GetCallbackIDPay qs)
        {
            var url = "https://panel.fittimeteam.com/";
            if (_hostingEnvironment.IsDevelopment())
                url = "http://localhost:8080/";
            try
            {
                Payment payment;
                if (qs.Status == 10)
                {
                    VerifyPaymentResponseIDPayDTO responseVerifyIDPay = (VerifyPaymentResponseIDPayDTO)await _idpay_getaway.Verify(qs.Id, qs.Order_Id);
                    if (responseVerifyIDPay.Status == 100)
                    {
                        string paymentId = responseVerifyIDPay.Order_Id;
                        payment = await _unitOfWork.Payments.Get(p => p.Id == Guid.Parse(paymentId), new List<string> { "Exercise", "User" });
                        payment.Exercise.Status = Exercise.ExerciseStatus.Paid;
                        payment.Status = PaymentStatus.Successful;
                        _unitOfWork.Payments.Update(payment);
                        _unitOfWork.Exercises.Update(payment.Exercise);
                        await _unitOfWork.Save();

                        try
                        {
                            await _smsPanel.SendSMS(new SendSmsDTO()
                            {
                                To = new string[] { payment.User.PhoneNumber },
                                Text = String.Format("{0} عزیز! شما با موفقیت برنامه خود را پرداخت کردید. فیت تایم", payment.User.FullName)
                            });
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Cant send sms");
                        }

                        return Redirect(String.Format("{0}#/payment/{1}", url, payment.Id));
                    }
                    else
                    {
                        payment = await _unitOfWork.Payments.Get(p => p.Id == Guid.Parse(qs.Order_Id));
                        payment.Status = PaymentStatus.Failed;
                        _unitOfWork.Payments.Update(payment);
                        await _unitOfWork.Save();

                        return Redirect(String.Format("{0}#/payment/{1}", url, payment.Id));
                    }
                }
                payment = await _unitOfWork.Payments.Get(p => p.Id == Guid.Parse(qs.Order_Id));
                payment.Status = PaymentStatus.Failed;
                _unitOfWork.Payments.Update(payment);
                await _unitOfWork.Save();

                return Redirect(String.Format("{0}#/payment/{1}", url, payment.Id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(CallbackPayir)}");
                return StatusCode(500, "Internal Server Error, Please try again later.");
            }
        }


    }
}
