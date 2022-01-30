using AutoMapper;
using FittimePanelApi.Data;
using FittimePanelApi.IControllers;
using FittimePanelApi.IGetaways;
using FittimePanelApi.IRepository;
using FittimePanelApi.Models;
using FittimePanelApi.Models.Getaways;
using Microsoft.AspNetCore.Authorization;
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
    public class PaymentController : ControllerBase, IPaymentController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ExercisesController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IPayirGetaway _payir_getaway;

        public PaymentController(UserManager<User> userManager
                                , IUnitOfWork unitOfWork
                                , ILogger<ExercisesController> logger
                                , IMapper mapper
                                , IPayirGetaway payir_getaway)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _payir_getaway = payir_getaway;
        }

        // DELETE: api/Payments/<uuid>
        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public Task<IActionResult> DeleteById(Guid id)
        {
            throw new NotImplementedException();
        }

        // GET: api/Exercises
        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReadAll()
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(User);
                var payments = await _unitOfWork.Payments.GetAll(
                    expression: q => q.User == currentUser);
                var result = _mapper.Map<IList<PaymentDTO>>(payments);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(ReadAll)}");
                return StatusCode(500, "Internal Server Error, Please try again later.");
            }
        }

        // GET: api/Payment/Link/{id:Guid}
        [Authorize]
        [HttpGet("Link/{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLink(Guid id)
        {
            try
            {
                var payment = await _unitOfWork.Payments.Get(q => q.Id == id, new List<string> { "Exercise", "PaymentGetway", "User" });
                ResponseLinkCreatedPayirDTO result = (ResponseLinkCreatedPayirDTO)await _payir_getaway.GetPayLink(payment);
                if(result.status == 1)
                {
                    payment.Token = result.token;
                    payment.Status = PaymentStatus.GoesToGetway;
                    _unitOfWork.Payments.Update(payment);
                    await _unitOfWork.Save();

                    return Ok(new PaymentLinkDTO()
                    {
                        Link = String.Format("https://pay.ir/pg/{0}", payment.Token)
                    });
                }

                return StatusCode(500, "Can't get token from payir.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(ReadAll)}");
                return StatusCode(500, "Internal Server Error, Please try again later.");
            }
        }

        // GET: api/Payment/callback/payir
        [HttpGet("callback/payir")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CallbackPayir([FromQuery] int status, [FromQuery] string token)
        {
            try
            {
                Payment payment;
                if (status == 1)
                {
                    ResponseVerifyPayirDTO responseVerifyPayir = (ResponseVerifyPayirDTO)await _payir_getaway.Verify(token);
                    if(responseVerifyPayir.status == 1)
                    {
                        string paymentId = responseVerifyPayir.factorNumber;
                        payment = await _unitOfWork.Payments.Get(p => p.Id == Guid.Parse(paymentId));
                        payment.Status = PaymentStatus.Successful;
                        _unitOfWork.Payments.Update(payment);
                        await _unitOfWork.Save();

                        return Redirect(String.Format("http://localhost:8080/#/payment/{0}", payment.Id));
                    }
                    else
                    {
                        payment = await _unitOfWork.Payments.Get(p => p.Token == token);
                        payment.Status = PaymentStatus.Failed;
                        _unitOfWork.Payments.Update(payment);
                        await _unitOfWork.Save();

                        return Redirect(String.Format("http://localhost:8080/#/payment/{0}", payment.Id));
                    }
                }
                payment = await _unitOfWork.Payments.Get(p => p.Token == token);
                payment.Status = PaymentStatus.Failed;
                _unitOfWork.Payments.Update(payment);
                await _unitOfWork.Save();

                return Redirect(String.Format("http://localhost:8080/#/payment/{0}", payment.Id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(CallbackPayir)}");
                return StatusCode(500, "Internal Server Error, Please try again later.");
            }
        }

        // GET: api/Payments/Types
        [Authorize]
        [HttpGet("Types")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReadAllTypes()
        {
            try
            {
                var paymentGetaways = await _unitOfWork.PaymentGetaways.GetAll(expression: q => q.Enabled == true);
                var result = _mapper.Map<IList<PaymentGetawayDTO>>(paymentGetaways);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(ReadAllTypes)}");
                return StatusCode(500, "Internal Server Error, Please try again later.");
            }
        }

        // GET: api/Payments/<uuid>
        [Authorize]
        [HttpGet("{id:Guid}", Name = "ReadPaymentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReadById(Guid id)
        {
            try
            {
                var payment = await _unitOfWork.Payments.Get(q => q.Id == id, new List<string> { "Exercise", "PaymentGetway" });
                var result = _mapper.Map<PaymentDetailDTO>(payment);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(ReadById)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        // POST: api/Payments
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> New([FromBody] CreatePaymentDTO createPaymentDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(New)}");
                return BadRequest(ModelState);
            }

            try
            {
                var payment = _mapper.Map<Payment>(createPaymentDTO);
                var exercise = await _unitOfWork.Exercises.Get(
                    expression: q => q.Id == createPaymentDTO.ExerciseId,
                    includes: new List<string> { "ExerciseType" });
                var currentUser = await _userManager.GetUserAsync(User);
                payment.User = currentUser;
                payment.Amount = exercise.ExerciseType.Price;
                payment.Status = PaymentStatus.Created;

                await _unitOfWork.Payments.Insert(payment);
                await _unitOfWork.Save();

                return CreatedAtRoute("ReadPaymentById", new { id = payment.Id }, payment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(New)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }


    }
}
