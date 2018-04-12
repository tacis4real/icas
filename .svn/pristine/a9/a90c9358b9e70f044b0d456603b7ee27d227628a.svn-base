using System;
using System.Data.Entity;
using ICASStacks.Infrastructure.Contract;

namespace ICASStacks.Infrastructure
{
    public class IcasUoWork : IIcasUoWork, IDisposable
    {

        private readonly IcasContext _dbContext;

        public IcasUoWork(IcasContext context)
        {
            _dbContext = context;
        }

        public IcasUoWork()
        {
            _dbContext = new IcasContext();
        }


        public DbContextTransaction BeginTransaction()
        {
            return _dbContext.IcasDbContext.Database.BeginTransaction();
        }
        
        public void SaveChanges()
        {
            _dbContext.IcasDbContext.SaveChanges();
        }

        public IcasContext Context { get { return _dbContext; } }


        #region Implementation of IDispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
           if(!disposing) return;
            if(_disposed) return;
            _dbContext.Dispose();
            _disposed = true;
        }

        private bool _disposed;

        ~IcasUoWork()
        {
            Dispose(false);
        }

        #endregion


    }
}
