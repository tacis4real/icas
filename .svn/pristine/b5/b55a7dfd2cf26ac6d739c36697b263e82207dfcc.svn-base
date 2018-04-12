using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAdminStacks.Infrastructure.Contract;

namespace WebAdminStacks.Infrastructure
{


    #region Open Access
    public class WebAdminOpenUoWork : IWebAdminOpenUoWork, IDisposable
    {

        private readonly WebAdminOpenContext _dbContext;
        
        public WebAdminOpenUoWork(WebAdminOpenContext context)
        {
            _dbContext = context;
        }

        public WebAdminOpenUoWork()
        {
            _dbContext = new WebAdminOpenContext();
        }


        public DbContextTransaction BeginTransaction()
        {
            return _dbContext.WebAdminDbContext.Database.BeginTransaction();
        }

        public void SaveChanges()
        {
            _dbContext.WebAdminDbContext.SaveChanges();
        }

        public WebAdminOpenContext Context { get { return _dbContext; } }

        #region Implementation of IDispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (_diposed) return;
            _dbContext.Dispose();
            _diposed = true;
        }

        private bool _diposed;

        ~WebAdminOpenUoWork()
        {
            Dispose(false);
        }

        #endregion


    }

    #endregion
    

    public class WebAdminUoWork : IWebAdminUoWork, IDisposable
    {

        private readonly WebAdminContext _dbContext;

        public WebAdminUoWork(WebAdminContext context)
        {
            _dbContext = context;
        }

        public WebAdminUoWork()
        {
            _dbContext = new WebAdminContext();
        }


        public DbContextTransaction BeginTransaction()
        {
            return _dbContext.WebAdminDbContext.Database.BeginTransaction();
        }
        
        public void SaveChanges()
        {
            _dbContext.WebAdminDbContext.SaveChanges();
        }

        public WebAdminContext Context { get { return _dbContext; } }

        #region Implementation of IDispose
        public void Dispose()
        {
            Dispose(true); 
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if(!disposing) return;
            if(_diposed) return;
            _dbContext.Dispose();
            _diposed = true;
        }

        private bool _diposed;

        ~WebAdminUoWork()
        {
            Dispose(false);
        }

        #endregion

        
    }
}
