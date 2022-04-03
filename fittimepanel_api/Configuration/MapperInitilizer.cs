using AutoMapper;
using FittimePanelApi.Data;
using FittimePanelApi.Models;
using FittimePanelApi.Repository;
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
            CreateMap<User, UserProfileDTO>().ReverseMap();
            CreateMap<User, UserShortProfileDTO>().ReverseMap();
            CreateMap<User, ForgotPasswordDTO>().ReverseMap();
            CreateMap<User, UserShortListItemDTO>().ReverseMap();
            CreateMap<User, UserListItemDTO>().ReverseMap();
            CreateMap<User, UserProfileDetailDTO>().ReverseMap();
            CreateMap<Paginated<User>, UserPageItemDTO>().ReverseMap();
            CreateMap<UserMeta, UserMetaDTO>().ReverseMap();
            CreateMap<UserBlob, UserBlobDTO>().ReverseMap();
            CreateMap<UserBlob, UserBlobResponseDTO>().ReverseMap();
            CreateMap<Ticket, TicketDTO>().ReverseMap();
            CreateMap<Ticket, TicketListItemDTO>().ReverseMap();
            CreateMap<Ticket, CreateTicketDTO>().ReverseMap(); 
            CreateMap<Ticket, TicketListAllItemDTO>().ReverseMap();
            CreateMap<Paginated<Ticket>, TicketPageAllItemDTO>().ReverseMap(); 
            CreateMap<TicketMessage, TicketMessageDTO>().ReverseMap();
            CreateMap<TicketMessage, CreateTicketMessageDTO>().ReverseMap();
            CreateMap<TicketMessage, CreateNewTicketMessageDTO>().ReverseMap();
            CreateMap<TicketStatus, TicketStatusDTO>().ReverseMap();
            CreateMap<TicketStatus, CreateTicketStatusDTO>().ReverseMap();
            CreateMap<Exercise, ExerciseDTO>().ReverseMap();
            CreateMap<Exercise, CreateExerciseDTO>().ReverseMap();
            CreateMap<Exercise, ExerciseListItemDTO>().ReverseMap();
            CreateMap<Exercise, ExerciseDetailDTO>().ReverseMap();
            CreateMap<Paginated<Exercise>, ExercisePageAllItemDTO>().ReverseMap();
            CreateMap<Exercise, ExerciseListAllItemDTO>().ReverseMap();
            CreateMap<ExerciseType, CreateExerciseTypeDTO>().ReverseMap();
            CreateMap<ExerciseType, ExerciseTypeDTO>().ReverseMap();
            CreateMap<ExerciseMeta, CreateExerciseMetaDTO>().ReverseMap();
            CreateMap<ExerciseMeta, ExerciseMetaDTO>().ReverseMap();
            CreateMap<ExerciseBlob, ExerciseBlobDTO>().ReverseMap();
            CreateMap<ExerciseBlob, ExerciseBlobResponseDTO>().ReverseMap();
            CreateMap<ExerciseDownload, CreateExerciseDownloadDTO>().ReverseMap();
            CreateMap<ExerciseDownload, ExerciseDownloadDTO>().ReverseMap();
            CreateMap<Payment, CreatePaymentDTO>().ReverseMap();
            CreateMap<Payment, PaymentDTO>().ReverseMap();
            CreateMap<Payment, PaymentDetailDTO>().ReverseMap();
            CreateMap<PaymentGetaway, PaymentGetawayDTO>().ReverseMap();
            CreateMap<PaymentDiscount, DiscardInfoDTO>().ReverseMap();
            CreateMap<PaymentDiscount, CreatePaymentDiscountDTO>().ReverseMap();
            CreateMap<PaymentDiscount, PaymentDiscountDTO>().ReverseMap();
            CreateMap<PaymentDiscount, PaymentDiscountListAllItemDTO>().ReverseMap();
            CreateMap<Paginated<PaymentDiscount>, PaymentDiscountPageAllItemDTO>().ReverseMap();
        }
    }
}
