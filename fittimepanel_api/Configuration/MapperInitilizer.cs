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
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, LoginUserDTO>().ReverseMap();
            CreateMap<Ticket, TicketDTO>().ReverseMap();
            CreateMap<Ticket, CreateTicketDTO>().ReverseMap();
            CreateMap<TicketMessage, TicketMessageDTO>().ReverseMap();
            CreateMap<TicketMessage, CreateTicketMessageDTO>().ReverseMap();
            CreateMap<TicketMessage, CreateNewTicketMessageDTO>().ReverseMap();
            CreateMap<TicketStatus, TicketStatusDTO>().ReverseMap();
            CreateMap<TicketStatus, CreateTicketStatusDTO>().ReverseMap();
            CreateMap<Exercise, ExerciseDTO>().ReverseMap();
            CreateMap<Exercise, CreateExerciseDTO>().ReverseMap();
            CreateMap<ExerciseType, CreateExerciseTypeDTO>().ReverseMap();
            CreateMap<ExerciseType, ExerciseTypeDTO>().ReverseMap();
            CreateMap<ExerciseMeta, CreateExerciseMetaDTO>().ReverseMap();
            CreateMap<ExerciseMeta, ExerciseMetaDTO>().ReverseMap();
            CreateMap<ExerciseDownload, CreateExerciseDownloadDTO>().ReverseMap();
            CreateMap<ExerciseDownload, ExerciseDownloadDTO>().ReverseMap();
        }
    }
}
