using FittimePanelApi.IRepository;
using FittimePanelApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FittimePanelApi.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDb _context;
        private IGenericRepository<Ticket> _tickets;
        private IGenericRepository<TicketStatus> _ticketStatuses;
        private IGenericRepository<Exercise> _exercises;
        private IGenericRepository<ExerciseType> _exerciseTypes;
        private IGenericRepository<ExerciseBlob> _exerciseBlobs;
        private IGenericRepository<ExerciseDownload> _exerciseDownloads;
        private IGenericRepository<User> _users;
        private IGenericRepository<UserMeta> _userMetas;
        private IGenericRepository<UserBlob> _userBlobs;
        private IGenericRepository<Payment> _payments;
        private IGenericRepository<PaymentGetaway> _paymentGetaways;
        private IGenericRepository<PaymentDiscount> _paymentDiscounts;

        public UnitOfWork(AppDb context)
        {
            _context = context;
        }

        public IGenericRepository<Ticket> Tickets => _tickets ??= new GenericRepository<Ticket>(_context);
        public IGenericRepository<TicketStatus> TicketStatuses => _ticketStatuses ??= new GenericRepository<TicketStatus>(_context);
        public IGenericRepository<Exercise> Exercises => _exercises ??= new GenericRepository<Exercise>(_context);
        public IGenericRepository<ExerciseType> ExerciseTypes => _exerciseTypes ??= new GenericRepository<ExerciseType>(_context);
        public IGenericRepository<ExerciseBlob> ExerciseBlobs => _exerciseBlobs ??= new GenericRepository<ExerciseBlob>(_context);
        public IGenericRepository<ExerciseDownload> ExerciseDownloads => _exerciseDownloads ??= new GenericRepository<ExerciseDownload>(_context);
        public IGenericRepository<User> Users => _users ??= new GenericRepository<User>(_context);
        public IGenericRepository<UserMeta> UserMetas => _userMetas ??= new GenericRepository<UserMeta>(_context);
        public IGenericRepository<UserBlob> UserBlobs => _userBlobs ??= new GenericRepository<UserBlob>(_context);
        public IGenericRepository<Payment> Payments => _payments ??= new GenericRepository<Payment>(_context);
        public IGenericRepository<PaymentGetaway> PaymentGetaways => _paymentGetaways ??= new GenericRepository<PaymentGetaway>(_context);
        public IGenericRepository<PaymentDiscount> PaymentDiscounts => _paymentDiscounts ??= new GenericRepository<PaymentDiscount>(_context);

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
