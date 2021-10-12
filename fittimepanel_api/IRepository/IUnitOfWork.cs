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
        Task Save();
    }
}
