using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAdminStacks.DataManager;
using WebAdminStacks.Infrastructure.Contract;

namespace WebAdminStacks.Infrastructure
{

    #region Open Acess Context

    public class WebAdminOpenContext : IWebAdminOpenContext
    {
        public WebAdminOpenContext()
        {
            WebAdminDbContext = new WebAdminStackEntities();
            WebAdminDbContext.Configuration.LazyLoadingEnabled = false;
        }

        public void Dispose()
        {
            WebAdminDbContext.Dispose();
        }

        public DbContext WebAdminDbContext { get; private set; }
    }

    #endregion
    


    public class WebAdminContext : IWebAdminContext
    {
        public WebAdminContext()
        {
            WebAdminDbContext = new WebAdminStackEntities();
            WebAdminDbContext.Configuration.LazyLoadingEnabled = false;
        }

        public void Dispose()
        {
            WebAdminDbContext.Dispose();
        }

        public DbContext WebAdminDbContext { get; private set; }
    }


}
