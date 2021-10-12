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

namespace FittimePanelApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase, ITicketsController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public TicketsController(IUnitOfWork unitOfWork
                                ,ILogger<TicketsController> logger
                                ,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        // GET: api/Tickets
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ReadAll()
        {
            try
            {
                var tickets = await _unitOfWork.Tickets.GetAll();
                var result = _mapper.Map<IList<TicketDTO>>(tickets);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(ReadAll)}");
                return StatusCode(500, "Internal Server Error, Please try again later.");
            }
        }

        // GET: api/Tickets/<uuid>
        [HttpGet("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ReadById(Guid id)
        {
            try
            {
                var ticket = await _unitOfWork.Tickets.Get(q => q.Id == id, new List<string> { "TicketMessages" , "TicketStatuses" });
                var result = _mapper.Map<TicketDTO>(ticket);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(ReadById)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        //// PUT: api/Tickets/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for
        //// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPut("{id}")]
        //public async Task<IActionResult> Update(Guid id, object newData)
        //{
        //    Ticket ticket = _context.Tickets.Find(id);
        //    TicketNewResponse _ticket = (TicketNewResponse)newData;

        //    if (id != ticket.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(ticket).State = EntityState.Modified;

        //    TicketMessage ticketMessage = new()
        //    {
        //        Text = _ticket.Description,
        //        Ticket = ticket
        //    };

        //    _context.TicketMessages.Add(ticketMessage);

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!Exist(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Tickets
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for
        //// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPost]
        //public async Task<ActionResult<Ticket>> New(object data)
        //{
        //    TicketNew _ticket = (TicketNew)data;

        //    Ticket ticket = new Ticket()
        //    {
        //        Title = _ticket.Title,
        //    };
        //    TicketMessage ticketMessage = new()
        //    {
        //        Text = _ticket.Description,
        //    };
        //    TicketStatus ticketStatus = new()
        //    {
        //        Status = 1,
        //        Text = "باز شده"
        //    };
        //    ticket.TicketMessages.Add(ticketMessage);
        //    ticket.TicketStatuses.Add(ticketStatus);

        //    _context.Tickets.Add(ticket);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetTicket", new { id = ticket.Id }, ticket);
        //}

        //// DELETE: api/Tickets/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<Ticket>> DeleteById(Guid id)
        //{
        //    var ticket = await _context.Tickets.FindAsync(id);
        //    if (ticket == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Tickets.Remove(ticket);
        //    await _context.SaveChangesAsync();

        //    return ticket;
        //}

        //public bool Exist(Guid id)
        //{
        //    return _context.Tickets.Any(e => e.Id == id);
        //}

    }
}
