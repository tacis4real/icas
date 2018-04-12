using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAdminStacks.DataContract;
using WebAdminStacks.Infrastructure;
using WebAdminStacks.Infrastructure.Contract;
using WebCribs.TechCracker.WebCribs.TechCracker;

namespace WebAdminStacks.Repository
{
    internal class ClientAdminRoleRepository
    {
        //private readonly IWebAdminRepository<RoleClient> _repository;
        private readonly IWebAdminRepository<ClientRole> _repository;
        private readonly WebAdminUoWork _uoWork;

        public ClientAdminRoleRepository()
        {
            _uoWork = new WebAdminUoWork();
            _repository = new WebAdminRepository<ClientRole>(_uoWork);
        }


        #region Client Roles




        //public List<ClientRole> GetClientRolesByClientProfileId(Int64 clientProfileId)
        //{
        //    try
        //    {
        //        var myItemList = _repository.GetAll().Where(m => m.ClientProfileId == clientProfileId);
        //        if (!myItemList.Any()) return null;
        //        return myItemList.ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
        //        return null;
        //    }
        //}


        //public ClientRole GetClientRole(int clientRoleId)
        //{
        //    try
        //    {
        //        var myItem = _repository.GetById(clientRoleId);
        //        if (myItem == null || myItem.ClientRoleId < 1) { return null; }
        //        return myItem;
        //    }
        //    catch (Exception ex)
        //    {
        //        BugManager.LogApplicationBug(ex.StackTrace, ex.Source, ex.Message);
        //        return null;
        //    }
        //}

        #endregion
    }
}
