using FittimePanelApi.Data;
using FittimePanelApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FittimePanelApi.IControllers
{
    public interface ITicketsController : IController<Ticket>
    {
        public Task<IActionResult> New([FromBody] CreateTicketDTO createTicketDTO);
        public Task<IActionResult> Update(Guid id, CreateNewTicketMessageDTO createNewTicketMessageDTO);

    }
}
