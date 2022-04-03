using AutoMapper;
using FittimePanelApi.Data;
using FittimePanelApi.IControllers;
using FittimePanelApi.IGetaways;
using FittimePanelApi.INotifications;
using FittimePanelApi.IRepository;
using FittimePanelApi.Models;
using FittimePanelApi.Models.Getaways;
using FittimePanelApi.Models.Notifications;
using Microsoft.AspNetCore.Authorization;
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
    public class PaymentsController : ControllerBase, IPaymentsController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ExercisesController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IPaymentGetaways _paymentGetaways;
        private readonly ISmsPanel _smsPanel;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly AppDb _context;

        public PaymentsController(UserManager<User> userManager
                                , IUnitOfWork unitOfWork
                                , ILogger<ExercisesController> logger
                                , IMapper mapper
                                , IPaymentGetaways paymentGetaways
                                , ISmsPanel smsPanel
                                , IHostingEnvironment hostingEnvironment
                                , AppDb context)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _paymentGetaways = paymentGetaways;
            _smsPanel = smsPanel;
            _hostingEnvironment = hostingEnvironment;
            _context = context;
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

                var link = await _paymentGetaways.GetLink(payment);

                return Ok(new PaymentLinkDTO()
                {
                    Link = link
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(ReadAll)}");
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

        // GET: api/Payments/Chart/{timespan}
        [Authorize(Policy = "GetAllPayments")]
        [HttpGet("Chart/{timespan}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChartPayments(string timespan)
        {
            try
            {
                IQueryable results;
                if (timespan == "month")
                    results = from payment in _context.Payments
                              where payment.CreatedDate > DateTime.Today.AddYears(-1)
                              group payment by payment.CreatedDate.Month into day
                              select new
                              {
                                  Time = day.Key,
                                  Total = day.Sum(p => p.Amount),
                              };
                else
                    results = from payment in _context.Payments
                              where payment.CreatedDate > DateTime.Today.AddDays(-7)
                              group payment by payment.CreatedDate.Date into day
                              select new
                              {
                                  Time = day.Key,
                                  Total = day.Sum(p => p.Amount),
                              };

                return Ok(results);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(ChartPayments)}");
                return StatusCode(500, "Internal Server Error, Please try again later.");
            }
        }

        // GET: api/Payments/Discounts
        [Authorize]
        [HttpGet("Discounts", Name = "ReadDiscounts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReadDiscounts([FromQuery] QueryParamsDTO qp)
        {
            try
            {
                var paymentDiscounts = _unitOfWork.PaymentDiscounts.GetPage(
                    page: qp.Page,
                    itemsPerPage: qp.ItemsPerPage);
                await paymentDiscounts.ToListAsync();
                var result = _mapper.Map<PaymentDiscountPageAllItemDTO>(paymentDiscounts);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(ReadDiscountById)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        // GET: api/Payments/Discounts/<uuid>
        [Authorize]
        [HttpGet("Discounts/{id:Guid}", Name = "ReadDiscountById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReadDiscountById(Guid id)
        {
            try
            {
                var paymentDiscount = await _unitOfWork.PaymentDiscounts.Get(q => q.Id == id, new List<string> { "Payments" });
                var result = _mapper.Map<PaymentDiscountDTO>(paymentDiscount);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(ReadDiscountById)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        // POST: api/Payments/Discounts
        [Authorize]
        [HttpPost("Discounts")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> NewDiscount([FromBody] CreatePaymentDiscountDTO createPaymentDiscountDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(NewDiscount)}");
                return BadRequest(ModelState);
            }

            try
            {
                var paymentDiscount = _mapper.Map<PaymentDiscount>(createPaymentDiscountDTO);

                await _unitOfWork.PaymentDiscounts.Insert(paymentDiscount);
                await _unitOfWork.Save();

                return CreatedAtRoute("ReadDiscountById", new { id = paymentDiscount.Id }, paymentDiscount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(NewDiscount)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

    }
}
