using System.Data.Entity;
using ICASStacks.DataManager;
using ICASStacks.Infrastructure.Contract;

namespace ICASStacks.Infrastructure
{
    public class IcasContext : IIcasContext
    {

        public IcasContext()
        {
            IcasDbContext = new IcasDataContext();
            IcasDbContext.Configuration.LazyLoadingEnabled = false;
        }


        public void Dispose()
        {
            IcasDbContext.Dispose();
        }

        public DbContext IcasDbContext { get; private set; }
    }
}
