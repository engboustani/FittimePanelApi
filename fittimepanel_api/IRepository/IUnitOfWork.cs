using FittimePanelApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FittimePanelApi.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Ticket> Tickets { get; }
        IGenericRepository<TicketStatus> TicketStatuses { get; }
        IGenericRepository<Exercise> Exercises { get; }
        IGenericRepository<ExerciseType> ExerciseTypes { get; }
        IGenericRepository<ExerciseBlob> ExerciseBlobs { get; }
        IGenericRepository<ExerciseDownload> ExerciseDownloads { get; }
        IGenericRepository<User> Users { get; }
        IGenericRepository<UserMeta> UserMetas { get; }
        IGenericRepository<UserBlob> UserBlobs { get; }
        IGenericRepository<Payment> Payments { get; }
        IGenericRepository<PaymentGetaway> PaymentGetaways { get; }
        IGenericRepository<PaymentDiscount> PaymentDiscounts { get; }
        Task Save();
    }
}
