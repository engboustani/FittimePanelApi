﻿using FittimePanelApi.IRepository;
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

        public UnitOfWork(AppDb context)
        {
            _context = context;
        }
        public IGenericRepository<Ticket> Countries => _tickets ??= new GenericRepository<Ticket>(_context);

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