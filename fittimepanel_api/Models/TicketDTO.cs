using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FittimePanelApi.Models
{
    public class CreateTicketDTO
    {
        public string Title { get; set; }
    }

    public class TicketDTO : CreateTicketDTO
    {
        public Guid Id { get; set; }
        public IList<TicketMessageDTO> TicketMessages { get; set; }
        public IList<TicketStatusDTO> TicketStatuses { get; set; }
    }

    public class CreateTicketMessageDTO
    {
        public Guid TicketId { get; set; }
        public string Text { get; set; }
    }

    public class TicketMessageDTO : CreateTicketMessageDTO
    {
        public Guid Id { get; set; }
        public TicketDTO Ticket { get; set; }
    }

    public class CreateTicketStatusDTO
    {
        public Guid TicketId { get; set; }
        public int Status { get; set; }
        public string Text { get; set; }
    }

    public class TicketStatusDTO : CreateTicketStatusDTO
    {
        public Guid Id { get; set; }
    }
}
