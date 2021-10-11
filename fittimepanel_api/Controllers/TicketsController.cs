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

namespace FittimePanelApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase, ITicketsController
    {
        private readonly AppDb _context;

        public TicketsController(AppDb context)
        {
            _context = context;
        }

        // GET: api/Tickets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> ReadAll()
        {
            return await _context.Tickets.ToListAsync();
        }

        // GET: api/Tickets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> ReadById(Guid id)
        {
            var ticket = await _context.Tickets
                .Include(t => t.TicketMessages)
                .Include(t => t.TicketStatuses)
                .Where(t => t.Id == id)
                .FirstOrDefaultAsync();

            if (ticket == null)
            {
                return NotFound();
            }

            return ticket;
        }

        // PUT: api/Tickets/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, object newData)
        {
            Ticket ticket = _context.Tickets.Find(id);
            TicketNewResponse _ticket = (TicketNewResponse)newData;

            if (id != ticket.Id)
            {
                return BadRequest();
            }

            _context.Entry(ticket).State = EntityState.Modified;

            TicketMessage ticketMessage = new()
            {
                Text = _ticket.Description,
                Ticket = ticket
            };

            _context.TicketMessages.Add(ticketMessage);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Exist(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Tickets
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Ticket>> New(object data)
        {
            TicketNew _ticket = (TicketNew)data;

            Ticket ticket = new Ticket()
            {
                Title = _ticket.Title,
            };
            TicketMessage ticketMessage = new()
            {
                Text = _ticket.Description,
            };
            TicketStatus ticketStatus = new()
            {
                Status = 1,
                Text = "باز شده"
            };
            ticket.TicketMessages.Add(ticketMessage);
            ticket.TicketStatuses.Add(ticketStatus);
            
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTicket", new { id = ticket.Id }, ticket);
        }

        // DELETE: api/Tickets/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Ticket>> DeleteById(Guid id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();

            return ticket;
        }

        public bool Exist(Guid id)
        {
            return _context.Tickets.Any(e => e.Id == id);
        }

    }
}
