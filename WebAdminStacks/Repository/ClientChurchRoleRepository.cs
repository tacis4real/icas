using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAdminStacks.DataContract;
using WebAdminStacks.Infrastructure;
using WebAdminStacks.Infrastructure.Contract;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace WebAdminStacks.Repository
{
    internal class ClientChurchRoleRepository
    {

        private readonly IWebAdminRepository<ClientChurchRole> _repository;
        private readonly WebAdminUoWork _uoWork;

        public ClientChurchRoleRepository()
        {
            _uoWork = new WebAdminUoWork();
            _repository = new WebAdminRepository<ClientChurchRole>(_uoWork);
        }

        public List<ClientChurchRole> GetClientChurchRolesByClientChurchProfileId(Int64 clientChurchProfileId)
        {
            try
            {
                var myItemList = _repository.GetAll().Where(m => m.ClientChurchProfileId == clientChurchProfileId);
                if (!myItemList.Any()) return null;
                return myItemList.ToList();
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        public ClientChurchRole GetClientChurchRole(int clientChurchRoleId)
        {
            try
            {
                var myItem = _repository.GetById(clientChurchRoleId);
                if (myItem == null || myItem.ClientChurchRoleId < 1) { return null; }
                return myItem;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

    }
}
