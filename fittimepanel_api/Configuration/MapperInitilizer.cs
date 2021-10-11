using AutoMapper;
using FittimePanelApi.Data;
using FittimePanelApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FittimePanelApi.Configuration
{
    public class MapperInitilizer : Profile
    {
        public MapperInitilizer()
        {
            CreateMap<Ticket, TicketDTO>().ReverseMap();
            CreateMap<Ticket, CreateTicketDTO>().ReverseMap();
            CreateMap<TicketMessage, TicketMessageDTO>().ReverseMap();
            CreateMap<TicketMessage, CreateTicketMessageDTO>().ReverseMap();
            CreateMap<TicketStatus, TicketStatusDTO>().ReverseMap();
            CreateMap<TicketStatus, CreateTicketStatusDTO>().ReverseMap();
        }
    }
}
