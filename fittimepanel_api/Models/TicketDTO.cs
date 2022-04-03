using AutoMapper;
using FittimePanelApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FittimePanelApi.Models
{
    public class CreateTicketDTO
    {
        public string Title { get; set; }
        public IList<CreateNewTicketMessageDTO> TicketMessages { get; set; }
        public IList<CreateNewTicketStatusDTO> TicketStatuses { get; set; }
        public string CaptchaId { get; set; }
        public string UserEnteredCaptchaCode { get; set; }
    }

    public class TicketDTO : CreateTicketDTO
    {
        public Guid Id { get; set; }
        public IList<TicketMessageDTO> TicketMessages { get; set; }
        public IList<TicketStatusDTO> TicketStatuses { get; set; }
        public TicketStatusDTO LastStatus { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class TicketListItemDTO
    {
        public string Title { get; set; }
        public Guid Id { get; set; }
        public TicketStatusDTO LastStatus { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class TicketListAllItemDTO
    {
        public string Title { get; set; }
        public Guid Id { get; set; }
        public TicketStatusDTO LastStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public UserShortListItemDTO UserCreated { get; set; }
    }

    public class TicketPageAllItemDTO
    {
        public PageInfo PageInfo { get; set; }
        public IList<TicketListAllItemDTO> ItemsList { get; set; }
    }

    public class CreateNewTicketMessageDTO
    {
        public string Text { get; set; }
        public string CaptchaId { get; set; }
        public string UserEnteredCaptchaCode { get; set; }

    }

    public class CreateTicketMessageDTO : CreateNewTicketMessageDTO
    {
        public Guid TicketId { get; set; }
    }

    public class TicketMessageDTO : CreateTicketMessageDTO
    {
        public Guid Id { get; set; }
        public TicketDTO Ticket { get; set; }
        public UserShortListItemDTO User { get; set; }
    }

    public class CreateNewTicketStatusDTO
    {
        public int Status { get; set; }
        public string Text { get; set; }
    }


    public class CreateTicketStatusDTO : CreateNewTicketStatusDTO
    {
        public Guid TicketId { get; set; }
    }

    public class TicketStatusDTO : CreateTicketStatusDTO
    {
        public Guid Id { get; set; }
    }
}
