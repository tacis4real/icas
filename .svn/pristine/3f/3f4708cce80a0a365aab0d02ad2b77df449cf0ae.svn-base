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
    internal class ClientRoleRepository
    {
        private readonly IWebAdminRepository<ClientRole> _repository;
        private readonly WebAdminUoWork _uoWork;

        public ClientRoleRepository()
        {
            _uoWork = new WebAdminUoWork();
            _repository = new WebAdminRepository<ClientRole>(_uoWork);
        }

        public long AddClientRole(ClientRole clientRole)
        {
            try
            {
                var processedClientRole = _repository.Add(clientRole);
                _uoWork.SaveChanges();
                return processedClientRole.ClientRoleId;
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return 0;
            }
        }



        public List<ClientRole> GetClientRolesByClientProfileId(Int64 clientProfileId)
        {
            try
            {
                var myItemList = _repository.GetAll().Where(m => m.ClientProfileId == clientProfileId);
                if (!myItemList.Any()) return null;
                return myItemList.ToList();
            }
            catch (Exception ex)
            {
                BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }

        public ClientRole GetClientRole(int clientRoleId)
        {
            try
            {
                var myItem = _repository.GetById(clientRoleId);
                if (myItem == null || myItem.ClientRoleId < 1) { return null; }
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
