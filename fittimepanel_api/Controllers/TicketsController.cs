using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FittimePanelApi;
using FittimePanelApi.Data;
using FittimePanelApi.IControllers;
using FittimePanelApi.IRepository;
using Microsoft.Extensions.Logging;
using AutoMapper;
using FittimePanelApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BotDetect.Web;

namespace FittimePanelApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase, ITicketsController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TicketsController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public TicketsController(UserManager<User> userManager
                                ,IUnitOfWork unitOfWork
                                ,ILogger<TicketsController> logger
                                ,IMapper mapper)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        // DELETE: api/Tickets/<uuid>
        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteById(Guid id)
        {
            try
            {
                var ticket = await _unitOfWork.Tickets.Get(q => q.Id == id);
                if (ticket == null)
                {
                    _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteById)}");
                    return BadRequest("Submitted data is invalid");
                }

                await _unitOfWork.Tickets.Delete(id);
                await _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(DeleteById)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        // POST: api/Tickets
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> New([FromBody] CreateTicketDTO createTicketDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(New)}");
                return BadRequest(ModelState);
            }

            try
            {
                SimpleCaptcha yourFirstCaptcha = new SimpleCaptcha();
                bool isHuman = yourFirstCaptcha.Validate(createTicketDTO.UserEnteredCaptchaCode, createTicketDTO.CaptchaId);
                if (!isHuman)
                {
                    _logger.LogError($"Invalid Captcha entered {nameof(New)}");
                    return BadRequest($"Invalid Captcha entered {nameof(New)}");
                }

                var ticket = _mapper.Map<Ticket>(createTicketDTO);
                var currentUser = await _userManager.GetUserAsync(User);
                ticket.UserCreated = currentUser;
                ticket.TicketMessages.First().User = currentUser;
                var ticketStatus = new TicketStatus()
                {
                    Status = 1,
                    Text = "در انتظار پاسخ"
                };
                ticket.TicketStatuses.Add(ticketStatus);

                await _unitOfWork.Tickets.Insert(ticket);
                await _unitOfWork.Save();

                return CreatedAtRoute("ReadTicketById", new { id = ticket.Id }, ticket);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(New)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        // GET: api/Tickets
        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReadAll()
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(User);
                var tickets = await _unitOfWork.Tickets.GetAll(
                    expression: t => t.UserCreated == currentUser,
                    includes: new List<string> { "TicketStatuses" });
                var result = _mapper.Map<IList<TicketListItemDTO>>(tickets);
                
                int index = 0;
                foreach (var ticket in result)
                {
                    if (tickets[index].TicketStatuses.Count == 0)
                    {
                        index++;
                        continue;
                    }
                    var last_status = _mapper.Map<TicketStatusDTO>(tickets[index].TicketStatuses.Last());
                    ticket.LastStatus = last_status;
                    index++;
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(ReadAll)}");
                return StatusCode(500, "Internal Server Error, Please try again later.");
            }
        }

        // GET: api/Tickets/All
        [Authorize(Policy = "GetAllTickets")]
        [HttpGet("All", Name = "ReadAllTickets")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReadAllTickets([FromQuery] QueryParamsDTO qp)
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(User);
                var tickets = _unitOfWork.Tickets.GetPage(
                    includes: new List<string> { "TicketStatuses", "UserCreated" },
                    page: qp.Page,
                    itemsPerPage: qp.ItemsPerPage);
                await tickets.ToListAsync();
                var result = _mapper.Map<TicketPageAllItemDTO>(tickets);

                int index = 0;
                foreach (var ticket in result.ItemsList)
                {
                    if (tickets.ItemsList[index].TicketStatuses.Count == 0)
                    {
                        index++;
                        continue;
                    }
                    var last_status = _mapper.Map<TicketStatusDTO>(tickets.ItemsList[index].TicketStatuses.Last());
                    ticket.LastStatus = last_status;
                    index++;
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(ReadAllTickets)}");
                return StatusCode(500, "Internal Server Error, Please try again later.");
            }
        }

        // GET: api/Tickets/<uuid>
        [Authorize]
        [HttpGet("{id:Guid}", Name = "ReadTicketById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReadById(Guid id)
        {
            try
            {
                var ticket = await _unitOfWork.Tickets.Get(q => q.Id == id, new List<string> { "TicketMessages" , "TicketStatuses", "TicketMessages.User" });
                var result = _mapper.Map<TicketDTO>(ticket);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(ReadById)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        // PUT: api/Tickets/<uuid>
        [Authorize]
        [HttpPut("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateNewTicketMessageDTO createNewTicketMessageDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(Update)}");
                return BadRequest(ModelState);
            }

            try
            {
                var currentUser = await _userManager.GetUserAsync(User);
                var roles = await _userManager.GetRolesAsync(currentUser);

                SimpleCaptcha yourFirstCaptcha = new SimpleCaptcha();
                bool isHuman = yourFirstCaptcha.Validate(createNewTicketMessageDTO.UserEnteredCaptchaCode, createNewTicketMessageDTO.CaptchaId);
                if (!isHuman && roles.Contains("User"))
                {
                    _logger.LogError($"Invalid Captcha entered {nameof(New)}");
                    return BadRequest($"Invalid Captcha entered {nameof(New)}");
                }

                var ticket = await _unitOfWork.Tickets.Get(q => q.Id == id);
                if (ticket == null)
                {
                    _logger.LogError($"Invalid UPDATE attempt in {nameof(Update)}");
                    return BadRequest("Submitted data is invalid");
                }

                var ticketMessage = _mapper.Map<TicketMessage>(createNewTicketMessageDTO);
                ticketMessage.User = currentUser;
                ticket.TicketMessages.Add(ticketMessage);
                if (roles.Contains("Administrator"))
                {
                    ticket.TicketStatuses.Add(new TicketStatus()
                    {
                        Status = 2,
                        Text = "پاسخ داده شده"
                    });
                }
                if (roles.Contains("User"))
                {
                    ticket.TicketStatuses.Add(new TicketStatus()
                    {
                        Status = 1,
                        Text = "در انتظار پاسخ"
                    });
                }
                _unitOfWork.Tickets.Update(ticket);
                await _unitOfWork.Save();

                return CreatedAtRoute("ReadTicketById", new { id = ticket.Id }, ticket);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(Update)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }
    }
}
